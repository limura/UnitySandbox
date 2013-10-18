using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 2D画像に線を引いたりするためのもの
/// </summary>
public class GraphUtil {
	/// <summary>
	/// 保持しているテクスチャ
	/// </summary>
	Texture2D m_Texture2D = null;
	/// <summary>
	/// m_Texture2D に変更を加えたか否か
	/// 変更を加えたならば 使う前に Apply() する必要があるため それを覚えておきます
	/// </summary>
	bool m_Modified = false;
	
	// swap 関数
	static void Swap<T>(ref T lhs, ref T rhs)
	{
		T tmp;
		tmp = lhs;
		lhs = rhs;
		rhs = tmp;
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="GraphUtil"/> class.
	/// </summary>
	/// <param name='width'>
	/// 生成される2Dテクスチャの幅
	/// </param>
	/// <param name='height'>
	/// 生成される2Dテクスチャの高さ
	/// </param>
	public GraphUtil(int width, int height)
	{
		m_Texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
	}
	
	/// <summary>
	/// 現在の Texture2D を取得します
	/// </summary>
	/// <returns>
	/// 利用可能な Texture2D
	/// </returns>
	public Texture2D GetTexture2D()
	{
		if(m_Modified)
		{
			m_Texture2D.Apply();
			m_Modified = false;
		}
		return m_Texture2D;
	}
	
	public int Width
	{
		get { return m_Texture2D.width; }
	}
	public int Height
	{
		get { return m_Texture2D.height; }
	}
	
	public bool Resize(int width, int height)
	{
		return m_Texture2D.Resize(width, height);
	}
	
	public void Clear(Color c)
	{
		for(int y = 0; y < Height; ++y)
		{
			for(int x = 0; x < Width; ++x)
			{
				m_Texture2D.SetPixel(x, y, c);
			}
		}
		m_Modified = true;
	}
	
	public void DrawLine(Color color, Vector2 start, Vector2 end)
	{
		// http://ja.wikipedia.org/wiki/%E3%83%96%E3%83%AC%E3%82%BC%E3%83%B3%E3%83%8F%E3%83%A0%E3%81%AE%E3%82%A2%E3%83%AB%E3%82%B4%E3%83%AA%E3%82%BA%E3%83%A0
		// をそのまま持ってきた
		int x0 = (int)start.x;
		int y0 = (int)start.y;
		int x1 = (int)end.x;
		int y1 = (int)end.y;
		
		int dx = (int)Mathf.Abs(x1-x0);
		int dy = (int)Mathf.Abs(y1-y0);
		
		int sx = -1;
		if(x0 < x1)
		{
			sx = 1;
		}
		int sy = -1;
		if(y0 < y1)
		{
			sy = 1;
		}
		
		int err = dx - dy;
		
		while(true)
		{
			m_Texture2D.SetPixel(x0, y0, color);
			if(x0 == x1 && y0 == y1)
			{
				break;
			}
			float e2 = 2 * err;
			if( e2 > -dy )
			{
				err -= dy;
				x0 += sx;
			}
			else if(e2 < dx)
			{
				err += dx;
				y0 += sy;
			}
		}
		
		m_Modified = true;
	}
	
	public void DrawLines(Color color, Vector2[] points)
	{
		for(int n = 1; n < points.Length; ++n)
		{
			DrawLine(color, points[n-1], points[n]);
		}
	}

	public void DrawLines(Color color, List<Vector2> points)
	{
		for(int n = 1; n < points.Count; ++n)
		{
			DrawLine(color, points[n-1], points[n]);
		}
	}
}

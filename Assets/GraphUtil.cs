using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphUtil {
	Texture2D m_Texture2D = null;
	bool m_Modified = false;
	
	static void Swap<T>(ref T lhs, ref T rhs)
	{
		T tmp;
		tmp = lhs;
		lhs = rhs;
		rhs = tmp;
	}
	
	public GraphUtil(int width, int height)
	{
		m_Texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
	}
	
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
		float x0 = start.x;
		float y0 = start.y;
		float x1 = end.x;
		float y1 = end.y;
		bool steep = Mathf.Abs(y1 - y0) > Mathf.Abs(x1 - x0);
		if( steep )
		{
			Swap(ref x0, ref y0);
			Swap(ref x1, ref y1);
		}
		float delta_x = Mathf.Abs(x1 - x0);
		float delta_y = Mathf.Abs(y1 - y0);
		float err = delta_x / 2.0f;
		int y = (int)y0;
		int inc = (x0 < x1) ? 1 :-1;
		int ystep = (y0 < y1) ? 1: -1;
		
		
		for(int x = (int)x0; x < x1; x += inc)
		{
			if(steep)
			{
				m_Texture2D.SetPixel(y, x, color);
			}
			else
			{
				m_Texture2D.SetPixel(x, y, color);
			}
			err -= delta_y;
			if( err < 0.0f)
			{
				y += ystep;
				err += delta_x;
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

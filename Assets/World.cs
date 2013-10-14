using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
	
	/// <summary>
	/// 盤面の横幅
	/// </summary>
  	public int width = 50;
	/// <summary>
	/// 盤面の縦幅
	/// </summary>
  	public int height = 50;
	/// <summary>
	/// 初期の盤面に存在するcellの割合[%]
	/// </summary>
 	public int first_cell_percent = 40;
	/// <summary>
	/// ターンの更新間隔[ミリ秒？]
	/// </summary>
	public int interval = 20;
	/// <summary>
	/// cell のプレハブ
	/// </summary>
  	public GameObject cellPrefab = null;
	/// <summary>
	/// 現在の盤面
	/// </summary>
  	GameObject[] m_CellMap = null;
	
	// 新しいcellを作って返します.
	GameObject CreateNewCell(int x, int y, Quaternion prev_rotation)
	{
		GameObject obj = Instantiate(cellPrefab, new Vector3((x - width / 2.0f) * 1.5f, 0.0f, (y - height / 2.0f) * 1.5f), prev_rotation) as GameObject;
		return obj;
	}
	
	// cell に死ぬように司令します.
	void DestroyCell(GameObject[] cellMap, int x, int y)
	{
		GameObject obj = cellMap[x + y * width];
		if(obj == null)
		{
			return;
		}
		CellScript script = obj.GetComponent( typeof(CellScript) ) as CellScript;
		if(script == null)
		{
			return;
		}
		script.DeathEvent();
		
	}

	// Use this for initialization
	void Start ()
	{
    	m_CellMap = new GameObject[width * height];
		
		for(int n = 0; n < m_CellMap.Length; ++n)
		{
			m_CellMap[n] = null;
		}
        for(int n = 0; n < width * height * first_cell_percent / 100; ++n)
        {
        	int target = Random.Range(0, width * height - 1);
			if(m_CellMap[target] == null)
			{
            	m_CellMap[target] = CreateNewCell(target % width, target / width, Quaternion.identity);
			}
        }
	}
	
	// その位置のcellが存在するかどうかを取得します.
	bool GetCellStatus(GameObject[] cellMap, int x, int y)
    {
    	if( x < 0 || x >= width || y < 0 || y >= height || (x + y * width) > cellMap.Length )
        {
        	return false;
        }
    	return cellMap[x + y * width] != null;
    }
	
	// Cellが居るなら n に1を加えて返します
	int AddIfCellAlive(GameObject[] cellMap, int x, int y, int n)
    {
    	if(GetCellStatus(cellMap, x, y))
        {
        	return n + 1;
        }
    	return n;
    }
	
	// 次のターンでそのマスにcellが存在するかどうかを取得します.
	bool IsAlive(GameObject[] cellMap, int x, int y)
  	{
      	int n = 0;
      	n = AddIfCellAlive(cellMap, x - 1, y - 1, n);
      	n = AddIfCellAlive(cellMap, x - 0, y - 1, n);
      	n = AddIfCellAlive(cellMap, x + 1, y - 1, n);
      	n = AddIfCellAlive(cellMap, x - 1, y - 0, n);
      	//n = AddIfCellAlive(cellMap, x - 0, y - 0, n);
      	n = AddIfCellAlive(cellMap, x + 1, y - 0, n);
      	n = AddIfCellAlive(cellMap, x - 1, y + 1, n);
      	n = AddIfCellAlive(cellMap, x - 0, y + 1, n);
      	n = AddIfCellAlive(cellMap, x + 1, y + 1, n);
      	if( n < 2 || n > 3 )
      	{
          	return false;
      	}
      	return true;
  	}
	
	// cellの状態を１ターン分更新します.
  	void CellUpdate()
  	{
		// 怪しく現在の回転状態を拾ってきます(後で新規に作成されるcellが同じ角度から生成できるようにするためです)
		Quaternion prev_rotation = Quaternion.identity;
		foreach(GameObject obj in m_CellMap)
		{
			if(obj != null)
			{
				prev_rotation = obj.transform.rotation;
				break;
			}
		}

		// 新しく GameObject[] を作ってそちらに新しい状態を生成します.
      	GameObject[] newCellMap = new GameObject[width * height];
      	for(int n = 0; n < newCellMap.Length; n++)
      	{
          	if(IsAlive(m_CellMap, n % width, n / width))
          	{
				if(m_CellMap[n] == null)
				{
					newCellMap[n] = CreateNewCell(n % width, n / width, prev_rotation);
				}
				else
				{
					newCellMap[n] = m_CellMap[n];
				}
          	}
          	else
          	{
				if(m_CellMap[n] != null)
				{
					DestroyCell(m_CellMap, n % width , n / width);
				}
              	newCellMap[n] = null;
          	}
        }
      	m_CellMap = newCellMap;
    }

  	// Update is called once per frame
	void Update ()
	{
    	if (Time.frameCount % interval == 0)
        {
        	CellUpdate();
        }
	}
}

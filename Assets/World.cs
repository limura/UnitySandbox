using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

  	public int width = 50;
  	public int height = 50;
 	public int first_cell_percent = 40;
	public int interval = 20;
  	public GameObject cellPrefab = null;
  	GameObject[] m_CellMap = null;
	
	GameObject CreateNewCell(int x, int y, Quaternion prev_rotation)
	{
		GameObject obj = Instantiate(cellPrefab, new Vector3((x - width / 2.0f) * 1.5f, 0.0f, (y - height / 2.0f) * 1.5f), prev_rotation) as GameObject;
		return obj;
	}
	
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

	bool GetCellStatus(GameObject[] cellMap, int x, int y)
    {
    	if( x < 0 || x >= width || y < 0 || y >= height || (x + y * width) > cellMap.Length )
        {
        	return false;
        }
    	return cellMap[x + y * width] != null;
    }

	int AddIfCellAlive(GameObject[] cellMap, int x, int y, int n)
    {
    	if(GetCellStatus(cellMap, x, y))
        {
        	return n + 1;
        }
    	return n;
    }

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

  	void CellUpdate()
  	{
		Quaternion prev_rotation = Quaternion.identity;
		foreach(GameObject obj in m_CellMap)
		{
			if(obj != null)
			{
				prev_rotation = obj.transform.rotation;
				break;
			}
		}
		
      	GameObject[] newCellMap = new GameObject[width * height];
		int count = 0;
      	for(int n = 0; n < newCellMap.Length; n++)
      	{
          	if(IsAlive(m_CellMap, n % width, n / width))
          	{
				count++;
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
	void Update () {
    	if (Time.frameCount % interval == 0)
        {
        	CellUpdate();
        }
	}
}

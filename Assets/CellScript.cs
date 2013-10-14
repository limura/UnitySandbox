using UnityEngine;
using System.Collections;

public class CellScript : MonoBehaviour {
	
	enum CellState
	{
		CELL_STATE_BORN,
		CELL_STATE_IDLE,
		CELL_STATE_DEATH,
	};
	
	CellState m_State = CellState.CELL_STATE_BORN;
	float m_Size = 0.0f;
	
	void Start()
	{
		transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
		m_State = CellState.CELL_STATE_BORN;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (1.0f, 1.0f, 1.0f);
		switch(m_State)
		{
		case CellState.CELL_STATE_BORN:
			m_Size += 0.1f;
			if(m_Size > 1.0f)
			{
				m_Size = 1.0f;
				m_State = CellState.CELL_STATE_IDLE;
			}
			transform.localScale = new Vector3(m_Size, m_Size, m_Size);
			break;
		case CellState.CELL_STATE_DEATH:
			m_Size -= 0.1f;
			if(m_Size < 0.0f)
			{
				m_Size = 0.0f;
				Destroy(gameObject);
			}
			transform.localScale = new Vector3(m_Size, m_Size, m_Size);
			break;
		default:
			break;
		}
	}
	
	public void DeathEvent()
	{
		m_State = CellState.CELL_STATE_DEATH;
	}
}

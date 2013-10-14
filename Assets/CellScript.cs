using UnityEngine;
using System.Collections;

public class CellScript : MonoBehaviour {
	/// <summary>
	/// Cellの状態
	/// </summary>
	enum CellState
	{
		/// <summary>
		/// 生まれた
		/// </summary>
		CELL_STATE_BORN,
		/// <summary>
		/// 生き続けている
		/// </summary>
		CELL_STATE_IDLE,
		/// <summary>
		/// 死につつある
		/// </summary>
		CELL_STATE_DEATH,
	};
	
	/// <summary>
	/// 現在の状態
	/// </summary>
	CellState m_State = CellState.CELL_STATE_BORN;
	/// <summary>
	/// 現在のサイズ
	/// </summary>
	float m_Size = 0.0f;
	
	void Start()
	{
		transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
		m_State = CellState.CELL_STATE_BORN;
	}
	
	// Update is called once per frame
	void Update () {
		// なにやら蠢く感じを出したいなぁと思ってとりあえず回してみる
		transform.Rotate (1.0f, 1.0f, 1.0f);
		switch(m_State)
		{
		case CellState.CELL_STATE_BORN:
			// 生まれた時は 1.0 の大きさになるまでは膨らみます
			m_Size += 0.1f;
			if(m_Size > 1.0f)
			{
				m_Size = 1.0f;
				m_State = CellState.CELL_STATE_IDLE;
			}
			transform.localScale = new Vector3(m_Size, m_Size, m_Size);
			break;
		case CellState.CELL_STATE_DEATH:
			// 死ぬときは 0.0 以下になるまで縮みます.
			m_Size -= 0.1f;
			if(m_Size < 0.0f)
			{
				m_Size = 0.0f;
				Destroy(gameObject);
			}
			transform.localScale = new Vector3(m_Size, m_Size, m_Size);
			break;
		default:
			// 通常時は特に膨らみもしぼみもしません.
			break;
		}
	}
	
	/// <summary>
	/// 死亡すると宣告される場合に発生するイベントハンドラ
	/// </summary>
	public void DeathEvent()
	{
		m_State = CellState.CELL_STATE_DEATH;
	}
}

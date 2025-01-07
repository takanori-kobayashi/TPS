using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの足部分の動作
/// アタッチ:PlayerLower
/// </summary>
public class PlayerLower : MonoBehaviour
{
    private GameObject m_PlayerOb;
    Player m_PlayerSc;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerOb = GameObject.Find("Player");
        if (m_PlayerOb != null)
        {
            m_PlayerSc = m_PlayerOb.GetComponent<Player>();
        }
        else
        {
            Debug.Log("m_PlayerOb null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // インターミッション時
        if (GameState.STATE.INTERMISSION == GameState.m_GameStateNow)
        {
            //プレイヤーの向きに設定する
            this.transform.eulerAngles = new Vector3(Player.DEF_ROT_X, Player.DEF_ROT_Y, Player.DEF_ROT_Z);
        }

        if (m_PlayerSc.m_moveForward != Vector3.zero)
        {
            //瞬時に回転
            //transform.rotation = Quaternion.LookRotation(m_PlayerSc.m_moveForward);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(m_PlayerSc.m_moveForward),
                                                      0.2f);
        }
    }
}

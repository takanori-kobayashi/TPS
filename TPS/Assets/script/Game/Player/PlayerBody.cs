using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの胴体の動作
/// アタッチ:PlayerBody
/// </summary>
public class PlayerBody : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのカメラ
    /// </summary>
    public Camera m_PlayerCamera;

    // Start is called before the first frame update
    void Start()
    {
        if(m_PlayerCamera == null)
        {
            Debug.Log("m_PlayerCamera null");
        }
    }

    // Update is called once per frame
    void Update()
    {
      //  transform.rotation = Quaternion.LookRotation(m_PlayerCamera.transform.forward);
    }

    private void FixedUpdate()
    {
        // インターミッション時
        if ( GameState.STATE.INTERMISSION == GameState.m_GameStateNow )
        {
            //プレイヤーの向きに設定する
            this.transform.eulerAngles = new Vector3(Player.DEF_ROT_X, Player.DEF_ROT_Y, Player.DEF_ROT_Z );
        }
    }
}

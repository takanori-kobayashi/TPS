using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのダメージ処理
/// </summary>
public class PlayerDamage : MonoBehaviour
{
    /// <summary>
    /// ヒットチェック
    /// </summary>
    private bool m_hit = false;

    // Start is called before the first frame update
    void Start()
    {
        m_hit = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // 無敵
        //return;


        // インターミッション時は無敵
        if ( GameState.STATE.INTERMISSION == GameState.m_GameStateNow ||
             GameState.STATE.STAGECLEAR == GameState.m_GameStateNow ||
             GameState.STATE.GAMEOVER == GameState.m_GameStateNow)
        {
            return;
        }

        if (m_hit == true)
        {
            Player.PlayerLifeSub(10.0f);
            m_hit = false;
        }
    }

    /// <summary>
    /// トリガーの場合
    /// </summary>
    /// <param name="hit"></param>
    void OnTriggerStay(Collider hit)
    {
        // 接触対象のタグ
        if ( true == TagCheck.PlayerHitDamageCheck(hit))
        {
            m_hit = true;
        }
    }
}

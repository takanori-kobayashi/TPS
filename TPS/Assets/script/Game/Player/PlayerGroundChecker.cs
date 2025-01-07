using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤー地面接触判定処理
/// アタッチ:PlayerFoot
/// </summary>
public class PlayerGroundChecker : MonoBehaviour
{
    /// <summary>
    /// 接地した場合の処理
    /// </summary>
    public UnityEvent OnEnterGround;


    /// <summary>
    /// 地面から離れた場合の処理 
    /// </summary>
    public UnityEvent OnExitGround;

    /// <summary>
    /// 接触時のタグのテーブル
    /// </summary>
    readonly string[] TAG_TBL =
    {
        "Untagged", //未設定
        "Obstacle", //障害物
        "MoveStage",//移動する床
    };

    //private void OnTriggerEnter(Collider collision)
    private void OnTriggerStay(Collider collision)
    {
        /*
        for (int i = 0; i < TAG_TBL.Length; i++)
        {
            if (collision.tag == TAG_TBL[i])
            {
                OnEnterGround.Invoke();
            }
        }
        */
        OnEnterGround.Invoke();
    }

    private void OnTriggerExit(Collider collision)
    {
        OnExitGround.Invoke();
    }
}

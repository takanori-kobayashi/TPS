using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アタッチ:PlayerCameraPos
/// プレイヤーのカメラの位置(切り替え時の移動先)
/// </summary>
public class PlayerCameraPos : MonoBehaviour
{
    private const float H_Adjust = -5.2f; // C_{pH}の部分
    private const float V_Adjust = 1.7f; // C_{pV}の部分

    GameObject PlayerObj;
    GameObject PlayerDirectionObj;
    GameObject PlayerBodyObj;
    GameObject PlayerBarrelObj;
    GameObject PlayerLowerObj;


    private float c_x, c_y, c_z; // cameraの座標

    /// <summary>
    /// Playerのポジション
    /// </summary>
    public Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        // オブジェクト取得
        PlayerObj = GameObject.Find("Player");
        Assertion.Assert(PlayerObj);

        PlayerDirectionObj = GameObject.Find("PlayerDirection");
        Assertion.Assert(PlayerDirectionObj);

        PlayerBarrelObj = GameObject.Find("PlayerBarrel");
        Assertion.Assert(PlayerBarrelObj);

        PlayerBodyObj = GameObject.Find("PlayerBody");
        Assertion.Assert(PlayerBodyObj);

        PlayerLowerObj = GameObject.Find("PlayerLower");
        Assertion.Assert(PlayerLowerObj);

        targetPos = PlayerBodyObj.transform.position;
    }

    /// <summary>
    /// 三人称視点カメラ
    /// </summary>
    public void SetTpsPos()
    {
        // 位置リセット
        c_x = H_Adjust * PlayerDirectionObj.transform.forward.x;
        c_y = V_Adjust;
        c_z = H_Adjust * PlayerDirectionObj.transform.forward.z;

        transform.LookAt(PlayerDirectionObj.transform);
        transform.eulerAngles = new Vector3(0, PlayerDirectionObj.transform.eulerAngles.y, 0);


        var offset = new Vector3(c_x, c_y, c_z);

        transform.position = PlayerDirectionObj.transform.position + offset;


        //角度を-180～180度の範囲に正規化する
        float angle = Mathf.Repeat(PlayerBodyObj.transform.eulerAngles.x + 180, 360) - 180;


        transform.RotateAround(PlayerObj.transform.position, transform.right, angle);

        //プレイヤーの向き
        //targetObjBody.transform.rotation = Quaternion.LookRotation(transform.forward);


    }


    /// <summary>
    /// 一人称視点のカメラ
    /// </summary>
    public void SetFpsPos()
    {
        targetPos = PlayerBarrelObj.transform.position;

        transform.position = targetPos;// DefaultPos;

        var lookAt = PlayerBarrelObj.transform.position;// + Vector3.up * HeightM;

    }

    /// <summary>
    /// カメラ向き正面セット
    /// </summary>
    public void SetFrontPos()
    {
#if true
        // 位置リセット
        c_x = H_Adjust * PlayerLowerObj.transform.forward.x;
        c_y = V_Adjust;
        c_z = H_Adjust * PlayerLowerObj.transform.forward.z;

        transform.LookAt(PlayerLowerObj.transform);
        transform.eulerAngles = new Vector3(0, PlayerLowerObj.transform.eulerAngles.y, 0);


        var offset = new Vector3(c_x, c_y, c_z);

        transform.position = PlayerLowerObj.transform.position + offset;


        //角度を-180～180度の範囲に正規化する
        float angle = Mathf.Repeat(PlayerBodyObj.transform.eulerAngles.x + 180, 360) - 180;


        transform.RotateAround(PlayerObj.transform.position, transform.right, angle);

        //プレイヤーの向き
        //targetObjBody.transform.rotation = Quaternion.LookRotation(transform.forward);
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }

}

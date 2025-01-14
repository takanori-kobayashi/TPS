using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 照準の座標取得
/// アタッチ:Player/PlayerBody/PlayerBarrel
/// </summary>
public class Aim : MonoBehaviour
{
    private const float RAYDISTANCE_TPS = 30.0f;
    private const float RAYDISTANCE_FPS = 60.0f;
    /// <summary>
    /// 照準の長さ
    /// </summary>
    private float m_rayDistance;

    /// <summary>
    /// レイヤーのマスク
    /// </summary>
    private int m_layerMask;

    /// <summary>
    /// 照準の座標
    /// </summary>
    public static Vector3 m_AimPos;

    /// <summary>
    /// 接触したタグ
    /// </summary>
    public static string m_OnTagName;


    /// <summary>
    /// タグ用辞書
    /// </summary>
    //private Dictionary<string, bool> dic;

    // Start is called before the first frame update
    void Start()
    {
        m_AimPos = new Vector3(0.0f, 0.0f, 0.0f);
        m_rayDistance = 30.0f;

        //Rayに反応しないレイヤー
        var LAYER_MASK_TBL = new string[]
        {
            "PlayerBullet",
            "EnemyBullet",
        };
        int tmp;

        // レイヤーのマスク
        // Rayに反応しないレイヤーを設定(これを設定しないと標準がガクつく)
        m_layerMask = 0;
        for (int i = 0; i < LAYER_MASK_TBL.Length; i++)
        {
            tmp = LayerMask.NameToLayer(LAYER_MASK_TBL[i]); //レイヤー番号取得
            m_layerMask |= 1 << tmp; //レイヤー番号分ビットシフトし設定
        }
        m_layerMask = ~m_layerMask; // ビット判定

        //tmp = LayerMask.NameToLayer("EnemyBullet");
        //m_layerMask = 1 << tmp;// 16384;
        //m_layerMask = ~m_layerMask;


    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        // RAYの長さ設定
        //if(PlayerCamera.CAMERATYPE_TPS == PlayerCamera.m_CameraType)
        if (Player.CAMERAMODE_TPS == Player.m_CameraMode)
        {
            m_rayDistance = RAYDISTANCE_TPS;
        }
        else if (Player.CAMERAMODE_FPS == Player.m_CameraMode)
        {
            m_rayDistance = RAYDISTANCE_FPS;
        }
        else
        {
            m_rayDistance = RAYDISTANCE_TPS;
        }

        //デバッグ表示
        Debug.DrawRay(transform.position, transform.forward * m_rayDistance, Color.red);

        if (Physics.Raycast(ray, out hit, m_rayDistance, m_layerMask))
        {
            if( true == TagCheck.AimHitCheck(hit.collider))
            {
                m_AimPos = hit.point;
                m_OnTagName = hit.collider.tag;
            }
            else
            {
                m_AimPos = ray.GetPoint(m_rayDistance);
                m_OnTagName = "";
            }
        }
        else
        {
            m_AimPos = ray.GetPoint(m_rayDistance);
            m_OnTagName = "";
        }
    }

    private void FixedUpdate()
    {

    }

    private void OnCollisionStay(Collision collision)
    {
    }
    
}

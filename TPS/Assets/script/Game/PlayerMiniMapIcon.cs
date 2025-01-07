using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerIconの処理
/// アタッチ:PlayerMiniMapIcon
/// </summary>
public class PlayerMiniMapIcon : MonoBehaviour
{
    /// <summary>
    /// プレイヤーカメラオブジェクト
    /// </summary>
    public GameObject m_PlayerCamObj;

    /// <summary>
    /// 角度
    /// </summary>
    Quaternion PlayerCameraRol;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCameraRol = m_PlayerCamObj.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 worldAngle = transform.eulerAngles;
        //アイコンをカメラの向きに合わせる
        worldAngle.y = m_PlayerCamObj.transform.eulerAngles.y;
        transform.eulerAngles = worldAngle;
    }
}

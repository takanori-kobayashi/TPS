using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ミニマップ表示用のカメラ
/// アタッチ:MiniMapCamera
/// </summary>
public class MiniMapCamera : MonoBehaviour
{
    private const float POS_Y = 100.0f;

    public GameObject m_PlayerObj;
    public GameObject m_PlayerCamObj;

    /// <summary>
    /// Playerのポジション
    /// </summary>
    Vector3 targetPos;

    /// <summary>
    /// 角度
    /// </summary>
    Quaternion PlayerCameraRol;

    // Start is called before the first frame update
    void Start()
    {
        //オブジェクトチェック
        Assertion.Assert(m_PlayerObj);
        Assertion.Assert(m_PlayerCamObj);

        targetPos = m_PlayerObj.transform.position;
        PlayerCameraRol = m_PlayerCamObj.transform.rotation;       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // targetの移動量分、自分（カメラ）も移動する
        transform.position = new Vector3(m_PlayerObj.transform.position.x, POS_Y, m_PlayerObj.transform.position.z);

        // カメラの回転
        Vector3 worldAngle = transform.eulerAngles;
        worldAngle.x = 90.0f;// transform.rotation.x;
        worldAngle.y = m_PlayerCamObj.transform.eulerAngles.y;// testy;//m_PlayerObj.transform.localRotation.y;
        worldAngle.z = m_PlayerObj.transform.localRotation.z;
        transform.eulerAngles = worldAngle;

    }
}

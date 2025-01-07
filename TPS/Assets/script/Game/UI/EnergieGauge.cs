using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// FPS時のエネルギーゲージ
/// アタッチ:EnergieGauge
/// </summary>
public class EnergieGauge : MonoBehaviour
{
    /// <summary>
    /// スライダーのオブジェクト
    /// </summary>
    public Slider m_slider;

    /// <summary>
    /// スライダーの子オブジェクト
    /// (Background)
    /// (Fill Area)
    /// </summary>
    private GameObject[] m_childObject;

    /// <summary>
    /// 値の一時保持用
    /// </summary>
    private float m_Tmp;

    // Start is called before the first frame update
    void Start()
    {
        // スライダーの子オブジェクト取得
        if (m_slider != null)
        {
            m_childObject = new GameObject[m_slider.transform.childCount];

            for (int i = 0; i < m_slider.transform.childCount; i++)
            {
                Transform childTransform = m_slider.transform.GetChild(i);
                m_childObject[i] = childTransform.gameObject;
            }
        }
        else
        {            
            //this.gameObject.SetActive(false);
            Assertion.Assert();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        m_Tmp = (float)(PlayerShot.m_fireIntervalFps / 60.0f);

        // ゲージの表示非表示
        if( Player.m_CameraMode == Player.CAMERAMODE_TPS )
        {
            // TPSは非表示
            GaugeDisp(false);
        }
        else if (Player.m_CameraMode == Player.CAMERAMODE_FPS)
        {
            // FPSは表示
            GaugeDisp(true);
        }
        else
        {
        }

        m_slider.value = m_Tmp;

        this.transform.position = Pos3DChange2DCamera.m_Aimpos;
        transform.Translate(Vector3.down * 140);
    }

    /// <summary>
    /// ゲージの表示・非表示
    /// </summary>
    /// <param name="bl"></param>
    private void GaugeDisp(bool bl)
    {
        if (m_slider != null)
        {
            for( int i =0; i< m_slider.transform.childCount; i++)
            {
                //子オブジェクトの表示・非表示
                m_childObject[i].SetActive(bl);
            }
        }
    }
}

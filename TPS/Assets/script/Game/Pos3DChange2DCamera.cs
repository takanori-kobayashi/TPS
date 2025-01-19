using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 照準の表示
/// アタッチ:Canvas/Aim
/// </summary>
public class Pos3DChange2DCamera : MonoBehaviour
{
    /// <summary>
    /// 照準のサイズ(TPS)
    /// </summary>
    private const float AIM_SIZE_TPS = 1.0f;

    /// <summary>
    /// 照準のサイズ(FPS)
    /// </summary>
    private const float AIM_SIZE_FPS = 5.0f;


    /// <summary>
    /// 照準が持っているカメラモード
    /// </summary>
    private int m_AimCameraMode = Player.CAMERAMODE_TPS;

    private Image image;
    public Camera MainCamera, UICamera;
   

    private RectTransform mRectTransform;

    /// <summary>
    /// 照準のサイズ
    /// </summary>
    private float m_AimScall = AIM_SIZE_TPS;

    /// <summary>
    /// 照準のサイズ変更中フラグ
    /// </summary>
    private bool m_AimScallChMv = false;

    public static Vector3 m_Aimpos;

    void Start()
    {
        mRectTransform = this.GetComponent<RectTransform>();

        //照準の色
        image = gameObject.GetComponent<Image>();
        image.color = new Color(1.0f, 0, 0, 0.5f);

        //mRectTransform.localScale = new Vector3(2,2,0);

    }

    private void AimColor()
    {
        if("Enemy" == Aim.m_OnTagName ||
           "EnemyBase" == Aim.m_OnTagName )
        {
            image.color = new Color(1.0f, 0, 0, 0.5f);
        }
        else
        {
            image.color = new Color(0.0f, 0, 0, 0.5f);
        }

    }

    /// <summary>
    /// 通常時の表示
    /// </summary>
    private void NmlState()
    {
        Vector3 CubeViewportPoint = new Vector3(0.0f, 0.0f, 0.0f); ;
        Vector3 labelPos = new Vector3(0.0f, 0.0f, 0.0f);

        AimColor();


        //Cubeがメインカメラのどこに表示されているかを取得
        CubeViewportPoint = MainCamera.WorldToViewportPoint(Aim.m_AimPos);
        CubeViewportPoint.z = 0;

        //UICameraでCubeViewportPointと同じ位置に表示するためのワールド座標を取得
        labelPos = UICamera.ViewportToScreenPoint(CubeViewportPoint);


        //Z座標指定
        labelPos.z = 0;


        transform.position = labelPos;

        // 他でアクセスできる変数にセット
        m_Aimpos = transform.position;
        //transform.position = new Vector3(100,100,0);

        // 照準のサイズ変更
        if (m_AimCameraMode != Player.m_CameraMode)
        {
            if (Player.CAMERAMODE_TPS == m_AimCameraMode)
            {
                m_AimCameraMode = Player.CAMERAMODE_FPS;
                m_AimScallChMv = true;

            }

            else if (Player.CAMERAMODE_FPS == m_AimCameraMode)
            {
                m_AimCameraMode = Player.CAMERAMODE_TPS;
                m_AimScallChMv = true;

            }
        }

        // 照準のサイズ変更動作
        if (true == m_AimScallChMv)
        {
            if (Player.CAMERAMODE_FPS == m_AimCameraMode)
            {
                m_AimScall += 0.5f;
                if (AIM_SIZE_FPS <= m_AimScall)
                {
                    m_AimScall = AIM_SIZE_FPS;
                    m_AimScallChMv = false;
                }
                mRectTransform.localScale = new Vector3(m_AimScall, m_AimScall, 0);
            }
            else if (Player.CAMERAMODE_TPS == m_AimCameraMode)
            {
                m_AimScall -= 0.5f;
                if (AIM_SIZE_TPS >= m_AimScall)
                {
                    m_AimScall = AIM_SIZE_TPS;
                    m_AimScallChMv = false;
                }
                mRectTransform.localScale = new Vector3(m_AimScall, m_AimScall, 0);
            }
            else
            {

            }

        }
    }

    /// <summary>
    /// ゲームオーバー時の状態
    /// </summary>
    private void GameOverState()
    {
        //非表示
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (true == Player.m_PlayerDeadFlg)
        {
            GameOverState();
        }
        else if( Player.CAMERAMODE_OTHER == Player.m_CameraMode )
        {

        }
        else
        {
            NmlState();
        }
    }
}

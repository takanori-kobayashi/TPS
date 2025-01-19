using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アタッチ:PlayerCamera
/// プレイヤーのカメラ
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    private const float POV_MAX = 60.0f;
    private const float POV_MIN = 30.0f;

    private const float GETAXIS_MAX = 1.0f;
    private const float GETAXIS_MIN = -1.0f;

    private const float PLAYERDISTANCE = 0.05f;

    public const int CAMERATYPE_TPS = Player.CAMERAMODE_TPS;
    public const int CAMERATYPE_FPS = Player.CAMERAMODE_FPS;
    public const int CAMERATYPE_OTHER = Player.CAMERAMODE_OTHER;

    /// <summary>
    /// Player用のゲームオブジェクト
    /// </summary>
    private GameObject PlayerObj;

    /// <summary>
    /// Playerの砲身
    /// </summary>
    private GameObject PlayerBarrelObj;

    /// <summary>
    /// Playerの体
    /// </summary>
    private GameObject PlayerBodyObj;

    /// <summary>
    /// Playerのカメラセット位置
    /// </summary>
    private GameObject PlayerCameraPosObj;

    /// <summary>
    /// Playerの向き
    /// </summary>
    private GameObject PlayerDirectionObj;

    /// <summary>
    /// Playerの足
    /// </summary>
    private GameObject PlayerLowerObj;

    /// <summary>
    /// Playerのポジション
    /// </summary>
    private Vector3 targetPos;

    /// <summary>
    /// Playerのコンポーネント
    /// </summary>
    private Player player;

    /// <summary>
    /// PlayerCameraPosのコンポーネント
    /// </summary>
    private PlayerCameraPos ComPlayerCameraPos;

    /// <summary>
    /// カメラのコンポーネント
    /// </summary>
    private Camera CameraComp;


    /// <summary>
    /// GameState用のゲームオブジェクト
    /// </summary>
    GameObject GameStateObj;

    /// <summary>
    /// プレイヤーカメラ自身のカメラタイプ
    /// </summary>
    private int m_CameraType;

    /// <summary>
    /// カメラ切り替えフラグ
    /// </summary>
    private bool m_CameraTypeChFlg = false;

    /// <summary>
    /// カメラ切り替え時移動中フラグ
    /// </summary>
    private bool m_CameraChMoveFlg = false;

    /// <summary>
    /// カメラ切り替え時のカメラ移動のスピードアップ
    /// (移動しながらだとカメラが追いつかないため加算していく)
    /// </summary>
    private float CameraChMoveSpeedUp = 0.0f;

    /// <summary>
    /// カメラの回転スピード
    /// </summary>
    //private readonly float ROTATE_SPEED = 10.0f;

    private bool bCameraChFlg = false;

    public float RotationSensitivity = 200f;// 感度

    Vector3 DefaultPos; //デフォルトの位

    private float mouseInputX = 0.0f;
    private float mouseInputY = 0.0f;


    public float DistanceToPlayerM = 100.0f;    // カメラとプレイヤーとの距離[m]
    public float SlideDistanceM = 0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    public float HeightM = 1.2f;            // 注視点の高さ[m]

    private Vector3 CameraMoveTowarsPos;
    private Vector3 CameraMoveTowarsRot;

    private Transform TransCameraTowars;

    void Start()
    {
        //オブジェクト取得
        PlayerObj = GameObject.Find("Player");
        Assertion.Assert(PlayerObj);

        PlayerBarrelObj = GameObject.Find("PlayerBarrel");
        Assertion.Assert(PlayerBarrelObj);

        PlayerBodyObj = GameObject.Find("PlayerBody");
        Assertion.Assert(PlayerBodyObj);

        PlayerCameraPosObj = GameObject.Find("PlayerCameraPos");
        Assertion.Assert(PlayerCameraPosObj);

        PlayerLowerObj = GameObject.Find("PlayerLower");
        Assertion.Assert(PlayerLowerObj);

        // コンポーネント取得
        player = PlayerObj.GetComponent<Player>();
        Assertion.Assert(player);

        ComPlayerCameraPos = PlayerCameraPosObj.GetComponent<PlayerCameraPos>();
        Assertion.Assert(ComPlayerCameraPos);

        CameraComp = GetComponent<Camera>();
        if( null == CameraComp )
        {
            Assertion.Assert();
        }

        DefaultPos = transform.position;

        targetPos = PlayerBodyObj.transform.position;

        if (PlayerObj == null)
        {
            Debug.LogError("ターゲットが設定されていない");
            Application.Quit();
        }

        for (int i = 0; i < PlayerObj.transform.childCount; i++)
        {
            if( "PlayerDirection" == PlayerObj.transform.GetChild(i).name )
            {
                PlayerDirectionObj = PlayerObj.transform.GetChild(i).gameObject;
                break;
            }
        }

        ResetCamera();

        //GameStateObj = GameObject.Find("GameState");
        //gamestate = GameStateObj.GetComponent<GameState>();

        var lookAt = PlayerObj.transform.position;// + Vector3.up * HeightM;
        transform.RotateAround(lookAt, transform.right, 0.0f);
    }

    /// <summary>
    /// カメラの位置リセット
    /// </summary>
    public void ResetCamera()
    {
        //SetTpsPosCamera();
    }


    public float H_Adjust = 14.8f; // C_{pH}の部分
    public float V_Adjust = 11.4f; // C_{pV}の部分
    private float c_x, c_y, c_z; // cameraの座標

    /// <summary>
    /// 三人称視点カメラ(リセット用)
    /// </summary>
    private void SetTpsPosCamera()
    {
        // 向きのリセット
        transform.LookAt(PlayerDirectionObj.transform);
        transform.eulerAngles = new Vector3(0, PlayerDirectionObj.transform.eulerAngles.y, 0);

        // 位置リセット
        c_x = -5.2f * PlayerDirectionObj.transform.forward.x;
        c_y = 1.7f;
        c_z = -5.2f * PlayerDirectionObj.transform.forward.z;

        var offset = new Vector3(c_x, c_y, c_z);       
        transform.position = PlayerDirectionObj.transform.position + offset;


        //角度を-180～180度の範囲に正規化する
        float angle = Mathf.Repeat(PlayerBodyObj.transform.eulerAngles.x + 180, 360) - 180;


        transform.RotateAround(PlayerObj.transform.position, transform.right, angle );

        //プレイヤーの向き
        //targetObjBody.transform.rotation = Quaternion.LookRotation(transform.forward);
    }


    /// <summary>
    /// 一人称視点のカメラ(未使用)
    /// </summary>
    private void SetFpsPosCamera()
    {
        targetPos = PlayerBodyObj.transform.position;


        transform.position = targetPos;// DefaultPos;

        var lookAt = PlayerBarrelObj.transform.position;// + Vector3.up * HeightM;
        transform.RotateAround(lookAt, transform.right, mouseInputY * 10.0f);
    }

    /// <summary>
    /// TPS視点時のカメラの動作
    /// </summary>
    private void MoveTpsCamera()
    {
        // 有効視野の変更(加算)
        // FixedUpdateに移動
        //if ( CameraComp.fieldOfView < POV_MAX )
        //{
        //    CameraComp.fieldOfView += 1;
        //
        //    if (CameraComp.fieldOfView > POV_MAX)
        //    {
        //        CameraComp.fieldOfView = POV_MAX;
        //    }
        //}

        // カメラ切り替え時の移動中ではない場合
        if ( false == m_CameraChMoveFlg )
        {
            // targetの移動量分、自分（カメラ）も移動する
            transform.position += PlayerObj.transform.position - targetPos;
            targetPos = PlayerObj.transform.position;


            // マウスの入力取得(X,Y)
            float MouseInputXtmp = Input.GetAxis("Mouse X");
            float MouseInputYtmp = Input.GetAxis("Mouse Y");

            //マウスの入力値の値をチェック
            //※GetAxisでは -1.0f～1.0fの値が返されるが範囲外が取得されることがあるため補正
            if (GETAXIS_MIN > MouseInputXtmp || GETAXIS_MAX < MouseInputXtmp)
            {
                MouseInputXtmp = 0.0f;
            }
            if (GETAXIS_MIN > MouseInputYtmp || GETAXIS_MAX < MouseInputYtmp)
            {
                MouseInputYtmp = 0.0f;
            }

            // マウスの移動量
            mouseInputX = MouseInputXtmp;// * Time.deltaTime * RotationSensitivity;
            mouseInputY = MouseInputYtmp;// * Time.deltaTime * RotationSensitivity;           

            // カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
            if (transform.forward.y > 0.6f && mouseInputY < 0)
            {
                mouseInputY = 0;
            }
            if (transform.forward.y < -0.6f && mouseInputY > 0)
            {
                mouseInputY = 0;
            }

            if (mouseInputX > 0)
            {
                // targetの位置のY軸を中心に、回転（公転）する                      
                //　transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * 200f);
                transform.RotateAround(targetPos, Vector3.up, mouseInputX * 10.0f);
            }
            if (mouseInputX < 0)
            {
                // targetの位置のY軸を中心に、回転（公転）する
                //transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * 200f);
                transform.RotateAround(targetPos, Vector3.up, mouseInputX * 10.0f);
            }

            var lookAt = PlayerObj.transform.position;
            transform.RotateAround(lookAt, transform.right, mouseInputY * 10.0f);

            if (1 < mouseInputY)
            {
                PlayerBodyObj.transform.rotation = Quaternion.LookRotation(transform.forward);
            }

            //プレイヤーの向き
            PlayerBodyObj.transform.rotation = Quaternion.LookRotation(transform.forward);
        }
    }

    /// <summary>
    /// FPS視点時のカメラの動作
    /// </summary>
    private void MoveFpsCamera()
    {
        // FixedUpdateに移動
        // 有効視野の変更(減算)
        //if (CameraComp.fieldOfView > POV_MIN)
        //{
        //    CameraComp.fieldOfView -= 1;
        //
        //    if( CameraComp.fieldOfView < POV_MIN )
        //    {
        //        CameraComp.fieldOfView = POV_MIN;
        //    }
        //}

        // targetの移動量分、自分（カメラ）も移動する
        transform.position += PlayerBarrelObj.transform.position - targetPos;
        targetPos = PlayerBarrelObj.transform.position;


        // マウスの移動量
        //mouseInputX = Input.GetAxis("Mouse X") * Time.deltaTime * RotationSensitivity;
        //mouseInputY = Input.GetAxis("Mouse Y") * Time.deltaTime * RotationSensitivity;
        mouseInputX = Input.GetAxis("Mouse X");
        mouseInputY = Input.GetAxis("Mouse Y");

        // カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
        if (transform.forward.y > 0.6f && mouseInputY < 0)
        {
            mouseInputY = 0;
        }
        if (transform.forward.y < -0.6f && mouseInputY > 0)
        {
            mouseInputY = 0;
        }

        if (mouseInputX > 0)
        {         // targetの位置のY軸を中心に、回転（公転）する
                  //transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * 200f);
            transform.RotateAround(targetPos, Vector3.up, mouseInputX * 10.0f);
        }
        if (mouseInputX < 0)
        {         // targetの位置のY軸を中心に、回転（公転）する
                  //transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * 200f);
            transform.RotateAround(targetPos, Vector3.up, mouseInputX * 10.0f);
        }


        var lookAt = PlayerBarrelObj.transform.position;// + Vector3.up * HeightM;
        transform.RotateAround(lookAt, transform.right, mouseInputY * 10.0f);
        //
        PlayerBodyObj.transform.rotation = Quaternion.LookRotation(transform.forward);
    }


    /// <summary>
    /// カメラ切り替え中の動作
    /// </summary>
    private void CameraChangeMove()
    {
        //カメラ切替
        if (Player.m_CameraMode != m_CameraType)
        {
            if (false == bCameraChFlg)
            {
                if (CAMERATYPE_TPS == m_CameraType)
                {
                    //SetFpsPosCamera();
                    m_CameraType = CAMERATYPE_FPS;
                    m_CameraTypeChFlg = true;
                    m_CameraChMoveFlg = true;


                    ComPlayerCameraPos.SetFpsPos();
                }
                else if (CAMERATYPE_FPS == m_CameraType)
                {
                    //SetTpsPosCamera();
                    m_CameraType = CAMERATYPE_TPS;
                    m_CameraTypeChFlg = true;
                    m_CameraChMoveFlg = true;

                    ComPlayerCameraPos.SetTpsPos();
                }
                else
                {
                    //スルー
                }

            }
            bCameraChFlg = true;
        }
        //死亡時のカメラ切り替え
        else if( Player.m_PlayerDeadFlg == true )
        {
            // FPS(もしくはその他)の場合はTPSに戻す
            if( CAMERATYPE_FPS == m_CameraType ||
                CAMERATYPE_OTHER == m_CameraType )
            {
                m_CameraType = CAMERATYPE_TPS;
                m_CameraTypeChFlg = true;
                m_CameraChMoveFlg = true;

                ComPlayerCameraPos.SetTpsPos();
            }            
        }
        else
        {
            bCameraChFlg = false;
        }


        if (m_CameraTypeChFlg == true)
        {
            m_CameraTypeChFlg = false;
            return;
        }

        //カメラ切り替え移動中フラグ
        float distance = Vector3.Distance(transform.position, ComPlayerCameraPos.transform.position);

        //カメラとプレイヤーの距離
        if (PLAYERDISTANCE >= distance)
        {
            targetPos = PlayerObj.transform.position;
            m_CameraChMoveFlg = false;
            CameraChMoveSpeedUp = 0.0f;
        }


        if (true == m_CameraChMoveFlg)
        {
            transform.position = Vector3.Lerp(transform.position, ComPlayerCameraPos.transform.position, (10.0f + CameraChMoveSpeedUp) * Time.deltaTime);
            CameraChMoveSpeedUp += 1.0f;
        }
    }

    /// <summary>
    /// カメラ向き正面移動
    /// </summary>
    private void CameraFrontMove()
    {
        if( true == Player.GetCameraFrontFlg() )
        {
            //角度を-180～180度の範囲に正規化する
            //float angle = Mathf.Repeat(PlayerLowerObj.transform.localEulerAngles.y + 180, 360) - 180;

            //transform.RotateAround(PlayerBodyObj.transform.position, Vector3.up, angle);
            //Player.CameraFrontMoveSet(false);
            //    //CameraChMoveFlg = true;
            //
            //
            ComPlayerCameraPos.SetFrontPos();
        }
    }

    void Update()
    {
#if true

        //死亡時とクリア時カメラくるくる
        //if(player.m_dead == true)
        //if (gamestate.m_state == gamestate.STATE_GAMEOVER ||
        //    gamestate.m_state == gamestate.STATE_GAMECLEAR)
        //{
        //    transform.RotateAround(targetPos, Vector3.up, Time.deltaTime * 10.0f);
        //    return;
        //}
        //CamaeraCh = Input.GetAxisRaw("視点変更");

        // インターミッション時
        if ( GameState.STATE.INTERMISSION == GameState.m_GameStateNow )
        {
            // 状態1の時の動作
            if (GameState.INTTERMISSION_STATE.STATE1 == GameState.m_InterMissionState)
            {
                //カメラくるくる
                transform.RotateAround(PlayerObj.transform.position, Vector3.up, Time.deltaTime * 50.0f);

                //プレイヤーにカメラを近づける
                transform.position = Vector3.Lerp(transform.position, PlayerObj.transform.position, Time.deltaTime * 0.25f);
            }

            // 状態2の時の動作
            else if (GameState.INTTERMISSION_STATE.STATE2 == GameState.m_InterMissionState)
            {
                // カメラ切り替え中の動作
                CameraChangeMove();

                // カメラ正面向き移動
                CameraFrontMove();

                //角度を-180～180度の範囲に正規化する
                float angle = Mathf.Repeat( transform.eulerAngles.x + 180, 360) - 180;
                if( 0 < angle )
                {
                    angle--;
                    if( angle < 0 )
                    {
                        angle = 0.0f;
                    }
                }
                else if ( angle < 0 )
                {
                    angle++;
                    if ( 0.0f < angle )
                    {
                        angle = 0.0f;
                    }
                }
                else
                {
                    angle = 0.0f;
                }
                transform.eulerAngles = new Vector3(angle, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
        else
        {
            // カメラ切り替え中の動作
            CameraChangeMove();

            // カメラ正面向き移動
            CameraFrontMove();

            // カメラの動き
            switch (m_CameraType)
            {
                case CAMERATYPE_TPS:
                    MoveTpsCamera();
                    break;
                case CAMERATYPE_FPS:
                    MoveFpsCamera();
                    break;                
                default:
                    MoveTpsCamera();
                    break;
            }
        }
#endif
    }


    private void FixedUpdate()
    {
        // インターミッション時
        if (GameState.STATE.INTERMISSION == GameState.m_GameStateNow)
        {
        }
        switch (m_CameraType)
        {
            case CAMERATYPE_TPS:
                // 有効視野の変更(加算)
                if (CameraComp.fieldOfView < POV_MAX)
                {
                    CameraComp.fieldOfView += 10;

                    if (CameraComp.fieldOfView > POV_MAX)
                    {
                        CameraComp.fieldOfView = POV_MAX;
                    }
                }
                break;
            case CAMERATYPE_FPS:
                // 有効視野の変更(減算)
                if (CameraComp.fieldOfView > POV_MIN)
                {
                    CameraComp.fieldOfView -= 10;

                    if (CameraComp.fieldOfView < POV_MIN)
                    {
                        CameraComp.fieldOfView = POV_MIN;
                    }
                }
                break;
            default:
                break;
        }
    }

    //インターミッション時のカメラの位置設定
    public void InitInterMissionCameraSet()
    {
        // カメラタイプをTPS
        m_CameraType = CAMERATYPE_TPS;

        //位置をセット
        transform.position = new Vector3(0.0f, 39.14f, -63.31f);
        //角度セット
        transform.eulerAngles = new Vector3(40.0f, 0.0f, 0.0f);
    }

    /// <summary>
    /// 最初にカメラの位置を移動させる
    /// </summary>
    public void InitCameraMove()
    {
        // カメラタイプをTPS
        m_CameraType = CAMERATYPE_TPS;

        CameraComp.fieldOfView = POV_MAX;

        // カメラ切り替えフラグON
        m_CameraTypeChFlg = true;

        // カメラ移動中フラグON
        m_CameraChMoveFlg = true;

        //角度Xをリセット
       // transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, transform.eulerAngles.z);
    }

}

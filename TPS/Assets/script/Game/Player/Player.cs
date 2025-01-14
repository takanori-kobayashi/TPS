using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの動作
/// アタッチ:Player
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのデフォルト位置X
    /// </summary>
    public const float DEF_POS_X = 0.0f;
    /// <summary>
    /// プレイヤーのデフォルト位置Y
    /// </summary>
    public const float DEF_POS_Y = 0.0f;
    /// <summary>
    /// プレイヤーのデフォルト位置Z
    /// </summary>
    public const float DEF_POS_Z = 0.0f;

    /// <summary>
    /// プレイヤーのデフォルト回転X
    /// </summary>
    public const float DEF_ROT_X = 0.0f;
    /// <summary>
    /// プレイヤーのデフォルト回転Y
    /// </summary>
    public const float DEF_ROT_Y = 0.0f;
    /// <summary>
    /// プレイヤーのデフォルト回転Z
    /// </summary>
    public const float DEF_ROT_Z = 0.0f;


    /// <summary>
    /// プレイヤーの最大ライフ
    /// </summary>
    public const float MAX_LIFE = 100.0f;

    /// <summary>
    /// プレイヤーの歩く速さ
    /// </summary>
    private const float MOVE_SPEED = 7.0f;

    /// <summary>
    /// 重力加速度最大値
    /// </summary>
    private const float MAX_ACC_GRAVITY = -15.5f;

    /// <summary>
    /// カメラモードTPS
    /// </summary>
    public const int CAMERAMODE_TPS = 0;

    /// <summary>
    /// カメラモードFPS
    /// </summary>
    public const int CAMERAMODE_FPS = 1;

    /// <summary>
    /// カメラモードその他
    /// </summary>
    public const int CAMERAMODE_OTHER = 2;


    /// <summary>
    /// ジャンプ力
    /// </summary>
    private const float JUMP_POWER = 5.0f;

    /// <summary>
    /// カメラモード
    /// </summary>
    public static int m_CameraMode { get; private set; }

    /// <summary>
    /// カメラ向き正面フラグ
    /// </summary>
    private static bool m_CameraFrontFlg ;

    /// <summary>
    /// 水平方向の入力
    /// </summary>
    float m_inputHorizontal;

    /// <summary>
    /// 垂直方向の入力
    /// </summary>
    float m_inputVertical;

    /// <summary>
    /// ジャンプのキー入力
    /// </summary>
    private float m_JumpKey;

    /// <summary>
    /// カメラモードのキー入力
    /// </summary>
    private float m_CameraModeKey = 0.0f;

    /// <summary>
    /// カメラ正面回転キー入力
    /// </summary>
    private float m_CameraFrontRotKey = 0.0f;

    /// <summary>
    /// カメラ正面回転キー入力フラグ
    /// </summary>
    private bool m_CameraFrontRotKeyFlg = false;

    /// <summary>
    /// カメラモードのキー入力中フラグ
    /// </summary>
    private bool m_CameraModeKyeFlg = false;

    /// <summary>
    /// プレイヤーの爆発までのカウント
    /// </summary>
    private int m_PlayerDeadCnt = 0;

    /// <summary>
    /// プレイヤーの爆発までの状態
    /// </summary>
    private int m_PlayerDeadState = 0;

    /// <summary>
    /// プレイヤーのライフ
    /// </summary>
    private static float m_PlayerLife = 100.0f;

    /// <summary>
    /// プレイヤーのダメージフラグ
    /// </summary>
    public static bool m_DamageFlg  { get; private set; } = false;

    /// <summary>
    /// 前フレームのプレイヤーのダメージフラグ
    /// </summary>
    public static bool m_DamageFlgOld { get; private set; } = false;

    /// <summary>
    /// プレイヤーの死亡フラグ
    /// </summary>
    public static bool m_PlayerDeadFlg { get; private set; } = false;

    /// <summary>
    /// Rigidbodyのコンポーネント
    /// </summary>
    Rigidbody m_rb;

    /// <summary>
    /// ジャンプ力
    /// </summary>
    public float m_JumpPower;

    /// <summary>
    /// ジャンプの状態
    /// </summary>
    public bool m_jumpFlg { get; set; }

    /// <summary>
    /// プレイヤーのカメラ
    /// </summary>
    public Camera m_PlayerCamera;

    /// <summary>
    /// 爆発エフェクト(パーティクル)
    /// </summary>
    public GameObject m_ExplosionObj;

    /// <summary>
    /// 爆発エフェクト(パーティクル)
    /// </summary>
    public GameObject m_BreakEffObj;

    /// <summary>
    /// プレイヤーの移動方向
    /// </summary>
    public Vector3 m_moveForward { get; private set; }

    /// <summary>
    /// プレイヤーを表示しているオブジェクト
    /// </summary>
    [SerializeField] private Renderer[] m_PlayerRendererObj = default;//Renderer型の変数aを宣言　好きなゲームオブジェクトをアタッチ

    // Start is called before the first frame update
    void Start()
    {
        m_jumpFlg = true;

        m_moveForward = Vector3.zero;

        m_rb = GetComponent<Rigidbody>();

        m_CameraMode = CAMERAMODE_OTHER;

        m_PlayerLife = 100.0f;
        m_PlayerDeadFlg = false;

        Assertion.Assert(m_ExplosionObj);
        Assertion.Assert(m_BreakEffObj);
       
    }

    // Update is called once per frame
    void Update()
    {
        //通常時とクリア時は操作可能
        if ( GameState.STATE.PLAY == GameState.m_GameStateNow ||
             GameState.STATE.STAGECLEAR == GameState.m_GameStateNow)
        {
            m_inputHorizontal = Input.GetAxisRaw("Horizontal");
            m_inputVertical = Input.GetAxisRaw("Vertical");
            m_JumpKey = Input.GetAxisRaw("Jump");
            m_CameraModeKey = Input.GetAxisRaw("視点変更");
            m_CameraFrontRotKey = Input.GetAxisRaw("正面回転");
        }
        else
        {
            m_inputHorizontal = 0;
            m_inputVertical = 0;
            m_JumpKey = 0;
            m_CameraModeKey = 0;
            m_CameraFrontRotKey = 0;
        }


    }

    private void FixedUpdate()
    {

        //if(m_PlayerDeadFlg == true)
        //{
        //    return;
        //}

        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(m_PlayerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * m_inputVertical + m_PlayerCamera.transform.right * m_inputHorizontal;
        m_moveForward = moveForward;

        var waitMove = new Vector3(0.0f, 0.0f, 0.0f);

        //停止中
        if(moveForward == Vector3.zero)
        {
            //ライフ回復
            if (m_PlayerLife < MAX_LIFE)
            {
                if ( false == m_PlayerDeadFlg )
                {
                    m_PlayerLife += 0.1f;
                }
            }
            if( MAX_LIFE <= m_PlayerLife )
            {
                m_PlayerLife = MAX_LIFE;
            }
        }

        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        m_rb.velocity = moveForward * MOVE_SPEED + new Vector3(0, m_rb.velocity.y, 0);


        //重力加速度の最大値
        if (m_rb.velocity.y <= MAX_ACC_GRAVITY)
        {
            m_rb.velocity = new Vector3(m_rb.velocity.x, MAX_ACC_GRAVITY, m_rb.velocity.z);
        }

        //ジャンプ
        if (0 != m_JumpKey)
        {

            if (!m_jumpFlg)
            {
                m_jumpFlg = true;
                m_rb.velocity = transform.up * JUMP_POWER;
            }

        }

        //カメラモード変更
        if (0 != m_CameraModeKey)
        {
            if (false == m_CameraModeKyeFlg)
            {
                if (CAMERAMODE_TPS == m_CameraMode)
                {
                    m_CameraMode = CAMERAMODE_FPS;
                }
                else if (CAMERAMODE_FPS == m_CameraMode)
                {
                    m_CameraMode = CAMERAMODE_TPS;
                }
                else
                {
                    m_CameraMode = CAMERAMODE_TPS;
                }

                m_CameraModeKyeFlg = true;
            }
        }
        else
        {
            m_CameraModeKyeFlg = false;
        }

        // カメラ向き正面
        if (0 != m_CameraFrontRotKey)
        {
            if (false == m_CameraFrontRotKeyFlg)
            {
                m_CameraFrontRotKeyFlg = true;

                m_CameraFrontFlg = true;

            }
        }
        else
        {
            m_CameraFrontRotKeyFlg = false;
        }

        // 穴に落ちた場合
        if( transform.position.y  <= - 10.0f )
        {
            // 穴におちて死ぬのは通常時
            if (GameState.STATE.PLAY == GameState.m_GameStateNow)
            {
                m_PlayerDeadFlg = true;
            }

        }

        // 前フレームのダメージフラグの情報をセット
        m_DamageFlgOld = m_DamageFlg;
        
        // ダメージのフラグをoffにする
        m_DamageFlg = false;

        // 死亡した場合
        if (true == m_PlayerDeadFlg)
        {
            //最初の状態
            if(0 == m_PlayerDeadState )
            {
                if (0 == m_PlayerDeadCnt)
                {
                    // 爆発エフェクトの複製
                    GameObject Oexp = Instantiate(m_BreakEffObj) as GameObject;

                    // 爆発エフェクトの位置を調整
                    Oexp.transform.position = this.transform.position;

                    // プレイヤーの子オブジェクトに設定
                    Oexp.transform.parent = this.transform;

                    //二秒後に削除
                    UnityEngine.Object.Destroy(Oexp, 2.0f);

                }
                m_PlayerDeadCnt++;

                //カウント
                if ( 60 < m_PlayerDeadCnt )
                {
                    m_PlayerDeadState = 1;
                }
            }
            //次の状態
            else if( 1 == m_PlayerDeadState )
            {
                if (m_ExplosionObj != null)
                {
                    // 爆発エフェクトの複製
                    GameObject Oexp = Instantiate(m_ExplosionObj) as GameObject;

                    // 爆発エフェクトの位置を調整
                    Oexp.transform.position = this.transform.position;

                    //二秒後に削除
                    UnityEngine.Object.Destroy(Oexp, 2.0f);
                }

                //UnityEngine.Object.Destroy(this.gameObject);
                // 爆発と同時に見た目を非表示に
                // ※Destroyだと関連付けたものがエラーになるため
                PlayerObjectDisp(false);

                //次の状態へ
                m_PlayerDeadState = 2;
            }
            //最後の状態
            else
            {
                // 爆発と同時に見た目を非表示に
                // ※Destroyだと関連付けたものがエラーになるため
                PlayerObjectDisp(false);
            }

            //保険用のオブジェクト破棄
            //UnityEngine.Object.Destroy(this.gameObject, 10.0f);            
        }
    }

    /// <summary>
    /// プレイヤーのオブジェクトの表示非表示
    /// </summary>
    private void PlayerObjectDisp(bool enable)
    {
        for (int i = 0; i < m_PlayerRendererObj.Length; i++)
        {
            m_PlayerRendererObj[i].enabled = enable;
        }
    }

    /// <summary>
    /// プレイヤーの状態の初期化
    /// </summary>
    public void InitPlayerState()
    {
        //初期位置
        this.transform.position = new Vector3(DEF_POS_X, DEF_POS_Y, DEF_POS_Z);
        
        //初期回転
        this.transform.eulerAngles = new Vector3(DEF_ROT_X, DEF_ROT_Y, DEF_ROT_Z);

        //ライフ
        m_PlayerLife = MAX_LIFE;

        // 入力の状態クリア
        m_inputHorizontal = 0.0f;
        m_inputVertical = 0.0f;
        m_JumpKey = 0.0f;
        m_CameraModeKey = 0.0f;
        m_CameraFrontRotKey = 0.0f;

        // カメラモードをTPSに
        m_CameraMode = CAMERAMODE_TPS;

    }

    /// <summary>
    /// カメラ向き移動フラグOFF
    /// </summary>
    public static void CameraFrontMoveSet( bool flg )
    {
        m_CameraFrontFlg = flg;
    }

    /// <summary>
    /// カメラ向き移動フラグ
    /// </summary>
    /// <returns></returns>
    public static bool GetCameraFrontFlg()
    {
        return m_CameraFrontFlg;
    }

    /// <summary>
    /// プレイヤーのライフ取得
    /// </summary>
    /// <returns></returns>
    public static float GetPlayerLife()
    {
        return m_PlayerLife;
    }

    /// <summary>
    /// プレイヤーのライフ減算
    /// </summary>
    /// <param name="damage"></param>
    public static void PlayerLifeSub( float damage )
    {
        m_PlayerLife -= damage;

        m_DamageFlg = true;

        if ( 0 >= m_PlayerLife )
        {
            m_PlayerLife = 0;            

            m_PlayerDeadFlg = true;
        }
    }

    /// <summary>
    /// カメラモード切替
    /// </summary>
    /// <param name="CameraMode"></param>
    public static void PlayerCameraModeChange( int CameraMode )
    {
        m_CameraMode = CameraMode;
    }
}

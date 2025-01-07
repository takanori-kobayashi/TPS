using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの弾発射処理
/// アタッチ:PlayerBarrel
/// </summary>
public class PlayerShot : MonoBehaviour
{
    /// <summary>
    /// ショットのインターバル(FPS時)
    /// </summary>
    public const int SHOT_INTERVAL_FPS = 60;

    /// <summary>
    /// bulletのゲームオブジェクト
    /// </summary>
    public GameObject bullet;

    /// <summary>
    /// bulletのゲームオブジェクト(FPSモード時)
    /// </summary>
    public GameObject bullet2;

    /// <summary>
    /// エフェクト(発射)
    /// </summary>
    public GameObject m_EffectFire;

    /// <summary>
    /// 弾丸発射点 
    /// </summary>
    public Transform muzzle;

    /// <summary>
    /// 弾丸の速度 
    /// </summary>
    public float speed = 10000.0f;

    /// <summary>
    /// fire1キーが押下状態
    /// </summary>
    private float m_fire1 = 0.0f;

    /// <summary>
    /// 弾の発射のインターバル
    /// </summary>
    private int m_fireInterval = 0;

    /// <summary>
    /// 弾の発射のインターバル(FPS)
    /// </summary>
    public static int m_fireIntervalFps { get; private set; }

    /// <summary>
    /// 弾を打った状態
    /// </summary>
    public bool m_fire1_flg { get; private set; }

    /// <summary>
    /// Rigidbodyのコンポーネント
    /// </summary>
    private Rigidbody rBody;

    /// <summary>
    /// Playerのゲームオブジェクト
    /// </summary>
    GameObject refObj;

    /// <summary>
    /// Playerのコンポーネント
    /// </summary>
    Player player;

    /// <summary>
    /// PlayerBarrelのオブジェクト
    /// </summary>
    public GameObject PlayerBarrelOb;

    /// <summary>
    /// サウンドファイル
    /// 通常弾
    /// </summary>
    public AudioClip sound1;

    /// <summary>
    /// サウンドファイル
    /// 遠距離弾
    /// </summary>
    public AudioClip sound2;

    /// <summary>
    /// AudioSourceのコンポーネント
    /// </summary>
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5000;

        audioSource = GetComponent<AudioSource>();

        //プレイヤーのオブジェクト取得
        refObj = GameObject.Find("Player");
        player = refObj.GetComponent<Player>();

        m_fire1_flg = false;

        m_fireInterval = 0;

        m_fireIntervalFps = SHOT_INTERVAL_FPS;
    }

    // Update is called once per frame
    void Update()
    {
        //通常時とクリア時は操作可能
        if ( GameState.STATE.PLAY == GameState.m_GameStateNow ||
             GameState.STATE.STAGECLEAR == GameState.m_GameStateNow)
        {
            m_fire1 = Input.GetAxisRaw("Fire1");
        }

        //インターミッション時には0を設定
        if( GameState.STATE.INTERMISSION == GameState.m_GameStateNow )
        {
            m_fire1 = 0;
        }
    }

    void FixedUpdate()
    {
        //死亡時は弾を打たない
        if (Player.m_PlayerDeadFlg == true)
        {
            return;
        }

        //if( PlayerCamera.CAMERATYPE_TPS == PlayerCamera.m_CameraType )
        if (Player.CAMERAMODE_TPS == Player.m_CameraMode)
        {
            PlayerShotType01();
        }
        else if (Player.CAMERAMODE_FPS == Player.m_CameraMode)
        {
            PlayerShotType02();
        }
        else
        {
            PlayerShotType01();
        }

    }

    /// <summary>
    /// TPS時の弾
    /// </summary>
    private void PlayerShotType01()
    {
        if (1.0f == m_fire1 && 0 == m_fireInterval % 4)
        {
            m_fire1_flg = true;

            // 弾丸の複製
            GameObject bullets = Instantiate(bullet) as GameObject;

            Vector3 force;

            force = (this.gameObject.transform.forward + new Vector3(0.0f, 0.0f, 0.0f)) * speed;

            // Rigidbodyに力を加えて発射
            bullets.GetComponent<Rigidbody>().AddForce(force);

            // 弾丸の位置を調整(Playerの座標+指定y座標)
            bullets.transform.position = muzzle.position + new Vector3(0.0f, 0.0f, 0.0f);

            //弾丸の向きを時機の向きに合わせる
            bullets.transform.rotation = Quaternion.LookRotation(transform.forward);

            //サウンドの再生
            //SoundPlay.PlaySE(sound1, gameObject.transform.position);
            SoundPlay.PlaySE(sound1);
        }

        else if (0.0f == m_fire1)
        {
            m_fire1_flg = false;
            m_fireInterval = 0;
        }

        m_fireInterval++;
    }

    /// <summary>
    /// FPS時の弾
    /// </summary>
    private void PlayerShotType02()
    {
        if (1.0f == m_fire1 && false == m_fire1_flg && 60 <= m_fireIntervalFps)
        {
            m_fire1_flg = true;

            // 弾丸の複製
            GameObject bullets = Instantiate(bullet2) as GameObject;

            // 弾丸の位置を調整(Playerの座標+指定y座標)
            bullets.transform.position = Aim.m_AimPos;

            //サウンドの再生
            //SoundPlay.PlaySE(sound2, gameObject.transform.position);
            SoundPlay.PlaySE(sound2);

            m_fireIntervalFps = 0;
        }

        else if (0.0f == m_fire1)
        {
            m_fire1_flg = false;
        }

        if (m_fireIntervalFps < SHOT_INTERVAL_FPS)
        {
            m_fireIntervalFps++;
        }
    }
}

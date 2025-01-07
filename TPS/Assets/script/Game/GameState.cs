using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームの状態
/// アタッチ:GameState
/// </summary>
public class GameState : MonoBehaviour
{
    public enum STATE
    {
        PRE_INTERMISSION,
        INTERMISSION,
        PLAY,
        GAMEOVER,
        STAGECLEAR,
    }

    public enum INTTERMISSION_STATE
    {
        STATE1,
        STATE2,
    }

    public enum STAGE
    {
        STAGE_01,
        STAGE_02,
        STAGE_03,
        STAGE_04,
        STAGE_05,
        MAX_STAGE,
    }

    private const int GAMEOVER_TIME = 300;
   
    private const int INTERMISSION_TIME = 400 ;
    private const int INTERMISSION_SWITCH_TIME = 360;

    private const float STAGE_DEF_POS_X = 0.0f;
    private const float STAGE_DEF_POS_Y = 14.09847f;
    private const float STAGE_DEF_POS_Z = -4.699767f;


    /// <summary>
    /// 状態のカウンター
    /// </summary>
    private int m_StateCnt = 0;

    /// <summary>
    /// ゲームの状態
    /// </summary>
    public static STATE m_GameStateNow { get; private set; } = STATE.PLAY;

    /// <summary>
    /// 前フレームゲームの状態
    /// </summary>
    public static STATE m_GameStateOld { get; private set; } = STATE.PLAY;

    /// <summary>
    /// インターミッション時の状態
    /// </summary>
    public static INTTERMISSION_STATE m_InterMissionState { get; private set; } = INTTERMISSION_STATE.STATE1;

    /// <summary>
    /// ゲーム状態変化フラグ
    /// </summary>
    private bool m_GameStateTransFlg = false;

    /// <summary>
    /// ステージ数
    /// </summary>
    public static STAGE m_GameStateStage { get; private set; } = STAGE.STAGE_01;

    /// <summary>
    /// Playerのオブジェクト
    /// </summary>
    private GameObject PlayerObj;

    /// <summary>
    /// Playerのコンポーネント
    /// </summary>
    private Player ComPlayer;

    /// <summary>
    /// PlayerCameraPosのコンポーネント
    /// </summary>
    private PlayerCameraPos ComPlayerCameraPos;

    /// <summary>
    /// Playerのカメラセット位置
    /// </summary>
    private GameObject PlayerCameraPosObj;

    /// <summary>
    /// PlayerCameraのコンポーネント
    /// </summary>
    private PlayerCamera ComPlayerCamera;

    /// <summary>
    /// Playerのカメラセット位置
    /// </summary>
    private GameObject PlayerCameraObj;

    /// <summary>
    /// ステージのオブジェクト
    /// </summary>
    private GameObject[] StageObj = new GameObject[(int)STAGE.MAX_STAGE];

    /// <summary>
    /// 実行しているステージ(StageObjをコピーして使用する)
    /// </summary>
    private GameObject StageExeObj;


    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーのカメラの位置
        PlayerObj = GameObject.Find("Player");
        Assertion.Assert(PlayerObj);

        ComPlayer = PlayerObj.GetComponent<Player>();
        Assertion.Assert(ComPlayer);

        // プレイヤーのカメラの位置
        PlayerCameraPosObj = GameObject.Find("PlayerCameraPos");
        Assertion.Assert(PlayerCameraPosObj);

        ComPlayerCameraPos = PlayerCameraPosObj.GetComponent<PlayerCameraPos>();
        Assertion.Assert(ComPlayerCameraPos);

        // プレイヤーのカメラ
        PlayerCameraObj = GameObject.Find("PlayerCamera");
        Assertion.Assert(PlayerCameraObj);

        ComPlayerCamera = PlayerCameraObj.GetComponent<PlayerCamera>();
        Assertion.Assert(ComPlayerCamera);

        //マウスカーソルの非表示
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StageObj[(int)STAGE.STAGE_01] = GameObject.Find("Stage01");
        Assertion.Assert(StageObj[(int)STAGE.STAGE_01]);

        StageObj[(int)STAGE.STAGE_02] = GameObject.Find("Stage02");
        Assertion.Assert(StageObj[(int)STAGE.STAGE_02]);

        StageObj[(int)STAGE.STAGE_03] = GameObject.Find("Stage03");
        Assertion.Assert(StageObj[(int)STAGE.STAGE_03]);

        StageObj[(int)STAGE.STAGE_04] = GameObject.Find("Stage04");
        Assertion.Assert(StageObj[(int)STAGE.STAGE_04]);

        StageObj[(int)STAGE.STAGE_05] = GameObject.Find("Stage05");
        Assertion.Assert(StageObj[(int)STAGE.STAGE_05]);


        // 一度ステージは非アクティブにする
        for (int i = 0; i < (int)STAGE.MAX_STAGE; i++)
        {
            StageObj[i].SetActive(false);
        }

        // ステージのセット
        m_GameStateStage = STAGE.STAGE_05;
        StageSet(m_GameStateStage);

        // 初期化
        InitState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //状態切り替え時
        if (m_GameStateOld != m_GameStateNow)
        {
            switch ( m_GameStateNow )
            {
                case STATE.PRE_INTERMISSION:
                    StatePreInterMission();
                    break;
                case STATE.INTERMISSION:
                    StateInterMission();
                    break;
                case STATE.PLAY:
                    StatePlay();
                    break;
                case STATE.GAMEOVER:
                    StateGameOver();
                    break;
                case STATE.STAGECLEAR:
                    StateStageClear();
                    break;
            }
        }


        switch( m_GameStateNow )
        {
            case STATE.PRE_INTERMISSION:
                StatePreInterMission();
                break;
            case STATE.INTERMISSION:
                StateInterMission();
                break;
            case STATE.PLAY:
                StatePlay();
                break;
            case STATE.GAMEOVER:
                StateGameOver();
                break;
            case STATE.STAGECLEAR:
                StateStageClear();
                break;
        }

        //前ゲームの状態をセット
        m_GameStateOld = m_GameStateNow;
    }

    /// <summary>
    /// 状態の初期化
    /// </summary>
    private void InitState()
    {
        //ゲームの状態
        m_GameStateNow = STATE.INTERMISSION;

        //インターミッション時の状態
        m_InterMissionState = INTTERMISSION_STATE.STATE1;

        //プレイヤーのカメラ位置セット
        ComPlayerCamera.InitInterMissionCameraSet();
    }

    /// <summary>
    /// ステージのセット
    /// </summary>
    private void StageSet( STAGE stage )
    {
        // ステージのオブジェクトをアクティブに
        StageObj[(int)stage].SetActive(true);

        //オブジェクトの破棄
        if (null != StageExeObj)
        {
            Object.Destroy(StageExeObj);
        }

        // ステージのオブジェクトをコピー
        StageExeObj = Instantiate(StageObj[(int)stage]) as GameObject;

        // ステージの座標セット
        StageExeObj.transform.position = new Vector3(STAGE_DEF_POS_X, STAGE_DEF_POS_Y, STAGE_DEF_POS_Z);

        // コピー元のステージのオブジェクトを非アクティブに
        StageObj[(int)stage].SetActive(false);
    }

    /// <summary>
    /// インターミッション準備の状態
    /// </summary>
    private void StatePreInterMission()
    {
        // ゲーム状態初期化()
        InitState();

        // ステージのセット
        StageSet(m_GameStateStage);

        // プレイヤーの状態をアクティブに
        PlayerObj.SetActive(true);

        // プレイヤーの状態初期化
        ComPlayer.InitPlayerState();
    }

    /// <summary>
    /// インターミッションの状態
    /// </summary>
    private void StateInterMission()
    {
        m_StateCnt++;

        if (INTERMISSION_TIME <= m_StateCnt)
        {
            m_GameStateNow = STATE.PLAY;

            // カメラモードをTPSに設定
            Player.PlayerCameraModeChange(Player.CAMERAMODE_TPS);

            // カメラの位置をセット
            ComPlayerCameraPos.SetTpsPos();

            // カメラの移動
            ComPlayerCamera.InitCameraMove();


            // 前ゲームの状態と違ったら実行
            if (m_GameStateNow != m_GameStateOld)
            {
                m_StateCnt = 0;
            }
        }
        else if(INTERMISSION_SWITCH_TIME <= m_StateCnt )
        {
            // カメラモードをTPSに設定
            Player.PlayerCameraModeChange( Player.CAMERAMODE_TPS );

            // カメラの位置をセット
            ComPlayerCameraPos.SetTpsPos();

            // カメラの移動
            ComPlayerCamera.InitCameraMove();

            // インターミッション時の状態切替
            m_InterMissionState = INTTERMISSION_STATE.STATE2;
        }
    }

    // ゲームの状態変化
    private void StatePlay()
    {
        //プレイヤー死亡時
        if (Player.m_PlayerDeadFlg == true)
        {
            m_GameStateNow = STATE.GAMEOVER;

            // 前ゲームの状態と違ったら実行
            if (m_GameStateNow != m_GameStateOld)
            {                
                m_StateCnt = 0;
            }
        }

        //拠点を全て破壊
        int count = GameObject.FindGameObjectsWithTag("EnemyBase").Length;

        if( 0 == count )
        {
            m_GameStateNow = STATE.STAGECLEAR;

            // 前ゲームの状態と違ったら実行
            if (m_GameStateNow != m_GameStateOld)
            {
                m_StateCnt = 0;
            }
        }
    }

    /// <summary>
    /// ゲームオーバー時の状態
    /// </summary>
    private void StateGameOver()
    {
        if( GAMEOVER_TIME <= m_StateCnt )
        {
            //タイトル画面に移動
            SceneManager.LoadScene("TitleScene");
        }

        m_StateCnt++;
    }

    /// <summary>
    /// ステージクリア時の状態
    /// </summary>
    private void StateStageClear()
    {
        if (GAMEOVER_TIME <= m_StateCnt)
        {
            //タイトル画面に移動
            //SceneManager.LoadScene("TitleScene");
            switch( m_GameStateStage )
            {
                case STAGE.STAGE_01:
                case STAGE.STAGE_02:
                case STAGE.STAGE_03:
                case STAGE.STAGE_04:
                    m_GameStateStage++;
                    m_GameStateNow = STATE.PRE_INTERMISSION;
                    break;
                case STAGE.STAGE_05:
                    SceneManager.LoadScene("TitleScene");
                    break;
                default:
                    SceneManager.LoadScene("TitleScene");
                    break;
            }

            m_StateCnt = 0;
        }

        m_StateCnt++;
    }

    /// <summary>
    /// インターミッション状態変更時
    /// </summary>
    private void ChangeStInterMission()
    {
        ComPlayerCamera.InitInterMissionCameraSet();
    }

    /// <summary>
    /// インターミッション状態変更時
    /// </summary>
    private void ChangeStPlay()
    {

    }

    /// <summary>
    /// インターミッション状態変更時
    /// </summary>
    private void ChangeStGameOver()
    {

    }

    /// <summary>
    /// インターミッション状態変更時
    /// </summary>
    private void ChangeStInterStageClear()
    {

    }
}

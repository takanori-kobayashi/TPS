using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy03
/// アタッチ:Enemy03
/// </summary>
public class Enemy03 : MonoBehaviour
{
    /// <summary>
    /// 弾(バリアー)
    /// </summary>
    public GameObject bullet;

    /// <summary>
    /// 目的地
    /// </summary>
    private Vector3 destination;

    /// <summary>
    /// 移動スピード
    /// </summary>
    [SerializeField]
    private float MoveSpeed = 100.0f;

    /// <summary>
    /// 最大移動スピード
    /// </summary>
    private const float MAX_SPEED = 15.0f;

    /// <summary>
    /// インターバル
    /// </summary>
    private const int MAX_INTERVAL = 60;

    /// <summary>
    /// 移動するタイミングの最大値
    /// </summary>
    private const int MAX_TIMING = (int)(MAX_SPEED * 4);

    /// <summary>
    /// 移動するタイミング
    /// </summary>
    private int timing = 0;

    /// <summary>
    /// バリアーを再度張るタイミング
    /// </summary>
    private int m_interval = MAX_INTERVAL;

    /// <summary>
    /// 子オブジェクトの最大数
    /// </summary>
    private const int CHILD_OBJCECT_MAX = 4;

    /// <summary>
    /// プレイヤーとの距離
    /// </summary>
    private float distance;

    /// <summary>
    /// Playerのゲームオブジェクト
    /// </summary>
    GameObject refObj;

    /// <summary>
    /// Playerのコンポーネント
    /// </summary>
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーのオブジェクト取得
        refObj = GameObject.Find("Player");
        Assertion.Assert(refObj);

        player = refObj.GetComponent<Player>();
        Assertion.Assert(player);


        MoveSpeed = 10.0f;
        timing = 20;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // インターミッション時は動作しない
        if (GameState.STATE.INTERMISSION == GameState.m_GameStateNow)
        {
            return;
        }

        //プレイヤーとの距離計算
        distance = (transform.position - player.transform.position).magnitude;
        if (20.0f < distance)
        {
            GetComponent<Rigidbody>().velocity = transform.forward.normalized * 0;
            return;
        }

        if (timing < 0)
        {
            //プレイヤーの座標取得(ジャンプはしない)
            destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

            transform.LookAt(player.transform); //プレイヤーの向きに設定

            timing = MAX_TIMING;
            MoveSpeed = MAX_SPEED;
        }
        else
        {

            GetComponent<Rigidbody>().velocity = transform.forward.normalized * MoveSpeed; //速度ベクトル設定

            if (0 < MoveSpeed)
            {
                MoveSpeed -= 1.0f;
            }
            timing--;
        }



        // 子オブジェクトが初期の数かつインターバルが最大なら生成
        if ( transform.childCount == CHILD_OBJCECT_MAX && MAX_INTERVAL <= m_interval )
        {
            // 弾丸の複製
            GameObject bullets = Instantiate(bullet) as GameObject;
            // 弾丸の位置を調整
            bullets.transform.position = transform.position;
            // 子オブジェクトに設定
            bullets.transform.parent = this.transform;
            // インターバル0
            m_interval = 0;
        }
        // 子オブジェクトが初期の数かつインターバルが最大未満ならインターバル加算
        else if ( transform.childCount == CHILD_OBJCECT_MAX && m_interval < MAX_INTERVAL)
        {
            if (m_interval < MAX_INTERVAL)
            {
                m_interval++;
            }
        }

    }
}

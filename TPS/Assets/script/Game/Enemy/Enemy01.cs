using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy01の動作
/// </summary>
public class Enemy01 : MonoBehaviour
{
    /// <summary>
    /// CharacterControllerのコンポーネント
    /// </summary>
    private CharacterController enemyController;

    /// <summary>
    /// 目的地点
    /// </summary>
    private Vector3 destination;

    /// <summary>
    /// 回転
    /// </summary>
    private Vector3 loatate;

    /// <summary>
    /// 歩くスピード
    /// </summary>
    [SerializeField]
    private float walkSpeed = 2.0f;

    /// <summary>
    /// bullet prefab 
    /// </summary>
    public GameObject bullet;

    /// <summary>
    /// 速度
    /// </summary>
    private Vector3 velocity;

    /// <summary>
    /// 移動方向
    /// </summary>
    private Vector3 direction;

    /// <summary>
    /// プレイヤーとの距離
    /// </summary>
    private float distance;

    /// <summary>
    /// インターバル
    /// </summary>
    private int m_interval;

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
        player = refObj.GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // インターミッション時は動作しない
        if( GameState.STATE.INTERMISSION == GameState.m_GameStateNow )
        {
            return;
        }

        //プレイヤーとの距離計算
        distance = (transform.position - player.transform.position).magnitude;
        if (40.0f < distance) return;

        destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        loatate = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        direction = (destination - transform.position).normalized;
        velocity = Vector3.zero;
        velocity = direction * walkSpeed;


        GetComponent<Rigidbody>().MovePosition(transform.position + velocity * Time.deltaTime);

        transform.LookAt(loatate);


        //攻撃
        if(distance < 5.0f )
        {
            m_interval++;

            if (60 < m_interval)
            {
                // 弾丸の複製
                GameObject bullets = Instantiate(bullet) as GameObject;
                // 弾丸の位置を調整
                bullets.transform.position = this.transform.position;

                Object.Destroy(bullets, 0.2f);

                m_interval = 0;
            }
        }
        else
        {
            m_interval = 0;
        }

    }
}

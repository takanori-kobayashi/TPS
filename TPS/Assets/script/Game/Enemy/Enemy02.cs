using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy02の動作
/// </summary>
public class Enemy02 : MonoBehaviour
{
    /// <summary>
    /// 弾の発射位置
    /// </summary>
    public Transform muzzle;

    /// <summary>
    /// Rigidbodyのコンポーネント
    /// </summary>
    Rigidbody rb;

    /// <summary>
    /// ジャンプ力
    /// </summary>
    private readonly float JumpPower = 10.0f;

    /// <summary>
    /// ジャンプ判定フラグ
    /// </summary>
    private bool m_jumpFlg;

   /// <summary>
   /// 回転
   /// </summary>
    private Vector3 loatate;

    /// <summary>
    /// Playerのゲームオブジェクト
    /// </summary>
    GameObject refObj;

    /// <summary>
    /// Playerのコンポーネント
    /// </summary>
    Player player;

    /// <summary>
    /// bulletのゲームオブジェクト 
    /// </summary>
    public GameObject bullet;

    /// <summary>
    /// 弾の発射のインターバル
    /// </summary>
    private const int MAX_INTERVAL = 100;

    /// <summary>
    /// プレイヤーとの距離
    /// </summary>
    private float distance;

    /// <summary>
    /// 前回のY座標
    /// </summary>
    private float old_y;

    /// <summary>
    /// 弾の発射フラグ
    /// </summary>
    private bool shot_flg;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーのオブジェクト取得
        refObj = GameObject.Find("Player");
        player = refObj.GetComponent<Player>();

        m_jumpFlg = false;

        rb = GetComponent<Rigidbody>();

        old_y = this.transform.position.y;

        shot_flg = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // インターミッション時は動作しない
        if (GameState.STATE.INTERMISSION == GameState.m_GameStateNow)
        {
            return;
        }

        //プレイヤーとの距離計算
        distance = (transform.position - player.transform.position).magnitude;
        if (35.0f < distance)
        {
            return;
        }

        loatate = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(loatate);

        //ジャンプ
        if (!m_jumpFlg)
        {
            m_jumpFlg = true;
            rb.velocity = transform.up * JumpPower;
        }



        //弾発射
        if (shot_flg == true)
        {
            if (this.transform.position.y < old_y)
            {
                // 弾丸の複製
                GameObject bullets = Instantiate(bullet) as GameObject;
                // 弾丸の位置を調整
                bullets.transform.position = muzzle.position;

                shot_flg = false;
            }
        }

        //y軸の位置を保持
        old_y = this.transform.position.y;
    }

    void OnCollisionEnter(Collision hit)
    {
        //接触したタグ
        if(hit.gameObject.tag == "Object")
        {
            m_jumpFlg = false;
            shot_flg = true;
        }
    }

}

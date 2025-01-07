using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy05の動作
/// </summary>
public class Enemy05 : MonoBehaviour
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
    /// 速度
    /// </summary>
    private Vector3 velocity;

    /// <summary>
    /// 移動方向
    /// </summary>
    private Vector3 direction;

    /// <summary>
    /// Playerのゲームオブジェクト
    /// </summary>
    GameObject refObj;

    /// <summary>
    /// Playerのコンポーネント
    /// </summary>
    Player player;

    /// <summary>
    /// bullet prefab 
    /// </summary>
    public GameObject bullet;

    /// <summary>
    /// ChargeEffect00 prefab
    /// </summary>
    public GameObject m_ChageEffectObj;


    /// <summary>
    /// インターバルの最大値
    /// </summary>
    private const int MAX_INTERVAL = 10;

    /// <summary>
    /// 弾発射のインターバル
    /// </summary>
    private int m_interval = MAX_INTERVAL;

    /// <summary>
    /// 照準のヒットフラグ
    /// </summary>
    private bool m_AimHitFlg;

    /// <summary>
    /// レイヤーのマスク
    /// </summary>
    private int m_layerMask;

    /// <summary>
    /// 弾丸発射点 
    /// </summary>
    public Transform muzzle;

    /// <summary>
    /// プレイヤーとの距離
    /// </summary>
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        destination = new Vector3(25f, 0f, 25f);
        velocity = Vector3.zero;

        //プレイヤーのオブジェクト取得
        refObj = GameObject.Find("Player");
        player = refObj.GetComponent<Player>();

        distance = 0.0f;

        //Rayに反応しないレイヤー
        var LAYER_MASK_TBL = new string[]
        {
            "Enemy",
            "Default",
        };
        int tmp;
        // レイヤーのマスク
        // Rayに反応しないレイヤーを設定(これを設定しないと標準がガクつく)
        m_layerMask = 0;
        for (int i = 0; i < LAYER_MASK_TBL.Length; i++)
        {
            tmp = LayerMask.NameToLayer(LAYER_MASK_TBL[i]); //レイヤー番号取得
            m_layerMask |= 1 << tmp; //レイヤー番号分ビットシフトし設定
        }
        //レイヤーのマスク
        m_layerMask = ~m_layerMask; // ビット判定
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        // インターミッション時は動作しない
        if (GameState.STATE.INTERMISSION == GameState.m_GameStateNow)
        {
            return;
        }

        //デバッグ表示
        Debug.DrawRay(transform.position, transform.forward * 40.0f, Color.green);

        //プレイヤーとの距離計算
        distance = (transform.position - player.transform.position).magnitude;        

        //プレイヤーの座標取得(ジャンプはしない)
        destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        Vector3 BarrelHi = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);

        if (m_AimHitFlg == false)
        {

            if (Physics.Raycast(ray, out hit, 40.0f, m_layerMask))
            {
                if (true == TagCheck.EnemyAimHitPlayerCheck(hit.collider))
                {
                    m_AimHitFlg = true;
                }
                else
                {
                    m_AimHitFlg = false;
                }
            }

            // ターゲットの方向ベクトルを取得
            Vector3 direction = player.transform.position - transform.position;
            // 方向ベクトルから回転クォータニオンを求める
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);


            // 現在の回転から目標の回転に向けて少しずつ回転する
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
        }
        else
        {
            // 距離が離れている場合は弾を打たない
            if (30.0f < distance)
            {
                return;
            }

            if (0 < m_interval)
            {
                m_interval--;
            }
            else
            {
                // 弾丸の複製
                GameObject bullets = Instantiate(bullet) as GameObject;


                Vector3 force;

                force = (this.gameObject.transform.forward + new Vector3(0.0f, 0.0f, 0.0f)) * 5000;

                // Rigidbodyに力を加えて発射
                bullets.GetComponent<Rigidbody>().AddForce(force);

                // 弾丸の位置を調整
                bullets.transform.position = muzzle.position;

                //弾丸の向きを時機の向きに合わせる
                bullets.transform.rotation = Quaternion.LookRotation(transform.forward);

                //インターバルセット
                m_interval = MAX_INTERVAL;

                // 照準のヒットフラグOFF
                m_AimHitFlg = false;
            }
        }
    }
}

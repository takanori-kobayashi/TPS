using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy04Axisの動作
/// アタッチ:Enemy04
/// </summary>
public class Enemy04Axis : MonoBehaviour
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
    /// EnemyBullet02Effect prefab
    /// </summary>
    public GameObject m_EnemyBullet02EffectObj;


    /// <summary>
    /// インターバルの最大値
    /// </summary>
    private const int MAX_INTERVAL = 100;

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
    public Transform muzzle2;

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

        //レイヤーのマスク
        m_layerMask = ~m_layerMask; // ビット判定
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float rot_x;
        //float rot_y;

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        //デバッグ表示
        Debug.DrawRay(transform.position, transform.forward * 150.0f, Color.green);

        // インターミッション時は動作しない
        if (GameState.STATE.INTERMISSION == GameState.m_GameStateNow)
        {
            return;
        }

        //プレイヤーとの距離計算
        distance = (transform.position - player.transform.position).magnitude;
        if (150.0f < distance)
        {
            return;
        }

        //プレイヤーの座標取得(ジャンプはしない)
        destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        Vector3 BarrelHi = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);

        if (m_AimHitFlg == false)
        {

            if (Physics.Raycast(ray, out hit, 140.0f, m_layerMask))
            {
                if (true == TagCheck.EnemyAimHitPlayerCheck(hit.collider))
                {
                    m_AimHitFlg = true;

                    // エフェクト
                    GameObject Effect = Instantiate(m_ChageEffectObj) as GameObject;
                    GameObject Effect2 = Instantiate(m_ChageEffectObj) as GameObject;
                    // エフェクトの位置を調整
                    Effect.transform.position = muzzle.position;
                    Effect2.transform.position = muzzle2.position;

                    // エフェクトを子オブジェクトに指定
                    Effect.transform.parent = this.transform;
                    Effect2.transform.parent = this.transform;

                    Object.Destroy(Effect, 2.0f);
                    Object.Destroy(Effect2, 2.0f);
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

            //角度を-180～180度の範囲に正規化する
            float angle_x = Mathf.Repeat(targetRotation.eulerAngles.x + 180, 360) - 180;
            float angle_y = Mathf.Repeat(targetRotation.eulerAngles.y + 180, 360) - 180;
            float angle_z = Mathf.Repeat(targetRotation.eulerAngles.z + 180, 360) - 180;

            rot_x = angle_x;

            // 下方向に向かないように再セット
            if (1.0f <= rot_x)
            {
                rot_x = 0.0f;
                targetRotation = Quaternion.Euler(rot_x, angle_y, angle_z); // Z軸を10°に設定
            }


            // 現在の回転から目標の回転に向けて少しずつ回転する
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
        }
        else
        {

            if (0 < m_interval)
            {
                m_interval--;
            }
            else
            {
                // 弾丸の複製
                GameObject bullets = Instantiate(bullet) as GameObject;
                GameObject bullets2 = Instantiate(bullet) as GameObject;



                Vector3 force;

                force = (this.gameObject.transform.forward + new Vector3(0.0f, 0.0f, 0.0f)) * 5000;

                // Rigidbodyに力を加えて発射
                bullets.GetComponent<Rigidbody>().AddForce(force);
                bullets2.GetComponent<Rigidbody>().AddForce(force);

                // 弾丸の位置を調整
                bullets.transform.position = muzzle.position;
                bullets2.transform.position = muzzle2.position;

                //弾丸の向きを時機の向きに合わせる
                bullets.transform.rotation = Quaternion.LookRotation(transform.forward);
                bullets2.transform.rotation = Quaternion.LookRotation(transform.forward);

                //インターバルセット
                m_interval = MAX_INTERVAL;

                // 照準のヒットフラグOFF
                m_AimHitFlg = false;


                // 発射エフェクト
                GameObject Effect = Instantiate(m_EnemyBullet02EffectObj) as GameObject;

                // エフェクトの位置を調整
                Effect.transform.position = muzzle.position;

                // エフェクトを子オブジェクトに指定
                Effect.transform.parent = this.transform;

                Object.Destroy(Effect, 1.0f);

            }
        }
    }
}

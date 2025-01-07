using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の弾1(狙い弾)
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    /// <summary>
    /// CharacterControllerのコンポーネント
    /// </summary>
    private CharacterController enemyController;

    /// <summary>
    /// スピード
    /// </summary>
    [SerializeField]
    private float Speed = 10.0f;

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
    /// MeshRendererのコンポーネント
    /// </summary>
    MeshRenderer meshrender;

    /// <summary>
    /// ヒット判定
    /// </summary>
    private bool m_hit;

    /// <summary>
    /// 存在時間
    /// </summary>
    private const float LIVING_TIME = 10.0f;

    void Start()
    {
        // プレイヤーのオブジェクト取得
        refObj = GameObject.Find("Player");
        player = refObj.GetComponent<Player>();

        //プレイヤーの向きに設定
        transform.LookAt(player.transform);

        //速度ベクトル設定
        GetComponent<Rigidbody>().velocity = transform.forward.normalized * Speed;

        meshrender = GetComponent<MeshRenderer>();

        Object.Destroy(this.gameObject, LIVING_TIME);

        //ヒット判定
        m_hit = false; 


    }

    /// <summary>
    /// トリガーの場合
    /// </summary>
    /// <param name="hit"></param>
    void OnTriggerStay(Collider hit)
    {
        // 接触対象のタグ
        if ( true == TagCheck.EnemyBulletHitCheck(hit) )
        {
            m_hit = true;
        }
    }


    void FixedUpdate()
    {
        float level = Mathf.Abs(Mathf.Sin(Time.time * 5));

        meshrender.material.color = new Color(level, 0.0f, level+1.0f, 1.0f);

        if (m_hit == true)
        {
            //敵キャラの削除
            Object.Destroy(this.gameObject);
        }
    }

}
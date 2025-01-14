using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の弾2(直線)
/// アタッチ:EnemyBullet02
/// </summary>
public class EnemyBullet02 : MonoBehaviour
{
    /// <summary>
    /// ヒット判定
    /// </summary>
    private bool m_hit;

    /// <summary>
    /// 存在時間
    /// </summary>
    private const float LIVING_TIME = 3.0f;

    /// <summary>
    /// 弾の位置
    /// </summary>
    private Vector3 bullet_pos;

    // Start is called before the first frame update
    void Start()
    {
        Object.Destroy(this.gameObject, LIVING_TIME);

        //bullet_pos = GetComponent<Transform>().position;

        //transform.Rotate(90.0f, 0.0f, 0.0f);

    }


    /// <summary>
    /// トリガーの場合
    /// </summary>
    /// <param name="hit"></param>
    void OnTriggerStay(Collider hit)
    {
        // 接触対象のタグ
        if ( hit.gameObject.tag == "Object" || 
             hit.gameObject.tag == "Obstacle")
        {
            m_hit = true;
        }
    }


    void FixedUpdate()
    {

        if (m_hit == true)
        {
            //敵キャラの削除
            Object.Destroy(this.gameObject);
        }
    }
}

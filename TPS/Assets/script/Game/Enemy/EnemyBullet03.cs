using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の弾3(バリアー系)
/// </summary>
public class EnemyBullet03 : MonoBehaviour
{
    /// <summary>
    /// CharacterControllerのコンポーネント
    /// </summary>
    private CharacterController enemyController;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// トリガーの場合
    /// </summary>
    /// <param name="hit"></param>
    void OnTriggerStay(Collider hit)
    {
        // 接触対象のタグ
        if (hit.gameObject.tag == "Player")
        {
            m_hit = true;
        }
    }

    private void FixedUpdate()
    {
        if (m_hit == true)
        {
            //敵キャラの削除
            Object.Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タグチェッククラス
/// </summary>
public class TagCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// 敵の照準のチェック
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    public static bool EnemyAimHitPlayerCheck(Collider hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 敵の被弾チェック
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    public static bool EnemyHitBulletCheck( Collider hit )
    {
        if( hit.gameObject.tag == "PlayerBullet" ||
            hit.gameObject.tag == "PlayerBullet02" )
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// プレイヤーの被弾チェック
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    public static bool PlayerHitDamageCheck( Collider hit )
    {
        if ( hit.gameObject.tag == "EnemyBullet" ||
             hit.gameObject.tag == "EnemyDamage")
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// プレイヤーの弾のヒットチェック
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    public static bool PlayerBulletHitCheck( Collider hit )
    {
        if (hit.gameObject.tag == "Enemy" ||
            hit.gameObject.tag == "Object" ||
            hit.gameObject.tag == "EnemyBase" )
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 標準のヒットチェック
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    public static bool AimHitCheck( Collider hit )
    {
        if (hit.gameObject.tag == "Enemy" ||
            hit.gameObject.tag == "Object" ||
            hit.gameObject.tag == "EnemyBase")
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// EnemyBulletのヒットチェック
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    public static bool EnemyBulletHitCheck( Collider hit )
    {
        if ( hit.gameObject.tag == "Player" ||
             hit.gameObject.tag == "Object" ||
             hit.gameObject.tag == "Obstacle")
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// ダメージが入るかチェック
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    public static bool EnemyDamageCheck( Collider hit )
    {
        if ( hit.gameObject.tag == "Enemy" ||
             hit.gameObject.tag == "EnemyBase")
        {
            return true;
        }

        return false;
    }

}

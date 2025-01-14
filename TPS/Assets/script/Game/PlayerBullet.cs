using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの弾の処理
/// アタッチ:PlayerBullet
/// </summary>
public class PlayerBullet : MonoBehaviour
{
    /// <summary>
    /// 爆発エフェクトのゲームオブジェクト
    /// </summary>
    public GameObject Effect_Hit;

    private ParticleSystem.MainModule m_particul;

    // Start is called before the first frame update
    void Start()
    {
        Object.Destroy(this.gameObject, 0.35f);
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ヒットエフェクトのセット
    /// </summary>
    private void HitEffectSet(Collider hit)
    {
        if (Effect_Hit != null)
        {
            // 爆発エフェクトの複製
            GameObject Oexp = Instantiate(Effect_Hit) as GameObject;

            // 爆発エフェクトの位置を調整
            //if (muzzle != null)
            {
                Oexp.transform.position = this.gameObject.transform.position;
                Oexp.transform.rotation = this.gameObject.transform.rotation;
                Oexp.transform.Rotate(180.0f, 0.0f, 0.0f);

                m_particul = Effect_Hit.GetComponent<ParticleSystem>().main;

                if ( true == TagCheck.EnemyDamageCheck(hit) )
                {
                    m_particul.startColor = new Color(1, 0, 0, 0.7f);
                }
                else
                {
                    m_particul.startColor = new Color(0, 0, 1, 0.7f);
                }
            }

            //エフェクトの削除
            Object.Destroy(Oexp, 0.5f);
        }
    }

    // トリガーとの接触時に呼ばれるコールバック
    //void OnCollisionEnter(Collision hit)
    void OnTriggerEnter(Collider hit)
    {

        // 接触タグチェック
        if (true == TagCheck.PlayerBulletHitCheck(hit))
        {
            //エフェクトセット
            HitEffectSet( hit );

            Object.Destroy(this.gameObject);
        }
       
    }
}

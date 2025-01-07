using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの弾が当たった時の処理
/// アタッチ:オブジェクト全般
/// </summary>
public class BulletHit : MonoBehaviour
{
    /// <summary>
    /// エフェクトの出現位置
    /// </summary>
    public Transform muzzle;

    /// <summary>
    /// 爆発エフェクトのゲームオブジェクト
    /// </summary>
    public GameObject exp;

    /// <summary>
    /// GameDataのゲームオブジェクト
    /// </summary>
    GameObject GameObj;

    /// <summary>
    /// GameDataのコンポーネント
    /// </summary>
    //GameData gamedata;

    /// <summary>
    /// GameStateのゲームオブジェクト
    /// </summary>
    GameObject GameStateObj;

    /// <summary>
    /// GameStateのコンポーネント
    /// </summary>
    GameState gamestate;

    /// <summary>
    /// Playerのゲームオブジェクト
    /// </summary>
    GameObject GamePlayerObj;

    /// <summary>
    /// Playerのゲームコンポーネント
    /// </summary>
    Player gameplayer;

    /// <summary>
    /// 敵のライフ
    /// </summary>
    public int life = 1;

    /// <summary>
    /// 敵撃破時のスコア
    /// </summary>
    public int AddScore = 0;

    /// <summary>
    /// 敵撃破時のスコア加算フラグ
    /// </summary>
    private bool AddScoreFlg;

    /// <summary>
    /// 撃破時のSE
    /// </summary>
    public AudioClip SoundExplosion;

    /// <summary>
    /// 弾ヒット時のSE
    /// </summary>
    public AudioClip SoundHit;

    /// <summary>
    /// SudioSourceのコンポーネント
    /// </summary>
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // トリガーとの接触時に呼ばれるコールバック
    //void OnCollisionEnter(Collision hit)
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はbulletタグですか？
        if ( true == TagCheck.EnemyHitBulletCheck(hit) )
        {
            // ライフ減算
            LifeSub( hit );

            //倒した場合
            if (life <= 0)
            {


                //---------------------
                //爆発エフェクト
                //---------------------
                if (exp != null)
                {
                    // 爆発エフェクトの複製
                    GameObject Oexp = Instantiate(exp) as GameObject;

                    // 爆発エフェクトの位置を調整
                    if (muzzle != null)
                    {
                        Oexp.transform.position = muzzle.position;
                    }

                    //二秒後に削除
                    Object.Destroy(Oexp, 1.0f);
                }

                //敵キャラの削除
                Object.Destroy(this.gameObject);

                
                if (AddScoreFlg == false)
                {
                    //スコア加算
                    //gamedata.AddScore(AddScore);

                    //加算フラグON
                    AddScoreFlg = true;
                }

                //撃破時のSE再生
                if (SoundExplosion != null)
                {
                    AudioSource.PlayClipAtPoint(SoundExplosion, gameObject.transform.position);
                }


            }
            else
            {
                //弾ヒット時のSE再生
                if (SoundHit != null)
                {
                    AudioSource.PlayClipAtPoint(SoundHit, gameObject.transform.position);
                }
            }

        }
    }

    /// <summary>
    /// ライフ減算
    /// </summary>
    private void LifeSub( Collider hit )
    {
        if (hit.gameObject.tag == "PlayerBullet")
        {
            life--;
        }
        else if (hit.gameObject.tag == "PlayerBullet02")
        {
            life -= 30;
        }
    }

}

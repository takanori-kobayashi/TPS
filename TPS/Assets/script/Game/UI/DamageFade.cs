using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ダメージを受けた時の画面の点滅処理
/// アタッチ:DamageFade
/// </summary>
public class DamageFade : MonoBehaviour
{
    private Image image;

    private bool m_FadeFlg;

    private float m_AlphaCnt;

    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<Image>();
        m_FadeFlg = false;

        m_AlphaCnt = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {        
    }

    private void FixedUpdate()
    {
        if (false == m_FadeFlg)
        {
            if (true == Player.m_DamageFlgOld)
            {
                m_FadeFlg = true;
                m_AlphaCnt = 0.3f;
            }
        }
        else
        {
            if (true == Player.m_DamageFlgOld)
            {
                m_AlphaCnt = 0.3f;
            }

            image.color = new Color(1.0f, 0, 0, m_AlphaCnt);
            if (0 < m_AlphaCnt)
            {
                m_AlphaCnt -= 0.01f;
            }
            else
            {
                m_FadeFlg = false;
            }

        }
    }
}

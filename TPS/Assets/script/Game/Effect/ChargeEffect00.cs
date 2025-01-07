using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ChargeEffect00の動作
/// アタッチ:ChargeEffect00
/// </summary>
public class ChargeEffect00 : MonoBehaviour
{

    private ParticleSystem m_particul;
    private ParticleSystem.MainModule m_particulMain;

    private float m_r;

    // Start is called before the first frame update
    void Start()
    {
        m_particulMain = GetComponent<ParticleSystem>().main;
        m_particul = GetComponent<ParticleSystem>();

        m_r = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //色変更
        m_particulMain.startColor = new Color(1.0f, 1.0f - m_r, 1.0f - m_r, 0.7f);

        m_r += 0.01f;

        if(1.0f <= m_r )
        {
            m_r = 1.0f;
        }
    }
}

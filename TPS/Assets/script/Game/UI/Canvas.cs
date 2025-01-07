using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UIの表示など
/// アタッチ:Canvas
/// </summary>
public class Canvas : MonoBehaviour
{
    /// <summary>
    /// 子オブジェクト(Image)
    /// </summary>
    GameObject m_childImage;

    /// <summary>
    /// 子オブジェクト(Aim)
    /// </summary>
    GameObject m_childAim;

    /// <summary>
    /// 子オブジェクト(DamageFade)
    /// </summary>
    GameObject m_childDamageFade;

    /// <summary>
    /// 子オブジェクト(Gauge)
    /// </summary>
    GameObject m_childLifeGauge;

    /// <summary>
    /// 子オブジェクト(EnergieGauge)
    /// </summary>
    GameObject m_childEnergieGauge;

    // Start is called before the first frame update
    void Start()
    {
        //子オブジェクト取得
        try
        {
            m_childImage = transform.Find("Image").gameObject;
            Assertion.Assert(m_childImage);

            m_childAim = transform.Find("Aim").gameObject;
            Assertion.Assert(m_childAim);

            m_childDamageFade = transform.Find("DamageFade").gameObject;
            Assertion.Assert(m_childDamageFade);

            m_childLifeGauge = transform.Find("LifeGauge").gameObject;
            Assertion.Assert(m_childLifeGauge);

            m_childEnergieGauge = transform.Find("EnergieGauge").gameObject;
            Assertion.Assert(m_childEnergieGauge);
        }catch
        {
            Assertion.Assert();
        }

        m_childImage.SetActive(false);
        m_childAim.SetActive(false);
        m_childDamageFade.SetActive(false);
        m_childLifeGauge.SetActive(false);
        m_childEnergieGauge.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        switch(GameState.m_GameStateNow)
        {
            case GameState.STATE.INTERMISSION:
                InterMission();
                break;
            case GameState.STATE.PLAY:
                NmlState();
                break;
            case GameState.STATE.STAGECLEAR:
                NmlState();
                break;
            default:
                NmlState();
                break;
        }
    }

    /// <summary>
    /// インターミッション時
    /// </summary>
    private void InterMission()
    {
        m_childImage.SetActive(false);
        m_childAim.SetActive(false);
        m_childDamageFade.SetActive(false);
        m_childLifeGauge.SetActive(false);
        m_childEnergieGauge.SetActive(false);
    }

    /// <summary>
    /// 通常時
    /// </summary>
    private void NmlState()
    {
        m_childImage.SetActive(true);
        m_childAim.SetActive(true);
        m_childDamageFade.SetActive(true);
        m_childLifeGauge.SetActive(true);
        m_childEnergieGauge.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI使うときは忘れずに。
using UnityEngine.UI;

/// <summary>
/// ライフゲージ表示
/// アタッチ:LifeGauge
/// </summary>
public class LifeGauge : MonoBehaviour
{
    public Slider slider;
    
    private float m_lifeTmp;

    // Start is called before the first frame update
    void Start()
    {
        //Sliderを満タンにする。
        slider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        m_lifeTmp = Player.GetPlayerLife() / 100.0f;
        
        if (slider.value > m_lifeTmp)
        {
            slider.value -= 0.02f;

            if( slider.value <= m_lifeTmp )
            {
                slider.value = m_lifeTmp;
            }
        }

        
        else if (slider.value < m_lifeTmp)
        {
            slider.value += 0.02f;

            if (slider.value >= m_lifeTmp)
            {
                slider.value = m_lifeTmp;
            }
        }

    }
}

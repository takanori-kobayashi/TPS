using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトル画面の操作
/// アタッチ:TitleControl
/// </summary>
public class TitleControl : MonoBehaviour
{
    private float m_ButtonPUsh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_ButtonPUsh = Input.GetAxisRaw("Fire1");
    }

    private void FixedUpdate()
    {        
        if( m_ButtonPUsh != 0 )
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲーム状態の表示
/// アタッチ:GameStateText
/// </summary>
public class GameStateText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text; //TextMeshProの変数宣言

    // Start is called before the first frame update
    void Start()
    {
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //ゲームオーバー時
        if(GameState.STATE.GAMEOVER == GameState.m_GameStateNow )
        {
            text.enabled = true;
            text.text = "GAME OVER"; //TextMeshProでテキスト出力
        }

        //ステージクリア時
        else if (GameState.STATE.STAGECLEAR == GameState.m_GameStateNow)
        {
            text.enabled = true;
            text.text = "STAGE CLEAR"; //TextMeshProでテキスト出力
        }

        //インターミッション時
        else if (GameState.STATE.INTERMISSION == GameState.m_GameStateNow)
        {
            text.enabled = true;
            text.text = "STAGE"+ (int)(GameState.m_GameStateStage + 1); //TextMeshProでテキスト出力
        }

        else
        {
            text.enabled = false;
            text.text = ""; //TextMeshProでテキスト出力
        }
    }
}

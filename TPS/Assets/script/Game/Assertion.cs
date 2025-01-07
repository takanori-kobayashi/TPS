//デバッグ表示時コメントをはずす
#define ASSERT_DISP

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// アサーション表示
/// アタッチ:Assert
/// </summary>
public class Assertion : MonoBehaviour
{
    /// <summary>
    /// 文字のサイズ変更用
    /// </summary>
    private static GUIStyle m_guistyle;

    /// <summary>
    /// 文字の色変更用
    /// </summary>
    private static GUIStyleState m_styleState;

    /// <summary>
    /// 最大アサート数
    /// </summary>
    private const int ASSERT_MAX = 16;

    /// <summary>
    /// 最大数超えフラグ
    /// </summary>
    private static bool m_overFlg = false;

    /// <summary>
    /// 表示文字列のデータ
    /// </summary>
    struct TextData
    {
        public string str;
        public Rect rect;
        public Color color;

    }

    /// <summary>
    /// 文字列のリスト
    /// </summary>
    private static List<TextData> m_TextList = new List<TextData>();

    // Start is called before the first frame update
    void Start()
    {
        m_guistyle = new GUIStyle();
        m_styleState = new GUIStyleState();

        m_guistyle.fontSize = 15;
        m_styleState.textColor = Color.red;

        m_guistyle.normal = m_styleState;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

    }

    private void NumAssertCnt()
    {

    }

    /// <summary>
    /// アサート
    /// </summary>
    /// <param name="path"></param>
    /// <param name="line"></param>
    public static void Assert([CallerFilePath]string path = "", [CallerLineNumber]int line = 0)
    {
#if ASSERT_DISP
        Rect zahyou;

        // 最大数のアサートが発生したらリターン
        if (ASSERT_MAX == m_TextList.Count)
        {
            m_overFlg = true;
            return;
        }


        string strTmp = "ASSERT path:"+ path + " line:" + line;
        zahyou = new Rect(0, m_TextList.Count * 15, 1024, 512);


        var TextListTmp = new TextData();
        TextListTmp.str = strTmp;
        TextListTmp.rect = zahyou;
        TextListTmp.color = Color.red;

        m_TextList.Add(TextListTmp);

#endif
    }

    /// <summary>
    /// アサート(ゲームオブジェクトチェック用)
    /// </summary>
    /// <param name="check"></param>
    /// <param name="path"></param>
    /// <param name="line"></param>
    public static void Assert(GameObject check, [CallerFilePath]string path = "", [CallerLineNumber]int line = 0)
    {
#if ASSERT_DISP
        Rect zahyou;

        // 最大数のアサートが発生したらリターン
        if (ASSERT_MAX == m_TextList.Count)
        {
            m_overFlg = true;
            return;
        }

        // 空ではなかったらリターン
        if( null != check )
        {
            return;
        }


        string strTmp = "ASSERT path:" + path + " line:" + line;
        zahyou = new Rect(0, m_TextList.Count * 15, 1024, 512);


        var TextListTmp = new TextData();
        TextListTmp.str = strTmp;
        TextListTmp.rect = zahyou;
        TextListTmp.color = Color.red;

        m_TextList.Add(TextListTmp);

#endif
    }

    /// <summary>
    /// アサート(コンポーネント取得チェック用)
    /// </summary>
    /// <param name="check"></param>
    /// <param name="path"></param>
    /// <param name="line"></param>
    public static void Assert(MonoBehaviour check, [CallerFilePath]string path = "", [CallerLineNumber]int line = 0)
    {
#if ASSERT_DISP
        Rect zahyou;

        // 最大数のアサートが発生したらリターン
        if (ASSERT_MAX == m_TextList.Count)
        {
            m_overFlg = true;
            return;
        }

        // 空ではなかったらリターン
        if (null != check)
        {
            return;
        }


        string strTmp = "ASSERT path:" + path + " line:" + line;
        zahyou = new Rect(0, m_TextList.Count * 15, 1024, 512);


        var TextListTmp = new TextData();
        TextListTmp.str = strTmp;
        TextListTmp.rect = zahyou;
        TextListTmp.color = Color.red;

        m_TextList.Add(TextListTmp);

#endif
    }

    /// <summary>
    /// アサート(例外出力)
    /// </summary>
    /// <param name="check"></param>
    /// <param name="path"></param>
    /// <param name="line"></param>
    public static void Assert(Exception e, [CallerFilePath]string path = "", [CallerLineNumber]int line = 0)
    {
#if ASSERT_DISP
        Rect zahyou;

        // 最大数のアサートが発生したらリターン
        if (ASSERT_MAX == m_TextList.Count)
        {
            m_overFlg = true;
            return;
        }

        // 空ではなかったらリターン
        //if (null != check)
        //{
        //    return;
        //}


        //string strTmp = "ASSERT path:" + path + " line:" + line +" :" + e;
        string strTmp = e.ToString();
        zahyou = new Rect(0, m_TextList.Count * 15, 1024, 512);


        var TextListTmp = new TextData();
        TextListTmp.str = strTmp;
        TextListTmp.rect = zahyou;
        TextListTmp.color = Color.red;

        m_TextList.Add(TextListTmp);

#endif
    }


    /// <summary>
    /// アサート(文字列出力)
    /// </summary>
    /// <param name="pathflg">pathの出力の有無</param>
    /// <param name="str">文字列</param>
    /// <param name="path">path</param>
    /// <param name="line">エラー行</param>
    public static void Assert(bool pathflg, string str , [CallerFilePath]string path = "", [CallerLineNumber]int line = 0)
    {
#if ASSERT_DISP
        Rect zahyou;

        // 最大数のアサートが発生したらリターン
        if (ASSERT_MAX == m_TextList.Count)
        {
            m_overFlg = true;
            return;
        }

        // 空ではなかったらリターン
        //if (null != check)
        //{
        //    return;
        //}


        //string strTmp = "ASSERT path:" + path + " line:" + line +" :" + e;
        string strTmp = "";

        if( true == pathflg )
        {
            strTmp = "ASSERT path:" + path + " line:" + line + " :" + str;
        }
        else
        {
            strTmp = "ASSERT :" + str;
        }

        zahyou = new Rect(0, m_TextList.Count * 15, 1024, 512);


        var TextListTmp = new TextData();
        TextListTmp.str = strTmp;
        TextListTmp.rect = zahyou;
        TextListTmp.color = Color.red;

        m_TextList.Add(TextListTmp);

#endif
    }


    //GUI更新イベントが有ると勝手に呼ばれる
    private void OnGUI()
    {
#if ASSERT_DISP
        Rect zahyou;

        for (int i = 0; i < m_TextList.Count; i++)
        {
            GUI.color = m_TextList[i].color;
            GUI.Label(m_TextList[i].rect, m_TextList[i].str, m_guistyle);   //文字を書く
        }

        if ( true == m_overFlg )
        {
            zahyou = new Rect(0, m_TextList.Count * 15, 1024, 512);
            GUI.Label(zahyou, "ASSERT 16 Over!!", m_guistyle);   //最大表示
        }
#endif
    }
}

//デバッグ表示時コメントをはずす
//#define DEBUG_TEXT_DISP

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// デバッグ文字表示
/// </summary>
public class DebugText : MonoBehaviour
{
#if DEBUG_TEXT_DISP
    /// <summary>
    /// デバッグ表示の有無(true:表示)
    /// </summary>
    const bool m_DispOn = true;

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
    static List<TextData> m_TextList = new List<TextData>();
#endif

    // Start is called before the first frame update
    void Start()
    {
        AddText("test", 100, 100, 100, 100);
        AddText("test2", 100, 130, 100, 100);
        AddText("test3", 100, 130, 100, 100);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 文字列の追加
    /// </summary>
    /// <param name="str">表示文字列</param>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    /// <param name="w">横幅</param>
    /// <param name="h">高さ</param>
    /// <returns></returns>
    public static int AddText(string str, float x, float y, float w, float h)
    {
#if DEBUG_TEXT_DISP
        string strTmp = str;
        Rect zahyou = new Rect(x, y, w, h);

        var TextListTmp = new TextData();
        TextListTmp.str = str;
        TextListTmp.rect = zahyou;
        TextListTmp.color = Color.black;

        m_TextList.Add(TextListTmp);

        return m_TextList.Count - 1;
#else
        return 0;
#endif

    }

    public static void DispTextColor(Color c, int num)
    {
    }

    /// <summary>
    /// 表示文字列の削除
    /// </summary>
    /// <param name="num">表示文字列のインデックス</param>
    public static void DeleteTextNum(int num)
    {
#if DEBUG_TEXT_DISP
        if (num < m_TextList.Count)
        {
           m_TextList.RemoveAt(num);
        }
#endif
    }

    /// <summary>
    /// 表示文字列のクリア
    /// </summary>
    public static void AllClear()
    {
#if DEBUG_TEXT_DISP
        for (int i=0; i<m_TextList.Count; i++)
        {
            m_TextList.RemoveAt(i);
        }

        m_TextList.Clear();
#endif
    }

    /// <summary>
    /// 文字列のセット(色黒)
    /// </summary>
    /// <param name="str">表示文字列</param>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    /// <param name="w">横幅</param>
    /// <param name="h">高さ</param>
    /// <param name="num">表示文字列のインデックス</param>
    public static void SetText(string str, float x, float y, float w, float h, int num)
    {
#if DEBUG_TEXT_DISP
        string strTmp = str;
        Rect zahyou = new Rect(x, y, w, h);

        if (m_DispOn == true)
        {
            var TextListTmp = new TextData();
            TextListTmp.str = str;
            TextListTmp.rect = zahyou;
            TextListTmp.color = Color.black;

            if (num < m_TextList.Count)
            {
                m_TextList[num] = TextListTmp;
            }
        }
#endif
    }

    /// <summary>
    /// 文字列のセット(色設定)
    /// </summary>
    /// <param name="str">表示文字列</param>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    /// <param name="w">横幅</param>
    /// <param name="h">高さ</param>
    /// <param name="c">色</param>
    /// <param name="num">表示文字列のインデックス</param>
    public static void SetText(string str, float x, float y, float w, float h, Color c, int num)
    {
#if DEBUG_TEXT_DISP
        string strTmp = str;
        Rect zahyou = new Rect(x, y, w, h);

        if (m_DispOn == true)
        {
            var TextListTmp = new TextData();
            TextListTmp.str = str;
            TextListTmp.rect = zahyou;
            TextListTmp.color = c;

            if (num < m_TextList.Count)
            {
                m_TextList[num] = TextListTmp;
            }
        }
#endif
    }

    //GUI更新イベントが有ると勝手に呼ばれる
    void OnGUI()
    {
#if DEBUG_TEXT_DISP
        if (m_DispOn == true)
        {
            for (int i = 0; i < m_TextList.Count; i++)
            {
                GUI.color = m_TextList[i].color;
                GUI.Label(m_TextList[i].rect, m_TextList[i].str);   //文字を書く
            }
        }
#endif
    }
}

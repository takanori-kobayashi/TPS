
//SEが設定されているかチェック
//※OFFはコメント
#define SOUNDPLAY_SE_CHECK

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アタッチ:Sound
/// サウンドの再生
/// </summary>
public class SoundPlay : MonoBehaviour
{
    /// <summary>
    /// AudioSourceのコンポーネント取得用
    /// </summary>
    private static AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // ※インスペクターでAudio Sourceを追加する必要あり
        //   そのためには空でもオブジェクトにアタッチする必要あり
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="se"></param>
    public static void PlaySE(AudioClip se)
    {
        //サウンドの再生
        if (se != null)
        {
            try
            {
                audioSource.PlayOneShot(se, 1.0f);
            }
            catch (Exception e)
            {
                Assertion.Assert(true, se.ToString());
            }
        }
#if SOUNDPLAY_SE_CHECK
        else
        {
            Assertion.Assert(true, se.ToString());
        }
#endif
    }

    /// <summary>
    /// SE再生(位置設定)
    /// </summary>
    /// <param name="se"></param>
    /// <param name="pos"></param>
    public static void PlaySE( AudioClip se, Vector3 pos )
    {
        //サウンドの再生
        if (se != null)
        {
            try
            {
                AudioSource.PlayClipAtPoint(se, pos);
            }
            catch (Exception e)
            {
                Assertion.Assert(true, se.ToString());
            }
        }
#if SOUNDPLAY_SE_CHECK
        else
        {
            Assertion.Assert(true, se.ToString());
        }
#endif
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ミニマップのアイコン方向の固定
/// </summary>
public class MiniMapIconRotate : MonoBehaviour
{
    public GameObject m_refObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        m_refObj.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    }
}

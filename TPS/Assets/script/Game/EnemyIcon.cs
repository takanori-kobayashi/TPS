using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ミニマップの敵アイコン
/// </summary>
public class EnemyIcon : MonoBehaviour
{
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
        //オブジェクトの回転を固定(親オブジェクトに影響を受けないように)
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}

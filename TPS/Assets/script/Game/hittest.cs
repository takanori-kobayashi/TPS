using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hittest : MonoBehaviour
{
    public static Vector3 m_point;
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
        Collider[] colList = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.8f, 4.0f), transform.rotation);

        int x = 30;

        Vector3 hitpos;

        hitpos.x = 0;
        hitpos.y = 0;
        hitpos.z = 0;


        foreach (var c in colList)
        {
            if (c.gameObject.tag == "Player")
            {
                x = 10;
                //接触位置
                hitpos = c.ClosestPoint(gameObject.transform.position);
                m_point = hitpos;
                break;
            }
            else
            {
                hitpos.x = 0;
                hitpos.y = 0;
                hitpos.z = 0;
                m_point = hitpos;

                x = 20;
            }
        }

        string text = x.ToString() + " " + hitpos.ToString() + " " + transform.position.ToString(); 
        DebugText.SetText(text, 100, 160, 300, 100, 0);

    }
}

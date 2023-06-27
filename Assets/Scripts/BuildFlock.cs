using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildFlock : MonoBehaviour
{

    private readonly Vector3 foxesMeetPos = new Vector3(48, 0, 0);
    private readonly Vector3 stagsMeetPos = new Vector3(0, 0, 48);
    private bool flockBuilt = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!flockBuilt)
        {
            if (CompareTag("Fox"))
            {
                if (Vector3.Distance(transform.position, foxesMeetPos) > 7.0f)
                {
                    transform.LookAt(foxesMeetPos);
                }
                else
                {
                    flockBuilt = true;
                }
            }
            else if (CompareTag("Stag"))
            {
                if (Vector3.Distance(transform.position, stagsMeetPos) > 7.0f)
                {
                    transform.LookAt(stagsMeetPos);
                } else
                {
                    flockBuilt = true;
                }
            }
        } else
        {
            if (CompareTag("Fox"))
            {
                
            }
            else if (CompareTag("Stag"))
            {
            }
        }
    }
}

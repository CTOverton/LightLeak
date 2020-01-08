using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rise : MonoBehaviour
{
    private float destination;
    void Start()
    {
        destination = transform.position.y;
        transform.position -= new Vector3(0,10,0);
    }

    
    void Update()
    {
        transform.position += new Vector3(0,10f,0) * Time.deltaTime;

        if (transform.position.y > destination)
        {
            enabled = false;
        }
    }
}

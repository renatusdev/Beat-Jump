using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public static Vector3 pos;

    private void Start()
    {
        pos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            
            foreach(GameObject o in GameObject.FindGameObjectsWithTag("CurrentCheckpoint"))
            {
                if(o != gameObject)
                {
                    Destroy(o);
                }
                
            }
            gameObject.tag = "CurrentCheckpoint";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

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

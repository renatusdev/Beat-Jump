using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public Transform player;

    private void Start()
    {
        player.position = transform.position;
    }
}

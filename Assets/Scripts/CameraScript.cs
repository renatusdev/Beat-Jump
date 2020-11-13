using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float mouseSensitivity;
    public Transform player;
    public bool Paused = false;
    private float x = 0;
    private float y = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        x += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        y += Input.GetAxis("Mouse X") * mouseSensitivity;

        x = Mathf.Clamp(x, -90, 90);

        transform.localRotation = Quaternion.Euler(x, y, 0);
        player.localRotation = Quaternion.Euler(0, y, 0);
        transform.position = player.transform.position;


        if (Input.GetKeyDown(KeyCode.Escape) && !Paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Paused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Paused = false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TextBoxStuff txtbs;
    private int count = 0;
    public static bool Paused = false;
    public static bool Dead = false;

    private void Update()
    {
        if (TextBoxStuff.waited)
        {
            if(count == 0)
            {
                MakeTextBoxText("Follow The Light.", 1);
                count++;
                TextBoxStuff.waited = false;
                txtbs.StartCoroutine(txtbs.MakeDelay(2.4f));
                return;
            }
            if(count == 1)
            {
                MakeTextBoxText("To Run Press LShift.", 1);
                count++;
                TextBoxStuff.waited = false;
                txtbs.StartCoroutine(txtbs.MakeDelay(4));
                return;
            }
            if (count == 2)
            {
                MakeTextBoxText("If you time it correctly... you can jump higher", 1);
                count++;
                TextBoxStuff.waited = false;
                txtbs.StartCoroutine(txtbs.MakeDelay(10));
                return;
            }
            if (count == 3)
            {
                MakeTextBoxText("Now lets see what you're made of.", 1);
                count++;
                TextBoxStuff.waited = false;
                txtbs.StartCoroutine(txtbs.MakeDelay(10));
                return;
            }
            if(count == 4)
            {
                MakeTextBoxText("", 1);
                count++;
                TextBoxStuff.waited = false;
                txtbs.StartCoroutine(txtbs.MakeDelay(6));
                return;
                
            }
            if(count == 5)
            {
                MakeTextBoxText("", 1);
                TextBoxStuff.waited = false;
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !Paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            Paused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Paused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            Paused = false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Adding To Eitans Code
        if(other.CompareTag("DeathCollider"))
        {
            Debug.Log("hello");
            Dead = true;
            GetComponent<PlayerController>().enabled = false;
            transform.position = GameObject.FindGameObjectWithTag("CurrentCheckpoint").transform.position;
            StartCoroutine(WaitTimer());

        }

        if (other.gameObject.tag == "FirstTutorialText")
        {
            MakeTextBoxText("Move Forward. Trust me.", 1);
            Destroy(other.gameObject);
            txtbs.StartCoroutine(txtbs.MakeDelay(5));
        }
        if(other.gameObject.tag == "SecondTutorialText")
        {
            TextBoxStuff.waited = false;
            count = 5;
            txtbs.StopAllCoroutines();
            MakeTextBoxText("", 1);
            MakeTextBoxText("You can wallrunu, but only jump out during beat.", 1);
            Destroy(other.gameObject);
            txtbs.StartCoroutine(txtbs.MakeDelay(8));
        }
    }

    private IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<PlayerController>().enabled = enabled;
    }

    public void MakeTextBoxText(string text, float delay)
    {
        txtbs.wantedTextObject.text = "";
        //txtbs.StartCoroutine(txtbs.MakeDelay(delay));
        txtbs.wantedText = text;
        txtbs.StartCoroutine(txtbs.MakeTextBeCool(0.1f));
    }
}

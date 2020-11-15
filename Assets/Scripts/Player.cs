using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TextBoxStuff txtbs;
    private int count = 0;
    public static bool Paused = false;

    private void Update()
    {
        if (TextBoxStuff.waited)
        {
            if(count == 0)
            {
                MakeTextBoxText("Follow The Light.", 1);
                count++;
                TextBoxStuff.waited = false;
                txtbs.StartCoroutine(txtbs.MakeDelay(2.2f));
                return;
            }
            if(count == 1)
            {
                MakeTextBoxText("To Run Press the LShift Button", 1);
                count++;
                TextBoxStuff.waited = false;
                txtbs.StartCoroutine(txtbs.MakeDelay(4));
                return;
            }
            if (count == 2)
            {
                MakeTextBoxText("If you time it correctly... you can hold space to go higher with your jump ", 1);
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
            Time.timeScale = 0;
            Paused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            Paused = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
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
            MakeTextBoxText("Did i mention you can wall run by jumping and holding LShift + W?", 1);
            Destroy(other.gameObject);
            txtbs.StartCoroutine(txtbs.MakeDelay(8));
        }
    }

    public void MakeTextBoxText(string text, float delay)
    {
        txtbs.wantedTextObject.text = "";
        //txtbs.StartCoroutine(txtbs.MakeDelay(delay));
        txtbs.wantedText = text;
        txtbs.StartCoroutine(txtbs.MakeTextBeCool(0.1f));
    }
}

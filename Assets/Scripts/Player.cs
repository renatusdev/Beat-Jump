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
                MakeTextBoxText("To move around Use the WASD Keys or The ARROW Keys", 1);
                count++;
                TextBoxStuff.waited = false;
                txtbs.StartCoroutine(txtbs.MakeDelay(6));
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
                MakeTextBoxText("To Jump press The Space Button \n Note: The Longer you hold down Space the higher you Jump", 1);
                count++;
                TextBoxStuff.waited = false;
                txtbs.StartCoroutine(txtbs.MakeDelay(10));
                return;
            }
            if (count == 3)
            {
                MakeTextBoxText("Please continue along to get to the first obstacle", 1);
                count++;
                TextBoxStuff.waited = false;
                txtbs.StartCoroutine(txtbs.MakeDelay(6));
                return;
            }
            if(count == 4)
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
            MakeTextBoxText("Hello and Welcome To... \"Beat-Jump\" :D", 1);
            Destroy(other.gameObject);
            txtbs.StartCoroutine(txtbs.MakeDelay(5));
        }
        if(other.gameObject.tag == "SecondTutorialText")
        {
            TextBoxStuff.waited = false;
            count = 4;
            txtbs.StopAllCoroutines();
            MakeTextBoxText("", 1);
            MakeTextBoxText("Did i mantion you can wall run? silly me... Press LShift + W to wall run", 1);
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

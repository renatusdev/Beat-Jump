using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBoxStuff : MonoBehaviour
{
    public string wantedText;
    public TMP_Text wantedTextObject;
    public bool isTextOnScreen = false;

    public static bool waited = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y) && !isTextOnScreen)
        {
            StartCoroutine(MakeTextBeCool(0.01f));
            isTextOnScreen = true;

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            wantedTextObject.text = "";
            isTextOnScreen = false;
        }
    }

    public IEnumerator MakeTextBeCool(float time)
    {
        for (int i = 0; i < wantedText.Length; i++)
        { 
            wantedTextObject.text += wantedText[i];
            yield return new WaitForSeconds(time);
        }
    }

    public IEnumerator MakeDelay(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Waited For " + time + " Seconds...");
        waited = true;
        
    }
}

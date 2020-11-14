using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 0.8659452
public class BeatGUI : MonoBehaviour
{
    float nonBeatScale = 1.09f;

    float timeElapse = 0;
    float duration = 0.5f;
    

    void Update()
    {
        Vector3 targetScale;

        if(BeatSignal.i.IsInLowBeat())
            targetScale = Vector3.one;
        else
            targetScale = Vector3.one * nonBeatScale;

        if(timeElapse < duration)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, timeElapse / duration);
            timeElapse += Time.deltaTime;
        }
        else
        {
            transform.localScale = targetScale;
            timeElapse = 0;
        }
    }
}

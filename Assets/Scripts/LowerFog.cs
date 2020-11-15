using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerFog : MonoBehaviour
{
    bool startLoweringFog;
    float timeElapsed;
    float transitionDuration;

    private void Start()
    {
        timeElapsed = 0;
        transitionDuration = 2f;
        startLoweringFog = false;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            startLoweringFog = true;
            Destroy(GetComponent<BoxCollider>());
        }
    }

    void Update()
    {
        if(startLoweringFog)
        {
            timeElapsed += Time.deltaTime;
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 0.01f, timeElapsed/transitionDuration);

            if(RenderSettings.fogDensity.Equals(0.01f))
            {
                startLoweringFog = false;
            }
        }
    }
}

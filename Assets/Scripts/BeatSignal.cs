using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSignal : MonoBehaviour
{
    public static BeatSignal i;

    // The lower this value, the longer the beat will remain.
    [Range(0,5)] public float timeProlongationAfterBeat; 

    [Range(0.0001f, 0.1f)] public float timeDecreaseOfBeatAfterItHits; 
    public bool visualize;
    public float mainAverage;

    // This value determines what a "low beat" is per track.
    public float minLowBeat;


    private AudioSource m_song;
    
    // Sample data of a wave spectrum within a given instance of time
    // The wave is Amplitude x Frequency 
    private float[] m_samples = new float[512];
    // Summarizes all samples to 8 numbers
    public float[] m_bands = new float[8];
    // Adds smoothness to the bands.
    private float[] bandBuffer = new float[8];
    
    private float[] bufferDecrease = new float[8]; 
    
    private void Awake()
    {
        if(i == null)
            i = this;
        else
            Destroy(this);    
    }    

	void Start ()
	{
        m_song = GetComponent<AudioSource>();
	}

    void Update()
    {
        m_song.GetSpectrumData(m_samples, 0, FFTWindow.Blackman);
        
        int count = 0;

        // For the eight bands
        for (int i=0; i < 8; i++)
        {
            float average = 0;

            int powerOf2Count = (int)Mathf.Pow(2,i) * 2;

            for(int j = 0; j < powerOf2Count; j++)
            {
                // average of all hertz within the sample
                average += (m_samples[count]) * (count + 1);
                count++;
            }
            average /= count;

            m_bands[i] = average * 100;
        }
        
        // For buffering the eight bands
        for (int i=0; i < 8; i++)
        {
            if(m_bands[i] > bandBuffer[i])
            {
                bandBuffer[i] = m_bands[i];
                bufferDecrease[i] = 0.005f;
            }
            else if(m_bands[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }

        mainAverage = 0;

        // Low beats
        for (int i=0; i < 3; i++)
        {
            mainAverage += bandBuffer[i];
        }
        mainAverage /= 3;

        if(visualize)
        {
            for (int i=0; i < 8; i++)
            {
                Debug.DrawLine(new Vector3(1*i, 0, 0), new Vector3(1*i, bandBuffer[i], 0), Color.blue);
            }
        }

    }

    private float timeTillBeatReset = 0.2f;
    private float timeSinceLastBeat = 1;

    public bool IsInLowBeat()
    {
        // If the time since the last beat is still not the time till the beat resets
        if(timeSinceLastBeat <= timeTillBeatReset)
        {
            // Increase the time by the time this frame has passed.
            timeSinceLastBeat += Time.deltaTime;

            // Fade out the low beat by still returning true
            return true;
        }
        // Else the time since the last beat has passed the time till the beat resets
        else
        {
            if(m_bands[0] > minLowBeat)
            {
                timeSinceLastBeat = 0;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void OnGUI()
    {
        if(visualize)
        {
            GUI.TextArea(new Rect(Screen.width/2,Screen.height/2,50,50), bandBuffer[0].ToString());
            if(IsInLowBeat())
                GUI.TextArea(new Rect(25,100,200,200), "Beat");
            else
                GUI.TextArea(new Rect(25,100,50,50), "No Beat");
        }
    }
}

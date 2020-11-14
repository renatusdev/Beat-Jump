using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSignal : MonoBehaviour
{
    public static BeatSignal i;

    // The lower this value, the longer the beat will remain.
    [Range(1,1.5f)] public float timeProlongationAfterBeat; 
    [Range(0.0001f, 0.1f)] public float timeDecreaseOfBeatAfterItHits; 
    public bool visualize;
    public float mainAverage;

    private AudioSource m_song;
    
    // Sample data of a wave spectrum within a given instance of time
    // The wave is Amplitude x Frequency 
    private float[] m_samples = new float[512];
    // Summarizes all samples to 8 numbers
    private float[] m_bands = new float[8];
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
                average += m_samples[count] * (count + 1);
                count++;
            }
            average /= count;

            m_bands[i] = average * 10;
        }
        
        // For buffering the eight bands
        for (int i=0; i < 8; i++)
        {
            if(m_bands[i] > bandBuffer[i])
            {
                bandBuffer[i] = m_bands[i];
                bufferDecrease[i] = timeDecreaseOfBeatAfterItHits;
            }
            else
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= timeProlongationAfterBeat;
            }
        }

        mainAverage = 0;

        // Low beats
        for (int i=0; i < 4; i++)
        {
            mainAverage += bandBuffer[i];
        }
        mainAverage /= 4;

        if(visualize)
        {
            for (int i=0; i < 8; i++)
            {
                Debug.DrawLine(new Vector3(1*i, 0, 0), new Vector3(1*i, bandBuffer[i] * 8, 0), Color.blue);
            }
        }
    }

    public bool IsInLowBeat()
    {
        return mainAverage > 2;
    }

    private void OnGUI()
    {
        if(visualize)
        {
            if(IsInLowBeat())
                GUI.TextArea(new Rect(25,100,200,200), "Beat");
            else
                GUI.TextArea(new Rect(25,100,50,50), "No Beat");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class fetchaudio : MonoBehaviour
{
    public AudioMixer mix;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setvol(Slider slide)
    {
        mix.SetFloat("master", slide.value * 40 - 40);
        if (slide.value == 0)
        {
            mix.SetFloat("master", -80);
        }
    }
}

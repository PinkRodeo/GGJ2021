using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource VRTrack;
    public AudioSource DownTrack;
    public AudioMixer mixer;
    
    Scene currentScene;
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene ();
        if (currentScene.name == "MenuVisuals")
        {
            StartHeadset();
            DontDestroyOnLoad(transform);
            VRTrack.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
       // currentScene = SceneManager.GetActiveScene ();
    }

    public void setVolume(Slider slide)
    {
        mixer.SetFloat("master", slide.value * 40 - 40);
        if (slide.value == 0)
        {
            mixer.SetFloat("master", -80);
        }
    }

    public void StartHeadset()
    {
        VRTrack.loop = false;
        
        mixer.SetFloat("headsettrack", 0);
        VRTrack.Play();
        StartCoroutine(StartDown());
    }

    IEnumerator StartDown()
    {
        yield return new WaitForSeconds(10);
        DownTrack.Play();
    }



}

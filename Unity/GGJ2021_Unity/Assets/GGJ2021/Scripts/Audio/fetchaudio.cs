using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fetchaudio : MonoBehaviour
{
    public GameObject pref;
    private AudioManager audioMan;
    void Awake()
    {
        if(!GameObject.Find("AudioEngine"))
        {
            Instantiate(pref);
        }else{
            audioMan = GameObject.Find("AudioEngine").GetComponent<AudioManager>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audioMan.StartHeadset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

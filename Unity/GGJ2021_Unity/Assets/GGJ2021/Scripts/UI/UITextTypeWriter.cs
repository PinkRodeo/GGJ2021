using System.Collections;
using UnityEngine;
using TMPro;

// attach to UI Text component (with the full text already there)

public class UITextTypeWriter : MonoBehaviour 
{
    public AudioClip typing;
    public AudioClip ding;
    
    private AudioSource Audio;
	TMP_Text txt;
	string story;
    public float delay = 0.0125f;

	void Awake () 
	{
        Audio = GetComponent<AudioSource>();
		txt = GetComponent<TMP_Text>();
		story = txt.text;
		txt.text = "";

		
		StartCoroutine ("PlayText");
	}

	IEnumerator PlayText()
	{
		foreach (char c in story) 
		{
            Audio.PlayOneShot(typing);
			txt.text += c;
			yield return new WaitForSeconds (delay);
		}
        Audio.PlayOneShot(ding);
	}
}
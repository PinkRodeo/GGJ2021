using System.Collections;
using UnityEngine;
using TMPro;

// attach to UI Text component (with the full text already there)

public class UITextTypeWriter : MonoBehaviour 
{
	TMP_Text txt;
	string story;
    public float delay = 0.0125f;

	void Awake () 
	{
		txt = GetComponent<TMP_Text>();
		story = txt.text;
		txt.text = "";

		
		StartCoroutine ("PlayText");
	}

	IEnumerator PlayText()
	{
		foreach (char c in story) 
		{
			txt.text += c;
			yield return new WaitForSeconds (delay);
		}
	}

}
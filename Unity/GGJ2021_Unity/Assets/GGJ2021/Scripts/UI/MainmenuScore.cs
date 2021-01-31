using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainmenuScore : MonoBehaviour
{
    public TMP_Text RunText;
    public TMP_Text TotalText;

    int totalScore;
    // Start is called before the first frame update
    void Start()
    {
        RefreshScore();
    }

    void RefreshScore()
    {
        //PlayerPrefs.SetInt("Score", 1337);
        totalScore = PlayerPrefs.GetInt("Score");
        TotalText.text = totalScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

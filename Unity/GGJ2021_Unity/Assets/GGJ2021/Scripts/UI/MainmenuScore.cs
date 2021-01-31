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
      //  RefreshScore();
    }

    void RefreshScore()
    {
        //PlayerPrefs.SetInt("Score", 1337);
     
        TotalText.text = ScoreManager.MaxScore.ToString();

        RunText.text = ScoreManager.Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        RefreshScore();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreLabel : MonoBehaviour
{
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();

        text.text = ScoreManager.Score.ToString();

        ScoreManager.OnScoreChanged += OnScoreChanged;
    }

    private void OnDestroy()
    {
        ScoreManager.OnScoreChanged -= OnScoreChanged;
    }

    // Update is called once per frame
    public void OnScoreChanged(ScoreChangeEvent e) 
    {
        text.text = e.NewScore.ToString();
    }
}

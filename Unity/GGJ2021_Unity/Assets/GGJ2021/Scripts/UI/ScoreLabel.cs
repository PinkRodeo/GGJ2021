using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreLabel : MonoBehaviour
{
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();

        text.text = ScoreManager.Score.ToString();

        // register listeners
        ScoreManager.OnScoreChanged += OnScoreChanged;
        ScoreManager.OnScoreIncreased += OnScoreIncread;
        ScoreManager.OnScoreDecreased += OnScoreDecreased;
    }

    private void OnDestroy()
    {
        // unregister listeners
        ScoreManager.OnScoreChanged -= OnScoreChanged;
        ScoreManager.OnScoreIncreased -= OnScoreIncread;
        ScoreManager.OnScoreDecreased -= OnScoreDecreased;
    }

    // Update is called once per frame
    public void OnScoreChanged(ScoreChangeEvent e)
    {
        text.text = e.NewScore.ToString();
    }

    private void OnScoreDecreased(ScoreChangeEvent e)
    {
        // @TODO: some 'negative' animation
    }

    private void OnScoreIncread(ScoreChangeEvent e)
    {
        // @TODO: some 'positive' animation
    }
}

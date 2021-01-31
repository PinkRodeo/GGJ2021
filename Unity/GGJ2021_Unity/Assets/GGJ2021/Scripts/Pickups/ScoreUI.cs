using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private float currentDisplayScore;

    private TextMeshProUGUI _textMesh;
    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
        _textMesh.text = "BitRate:   0kHz";


        ScoreManager.OnScoreChanged += OnScoreChanged;
        ScoreManager.OnScoreIncreased += OnScoreIncreased;
        ScoreManager.OnScoreDecreased += OnScoreDecreased;
    }

    private void OnDestroy()
    {
        ScoreManager.OnScoreChanged -= OnScoreChanged;
        ScoreManager.OnScoreIncreased -= OnScoreIncreased;
        ScoreManager.OnScoreDecreased -= OnScoreDecreased;
    }

    private void OnScoreChanged(ScoreChangeEvent e)
    {
        // float oldScore = e.OldScore;
        DOTween.To(() => currentDisplayScore, newValue =>
        {
            currentDisplayScore = newValue;
            _textMesh.text = "BitRate:   " + (Mathf.Floor(newValue)).ToString() + "kHz";
        }, e.NewScore, 0.4f);

        float textMoveOffset = 0f;

        if (e.Delta > 0)
        {
            textMoveOffset = 50f;
        }
        else
        {
            textMoveOffset = -20f;
        }

        _textMesh.rectTransform.DOBlendableLocalMoveBy(Vector3.up * textMoveOffset, 0.1f).SetEase(Ease.OutElastic).onComplete += () =>
        {
            _textMesh.rectTransform.DOBlendableLocalMoveBy(Vector3.down * textMoveOffset, 0.5f).SetEase(Ease.InQuad);
        };

        // _textMesh.DOBlendableColor(Color.cyan, 0.1f).onComplete += () =>
        // {
        //     _textMesh.DOBlendableColor(Color.white, 0.2f);
        // };
    }


    private void OnScoreIncreased(ScoreChangeEvent e)
    {

    }


    private void OnScoreDecreased(ScoreChangeEvent e)
    {

    }
}

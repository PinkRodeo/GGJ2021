using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using DG.Tweening;


[RequireComponent(typeof(Collider))]

public class KidPortal : MonoBehaviour
{
    private bool _closed = true;
    public GameObject _visualObject;

    private void Awake()
    {
        _visualObject.transform.localScale = Vector3.zero;

        _closed = true;
        ScoreManager.OnScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(ScoreChangeEvent e)
    {
        if (ScoreManager.Score >= ScoreManager.MaxScore)
        {
            OpenPortal();
        }
        else
        {
            ClosePortal();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (_closed)
            return;

        GameObject other = collider.gameObject;
        if (other.tag == "Player")
        {
            // TODO: Play sound


            SceneManager.LoadScene("MenuVisuals");

        }
    }

    private void OpenPortal()
    {
        if (_closed == false)
        {
            // Already open
            return;
        }

        _closed = false;

        _visualObject.transform.DOScale(Vector3.one, 0.5f);
        // TODO: SFX, animation
    }

    private void ClosePortal()
    {
        if (_closed == true)
        {
            // Already closed
            return;
        }

        // Close animatino

        _visualObject.transform.DOScale(Vector3.zero, 0.5f);

        _closed = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class KidPortal : MonoBehaviour
{
    private bool _closed = true;
    public GameObject visuals;

    private void Awake()
    {
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

        _closed = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

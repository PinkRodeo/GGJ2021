using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{

    private bool _tempDisabled = false;

    public GameObject _visual;

    private AudioSource _hitSound;

    private void Awake()
    {
        _hitSound = GetComponent<AudioSource>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_tempDisabled)
            return;

        GameObject other = collision.gameObject;
        if (other.tag == "Player")
        {
            ScoreManager.Add(-50);

            _tempDisabled = true;

            _hitSound.Play();

            _visual.transform.DOBlendableScaleBy(Vector3.one * 1.1f, 0.15f).SetEase(Ease.OutBack).OnComplete(() =>
                 {
                     _visual.transform.DOBlendableScaleBy(Vector3.one / 1.1f, 0.2f).SetEase(Ease.InQuart).OnComplete(() =>
                     {
                         _tempDisabled = false;
                     });
                 });
        }

        Debug.LogWarning("TODO: Decide if we want to do something when enemies touch you.");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        GameObject other = collider.gameObject;
        if (other.tag == "Player")
        {
            ScoreManager.Add(100);

            // TODO: Play sound

            transform.DOBlendableScaleBy(Vector3.one * -1f, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
                 {
                     Destroy(gameObject);
                 });
        }
    }

    private void OnPickupComplete()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject other = collider.gameObject;
        if (other.tag == "Player")
        {
            var controller = gameObject.GetComponent<PlayerController>();

            if (controller == null)
            {
                controller = gameObject.GetComponentInParent<PlayerController>();
            }

            if (controller == null)
            {
                Debug.LogError("Cannot find the playercontroller component!!!");
            }

            // TODO: Play sound

            // TODO: Add Score

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

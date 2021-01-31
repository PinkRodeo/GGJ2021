using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRotation : MonoBehaviour
{
    public float mainSpeed = 90f;
    public float secondarySpeed = 20f;

    private void Awake()
    {
        transform.Rotate(new Vector3(0, 0, 360f * Random.Range(0f, 1f)), Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, secondarySpeed * Time.deltaTime), Space.Self);
        transform.Rotate(new Vector3(0, mainSpeed * Time.deltaTime, 0), Space.World);
    }
}

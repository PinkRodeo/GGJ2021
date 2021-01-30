using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoIntoRandomDirections : MonoBehaviour
{
    public Vector3 _randomDirection;
    private Vector3 _generalDirection = Vector3.down;

    public float speed = 0.2f;

    public float maxDeviationFactor = 0.3f;
    public float minDelay = 0.4f;
    public float maxDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _generalDirection = Vector3.down;
        Invoke("GoIntoRandomDirection", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _randomDirection * speed * Time.deltaTime * 11f;
    }

    private void GoIntoRandomDirection()
    {
        _randomDirection = Vector3.Lerp(_generalDirection, Random.rotation * Vector3.forward, maxDeviationFactor).normalized;

        Invoke("GoIntoRandomDirection", Random.Range(minDelay, maxDelay));
    }
}

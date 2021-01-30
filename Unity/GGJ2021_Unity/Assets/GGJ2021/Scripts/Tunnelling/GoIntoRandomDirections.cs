using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoIntoRandomDirections : MonoBehaviour
{
    private Vector3 _randomDirection;
    private Vector3 _generalDirection = Vector3.forward;

    public float speed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("GoIntoRandomDirection", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _randomDirection * speed * Time.deltaTime;
    }

    private void GoIntoRandomDirection()
    {
        _randomDirection = Vector3.Lerp(_generalDirection, Random.rotation.eulerAngles, 0.1f).normalized;

        Invoke("GoIntoRandomDirection", Random.Range(0.6f, 1f));
    }
}

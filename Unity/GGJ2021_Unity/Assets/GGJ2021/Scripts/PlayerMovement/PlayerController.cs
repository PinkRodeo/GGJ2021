using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
    
public class PlayerController : MonoBehaviour
{
    
private bool _movingDown = true;
[SerializeField]
private int ForceIntensity = 10;
[SerializeField]
private int SteerIntensity = 2;
Vector3 mousesaved = new Vector3();
Vector3 mouseoffset = new Vector3();

private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        mousesaved = Input.mousePosition;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mousesaved != Input.mousePosition)
        {
              mouseoffset = Input.mousePosition - mousesaved; 
              mousesaved = Input.mousePosition;
              
        } else
        {
            mouseoffset = mouseoffset *0.6f;
        }

        if (_movingDown) 
        {   
            _rb.AddForce(new Vector3 (mouseoffset.x * SteerIntensity, 0, mouseoffset.y* SteerIntensity));
            _rb.AddForce(Vector3.down * ForceIntensity,ForceMode.Acceleration);
            _rb.velocity = new Vector3(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -100, 0), _rb.velocity.z);
            transform.rotation.eulerAngles.Set()
        }
    }
}

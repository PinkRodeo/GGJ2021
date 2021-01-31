using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrmovement : MonoBehaviour {
	public Vector2 rotation = Vector2.zero ;
	public float speed = 3;

    void Start()
    {
        rotation.x = transform.eulerAngles.x;
        rotation.y = transform.eulerAngles.y;
    }
    
	void Update () {
		rotation.y += -Input.GetAxis ("Mouse X");
		rotation.x += -Input.GetAxis ("Mouse Y");
		transform.eulerAngles = (Vector2)rotation * speed;
	}
}

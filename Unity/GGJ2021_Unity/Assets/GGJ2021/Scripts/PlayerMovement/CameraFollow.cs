using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject PlayerItem;
    private float smoothspeed =  0.02f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //transform.LookAt(PlayerItem.transform, Vector3.down);
        //transform.position = new Vector3(0,PlayerItem.transform.position.y, 0);
    }
}

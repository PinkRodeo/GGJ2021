using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFollowsDirection : MonoBehaviour
{
    // Start is called before the first frame update

    private TunnelTracer _tunnelTracer;
    private GoIntoRandomDirections _randomDirectionGoer;
    public GameObject cameraObject;

    void Start()
    {
        _tunnelTracer = GetComponent<TunnelTracer>();
        _randomDirectionGoer = GetComponent<GoIntoRandomDirections>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraObject.transform.rotation = Quaternion.Slerp(
            cameraObject.transform.rotation,
            // Quaternion.LookRotation(_tunnelTracer.currentDirection, Vector3.up),
            Quaternion.LookRotation(_randomDirectionGoer._randomDirection, Vector3.up),
            Time.deltaTime * 0.2f
        );
    }
}

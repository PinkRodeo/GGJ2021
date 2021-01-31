using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteerInputFeedback : MonoBehaviour
{
    public PlayerController playerController;

    // Start is called before the first frame update
    private void Start()
    {
        playerController.onInputUpdated += UpdateInputFeedback;
    }

    private void UpdateInputFeedback()
    {
        var steeringInput = playerController.steeringInput;
        Vector3 newPosition = new Vector3(
            steeringInput.x * 0.29f,
            steeringInput.y * 0.29f,
            -0.249f);

        transform.localPosition = newPosition;

    }

}

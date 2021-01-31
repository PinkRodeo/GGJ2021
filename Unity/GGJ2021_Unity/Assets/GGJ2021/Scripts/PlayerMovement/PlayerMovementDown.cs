using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementDown : MonoBehaviour
{
    [SerializeField]
    private float panSpeed = 50f;

    private Rigidbody _rigidbody;
    private PlayerController _playerController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Pause();
    }

    public void SetupPhysicsAndGo()
    {
        this.enabled = true;

        _rigidbody.useGravity = true;
    }

    public void SetPlayerController(PlayerController playerController)
    {
        _playerController = playerController;
    }

    internal void Pause()
    {
        this.enabled = false;
    }
    void FixedUpdate()
    {
        var steerInput = _playerController.steeringInput;

        // Apply a curve to the inputs
        steerInput.x = Mathf.Sign(steerInput.x) * Mathf.SmoothStep(0f, 1f, Mathf.Abs(steerInput.x));
        steerInput.y = Mathf.Sign(steerInput.y) * Mathf.SmoothStep(0f, 1f, Mathf.Abs(steerInput.y));

        if (_playerController.steerMode == SteerMode.Movement)
        {
            var rotator = Quaternion.LookRotation(transform.forward, transform.up);

            var newDirection = new Vector3(
                steerInput.x * panSpeed * Time.fixedDeltaTime,
                steerInput.y * panSpeed * Time.fixedDeltaTime,
                0f);

            _rigidbody.velocity += rotator * newDirection;
            // (_rigidbody.rotation * Quaternion.Euler(newDirection));
        }
        else
        {
            var newDirection = new Vector3(0, 0, steerInput.x * Time.fixedDeltaTime * -80f);
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(newDirection));
        }

        var rotationBeforeLookingDown = Quaternion.LookRotation(Vector3.down);
        var adjustment = Quaternion.RotateTowards(transform.rotation, rotationBeforeLookingDown, 30f * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(adjustment);


        // _rigidbody.velocity += transform.forward * MaxAcceleration * Time.fixedDeltaTime;
    }
}

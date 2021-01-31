using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovementUp : MonoBehaviour
{
    [SerializeField]
    private float ForceIntensity = 10;
    [SerializeField]
    private int MaxAcceleration = 50;
    [SerializeField]
    private float RotSpeed = 0.2f;

    private Rigidbody _rigidbody;

    private PlayerController _playerController;

    public void SetPlayerController(PlayerController playerController)
    {
        _playerController = playerController;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Pause();
    }

    public void Pause()
    {
        this.enabled = false;
    }

    public void SetupPhysicsAndGo()
    {
        this.enabled = true;

        _rigidbody.useGravity = false;
        _rigidbody.drag = 2.5f;
        _rigidbody.angularDrag = 0.15f;

        // TODO: Ideally the velocity should carry over from before
        _rigidbody.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var steerInput = _playerController.steeringInput;

        // Apply a curve to the inputs
        steerInput.x = Mathf.Sign(steerInput.x) * Mathf.SmoothStep(0f, 1f, Mathf.Abs(steerInput.x));
        steerInput.y = Mathf.Sign(steerInput.y) * Mathf.SmoothStep(0f, 1f, Mathf.Abs(steerInput.y));

        if (_playerController.steerMode == SteerMode.Movement)
        {
            var newDirection = new Vector3(-steerInput.y * RotSpeed * Time.fixedDeltaTime, steerInput.x * RotSpeed * Time.fixedDeltaTime, 0);
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(newDirection));
        }
        else
        {
            var newDirection = new Vector3(0, 0, steerInput.x * Time.fixedDeltaTime * -80f);
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(newDirection));
        }

        _rigidbody.velocity += transform.forward * MaxAcceleration * Time.fixedDeltaTime;
    }

}

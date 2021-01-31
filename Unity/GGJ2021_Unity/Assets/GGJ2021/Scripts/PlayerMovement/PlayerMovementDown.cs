using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementDown : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerController _playerController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Pause();
    }

    private void FixedUpdate()
    {
        // TODO: Make downwards platformer
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
}

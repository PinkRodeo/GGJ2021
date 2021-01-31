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

    private void Start()
    {
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

    }

    public void SetupPhysics()
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

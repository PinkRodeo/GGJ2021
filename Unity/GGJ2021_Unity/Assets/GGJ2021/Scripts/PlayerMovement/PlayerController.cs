using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnInputUpdated();

public enum MoveDirection
{
    Down,
    Up
}

public enum SteerMode
{
    Movement,
    Look
}

public class PlayerController : MonoBehaviour
{
    private PlayerMovementDown _playerMovementDown;
    private PlayerMovementUp _playerMovementUp;

    private MoveDirection currentMoveMode;
    private Vector2 _currentMousePosition;
    private Vector2 _previousMousePosition;

    private SteerMode _steerMode = SteerMode.Movement;

    private bool _hasFocus = true;

    public SteerMode steerMode
    {
        get
        {
            return _steerMode;
        }
    }

    public Vector2 steeringInput
    {
        get
        {
            return _currentMousePosition;
        }
    }

    public OnInputUpdated onInputUpdated;

    private void Awake()
    {
        _playerMovementDown = GetComponent<PlayerMovementDown>();
        _playerMovementUp = GetComponent<PlayerMovementUp>();

        _playerMovementDown.SetPlayerController(this);
        _playerMovementUp.SetPlayerController(this);
    }

    private void Start()
    {
        // currentMoveMode = MoveDirection.Down;
        // _playerControllerDown.SetupPhysics();

        currentMoveMode = MoveDirection.Up;
        _playerMovementUp.SetupPhysics();
    }
    private void FixedUpdate()
    {
        ListenToInput();
    }

    private void ListenToInput()
    {
#if UNITY_EDITOR

        // Debug direction flip
        if (Input.GetKeyDown(KeyCode.R))
        {
            FlipDirection();
        }

        // Debug lock input
        if (Input.GetKeyDown(KeyCode.Y))
        {

        }
#endif

        ListenToInputSteering();

        if (onInputUpdated != null)
            onInputUpdated();
    }

    private void ListenToInputSteering()
    {
        _previousMousePosition = _currentMousePosition;

        if (!_hasFocus)
        {
            _currentMousePosition = Vector2.Lerp(_previousMousePosition, Vector2.zero, Time.fixedDeltaTime * .8f);
            return;
        }

        _currentMousePosition = GetMouseInScreenCoordinates();

        _steerMode = SteerMode.Movement;

        if (Input.GetMouseButton(1))
        {
            _steerMode = SteerMode.Look;
        }
    }

    /// <summary>
    /// Gets the mouse position remapped to a square 
    /// </summary>
    /// <returns></returns>
    private Vector2 GetMouseInScreenCoordinates()
    {
        Vector2 mousePos = Input.mousePosition;

        var scrWidth = Screen.width;
        var scrHeight = Screen.height;

        var minScreenDimension = Mathf.Min(scrWidth, scrHeight);
        var maxScreenDimension = Mathf.Max(scrWidth, scrHeight);

        mousePos.x = Mathf.Clamp(mousePos.x, 0f, scrWidth);
        mousePos.y = Mathf.Clamp(mousePos.y, 0f, scrHeight);

        if (scrWidth > scrHeight)
        {
            var diff = scrWidth - scrHeight;
            mousePos.x = Mathf.Clamp(mousePos.x, diff / 2, scrWidth - diff / 2).Remap(diff / 2, scrWidth - diff / 2, -1f, 1f);
            mousePos.y = mousePos.y.Remap(0, scrHeight, -1f, 1f);
        }
        else
        {
            var diff = scrHeight - scrWidth;
            mousePos.x = mousePos.x.Remap(0, scrWidth, -1f, 1f);
            mousePos.y = Mathf.Clamp(mousePos.y, diff / 2, scrHeight - diff / 2).Remap(diff / 2, scrHeight - diff / 2, -1f, 1f);
        }

        if (mousePos.magnitude > 1f)
            mousePos.Normalize();

        return mousePos;
    }

    /// <summary>
    /// Switch MoveMode into the other direction.
    /// </summary>
    private void FlipDirection()
    {
        if (currentMoveMode == MoveDirection.Down)
        {
            currentMoveMode = MoveDirection.Up;
            _playerMovementDown.Pause();
            _playerMovementUp.SetupPhysics();
        }
        else if (currentMoveMode == MoveDirection.Up)
        {
            currentMoveMode = MoveDirection.Down;
            _playerMovementUp.Pause();
            _playerMovementDown.SetupPhysics();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        _hasFocus = hasFocus;
    }
}

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}

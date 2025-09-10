using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event EventHandler OnPauseAction;

    InputActions inputActions;

    private void Awake()
    {
        Instance = this;
        inputActions = new InputActions();
        inputActions.Enable();

        inputActions.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        inputActions.Disable();
    }

    public bool IsUpPressed()
    {
        return inputActions.Player.Up.IsPressed();
    }

    public bool IsLeftPressed()
    {
        return inputActions.Player.Left.IsPressed();
    }

    public bool IsRightPressed()
    {
        return inputActions.Player.Right.IsPressed();
    }

    public Vector2 GetMovementVector()
    {
        return inputActions.Player.Movement.ReadValue<Vector2>();
    }

}

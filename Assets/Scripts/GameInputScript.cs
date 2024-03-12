using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : NetworkBehaviour
{
    public EventHandler OnAttackAction;
    public EventHandler OnJumpAction;
    public EventHandler OnTestAction;

    public EventHandler OnPickUpAction;
    public static GameInput Instance;

    
    private PlayerInputs playerInputs;
    public PlayerInput playerInput;
    private void Awake() {
        Instance = this;

        playerInputs = new PlayerInputs();
        playerInputs.Player_InGame.Enable();

        playerInputs.Player_InGame.Attack.performed += Attack_performed;
        playerInputs.Player_InGame.Jump.performed += Jump_performed;
        playerInputs.Player_InGame.PickUp.performed += PickUp_performed;
        //this is for testing purpose delete in the final build
        playerInputs.Player_InGame.Test.performed += Test_performed;

    }

//this is for testing and to be deleted
    private void Test_performed(InputAction.CallbackContext context)
    {
        OnTestAction? .Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        OnJumpAction? .Invoke(this,EventArgs.Empty);
    }

    private void Attack_performed(InputAction.CallbackContext context)
    {
        OnAttackAction? .Invoke(this, EventArgs.Empty);
    }

    private void PickUp_performed(InputAction.CallbackContext context){
        OnPickUpAction? .Invoke(this, EventArgs.Empty);
    }
    public Vector3 GetMoveDirections(){
        Vector2 moveDirection = playerInputs.Player_InGame.Movement.ReadValue<Vector2>();
        return new Vector3(moveDirection.x, moveDirection.y, 0);
    }
    public Vector3 GetMosuseInput(){
        Vector3 mouseInput = Vector3.zero;
        mouseInput.x = playerInputs.Player_InGame.MouseX.ReadValue<float>();
        mouseInput.y = playerInputs.Player_InGame.MouseY.ReadValue<float>();
        return mouseInput;
    }
    public Vector3 GetGamepadInput(){
        return playerInputs.Player_InGame.Gamepad_Look.ReadValue<Vector2>();
    }
    public string GetControlScheme(){
        return playerInput.currentControlScheme.ToString();
    }
}

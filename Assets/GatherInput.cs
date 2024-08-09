using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GatherInput : MonoBehaviour
{
    private Controls controls;
    public float valueX = 0;
    public bool jumpInput;
    // Start is called before the first frame update


    public void Awake()
    {
        controls = new Controls();
    }
    

    private void OnEnable(){
        controls.Player.Move.performed += StartMove;
        controls.Player.Move.canceled += StopMove;
        controls.Player.Jump.performed += JumpStart;
        controls.Player.Jump.canceled += JumpStop;



        controls.Player.Enable();


    }
    private void OnDisable(){
        controls.Player.Move.performed -= StartMove;
        controls.Player.Move.canceled -= StopMove;
        controls.Player.Jump.performed -= JumpStart;
        controls.Player.Jump.canceled -= JumpStop;
        controls.Player.Disable();


    }
    private void StartMove(InputAction.CallbackContext ctx) {
        valueX = ctx.ReadValue<float>();
    }
    private void StopMove(InputAction.CallbackContext ctx) {
        valueX = 0;
    }

     private void JumpStart(InputAction.CallbackContext ctx){
        jumpInput = true;
    }
    private void JumpStop(InputAction.CallbackContext ctx){
        jumpInput = false;
    }
}

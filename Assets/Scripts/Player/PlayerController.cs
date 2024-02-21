using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float crouchSpeed;
    
    public LocationManager locMan;
    private Rigidbody rb;
    public InputActionAsset actions;
    
    public InputAction moveVector;
    public InputAction lookVector;
    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitializeComponents(){
        //Components
        rb = GetComponent<Rigidbody>();
        locMan = GetComponent<LocationManager>();
        actions = GetComponent<InputActionAsset>();
        
        //Actions
        moveVector = actions.FindActionMap("Base").FindAction("Move");
        lookVector = actions.FindActionMap("Base").FindAction("Look");
        actions.FindActionMap("Base").FindAction("Jump").performed += OnJump;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
    }
}

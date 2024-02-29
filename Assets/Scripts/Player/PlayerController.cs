using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Damageable
{
    //Movement
    public float walkSpeed;
    public float crouchSpeed;
    private float currSpeed;
    public bool crouched;
    public bool grounded;
    private float drag = .8f;
    public InputAction moveVector;

    //Jump
    public float jumpForce = 100;
    private GameObject me;

    //Camera
    private float mouseX;
    private float mouseY;
    [Range(0.0f, 10.0f)]
    public float mouseSens = 1;
    private float xRot;
    private float yRot;
    public InputAction lookVector;

    //Components
    public LocationManager locMan;
    private Rigidbody rb;
    public InputActionAsset actions;
    public Camera myCam;

    //Location
    public bool audible = true;
    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
        locMan.UpdatePlayerLocation();
    }

    // Update is called once per frame
    void Update()
    {
        CameraUpdate();
        MoveUpdate();
        if (!crouched) { locMan.UpdatePlayerLocation(); }
    }
    void InitializeComponents()
    {
        //Components
        rb = GetComponent<Rigidbody>();

        //Actions
        moveVector = actions.FindAction("Move");
        lookVector = actions.FindAction("Look");
        actions.FindAction("Jump").performed += OnJump;
        actions.FindAction("Crouch").performed += OnCrouch;
        actions.FindActionMap("Base").FindAction("Pause").performed += OnPause;
        actions.Enable();
        //Set Variables
        currSpeed = walkSpeed;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (grounded && !crouched)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            grounded = false;
            locMan.UpdatePlayerLocation();
        }
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        crouched = !crouched;
        currSpeed = crouched ? crouchSpeed : walkSpeed;
        audible = crouched ? true : false;
        if (!crouched)
        {
            locMan.UpdateLocation(myLoc);
        }
    }
    private void OnPause(InputAction.CallbackContext context)
    {
        gm.OnPause();
    }

    private void CameraUpdate()
    {
        mouseX = lookVector.ReadValue<Vector2>().x;
        mouseY = lookVector.ReadValue<Vector2>().y;
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 100);
        myCam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        rb.transform.Rotate(Vector3.up * mouseX * mouseSens);
    }

    private void MoveUpdate()
    {
        Vector3 moveDir = new Vector3(moveVector.ReadValue<Vector2>().x, 0, moveVector.ReadValue<Vector2>().y);
        transform.Translate(moveDir * currSpeed * Time.deltaTime * drag, Space.Self);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            drag = .8f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "DuckSpawnBox")
        {
            locMan.broadcast = true;
        }
    }

    public override void Die()
    {
        Debug.Log($"{this.gameObject.name} took is Dead");
        Time.timeScale = 0;
        gm.gameObject.transform.GetChild(3).gameObject.SetActive(true);
    }


}
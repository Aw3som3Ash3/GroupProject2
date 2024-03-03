using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : PlayerSettings
{
    //Movement
    public bool grounded;
    private float drag = .8f;
    public InputAction moveVector;

    //Jump
    public float jumpForce = 100;
    private GameObject me;
    public GameObject loseMenu;

    //Camera
    private float mouseX;
    private float mouseY;
    private float xRot;
    private float yRot;
    public InputAction lookVector;

    //Components
    public LocationManager locMan;
    private Rigidbody rb;
    public InputActionAsset actions;
    public Camera myCam;
    public float walkSpeed, walkSpeedMod;
    public float crouchSpeed, crouchSpeedMod;
    public float currSpeed;
    public GameObject warning;
    public bool canLook;

    public GameObject crouchIcon;
    public TMP_Text healthText;
    public AudioSource audioSource;
    private bool walking;
    public float airSpeed;
    public float airSpeedMod;
    public bool bread;
    public float CurrSpeed
    {
        get { return currSpeed;}
        set
        {
            currSpeed = value;
            if (!crouched)
            {
                currSpeed = speedBoost ? walkSpeedMod : walkSpeed;
            }
            else
            {
                currSpeed = speedBoost ? crouchSpeedMod : crouchSpeed;
            }
        }
    }
    public bool crouched;

    //Location
    public bool audible = true;
    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
        locMan.UpdatePlayerLocation();
        Time.timeScale = 1f;
        canLook = true;
    }

    // Update is called once per frame
    void Update()
    {
        CameraUpdate();
        MoveUpdate();
        if (!crouched) { locMan.UpdatePlayerLocation(); }

        if (myLoc == Location.PlayerSpawn)
        {
            locMan.broadcast = false;
        }

        healthText.text = "Health: " + health/2;
        audioSource.enabled = walking;
        warning.SetActive(locMan.broadcast);
    }
    void InitializeComponents()
    {
        
        
        //Components
        rb = GetComponent<Rigidbody>();

        //Actions
        moveVector = actions.FindAction("Move");
        actions.FindAction("Move").performed += OnMove;
        actions.FindAction("Move").canceled += OnMoveEnd;
        lookVector = actions.FindAction("Look");
        actions.FindAction("Jump").performed += OnJump;
        actions.FindAction("Crouch").performed += OnCrouch;
        actions.FindActionMap("Base").FindAction("Pause").performed += OnPause;
        actions.Enable();
        //Set Variables
        currSpeed = speedBoost ? walkSpeedMod : walkSpeed;
        if (speedBoost)
        {
            //Debug.Log("Speedy");
        }
        if (invincible)
        {
            //Debug.Log("Invincible");
        }

        audioSource = GetComponent<AudioSource>();
        
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        walking = true;
    }
    private void OnMoveEnd(InputAction.CallbackContext context)
    {
        walking = false;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (grounded && !crouched)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            grounded = false;
            locMan.UpdatePlayerLocation();
            currSpeed = speedBoost ? airSpeedMod : airSpeed;
        }
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        crouched = !crouched;
        if (speedBoost)
        {
            currSpeed = crouched ? crouchSpeedMod : walkSpeedMod;
        }
        else
        {
            currSpeed = crouched ? crouchSpeed : walkSpeed;
        }

        audible = crouched ? true : false;
        if (!crouched)
        {
            locMan.UpdateLocation(myLoc);
        }
        else
        {
            locMan.broadcast = false;
        }
        crouchIcon.SetActive(crouched);
    }
    private void OnPause(InputAction.CallbackContext context)
    {
        gm.OnPause();
        canLook = !gm.paused;
    }

    private void CameraUpdate()
    {
        if (canLook)
        {
            mouseX = lookVector.ReadValue<Vector2>().x;
            mouseY = lookVector.ReadValue<Vector2>().y;
            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -90f, 100);
            myCam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
            rb.transform.Rotate(Vector3.up * mouseX * mouseSens);
        }
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
            if (speedBoost)
            {
                currSpeed = crouched ? crouchSpeedMod : walkSpeedMod;
            }
            else
            {
                currSpeed = crouched ? crouchSpeed : walkSpeed;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Detection"))
        {
            locMan.UpdateLocation(other.gameObject.GetComponent<BoxDetection>().boxLoc);
            locMan.broadcast = true;
            if (other.gameObject.GetComponent<BoxDetection>().boxLoc == Location.PlayerSpawn)
            {
                locMan.broadcast = false;
            }
        }
        //Debug.Log("Entered" + other.gameObject.name);
    }

    public override void Die()
    {
        if (!invincible || !bread)
        {
            //Debug.Log($"{this.gameObject.name} took is Dead");
            Time.timeScale = 0;
            loseMenu.SetActive(true);
            canLook = false;
        }
    }

}
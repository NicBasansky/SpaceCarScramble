// Created by the GameDev.tv team. Let us know what cool things you create
// using this! https://GameDev.tv

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Car.Combat;

public class CarController : MonoBehaviour
{
    [SerializeField] Rigidbody sphereRigidbody;
    [SerializeField] float forwardSpeed;
    [SerializeField] float reverseSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] float distanceCheck = .2f;
    [SerializeField] LayerMask groundLayers;
    [SerializeField] float gravity = 980f;
    Fighter fighter;

    float moveInput;
    float turnInput;
    bool isGrounded;
    bool isDead;
    bool isWaitingOnDetonation;

    void Awake()
    {
        fighter = GetComponent<Fighter>();
    }
    void Start()
    {
        // this simply is making sure we don't have issues with the car body following the sphere
        sphereRigidbody.transform.parent = null;
    }

    void FixedUpdate()
    {
        // make sure any objects you want to drive on are tagged as ground layer
        CheckIfGrounded();
        
        if (isGrounded)
        {
            // make car go
            sphereRigidbody.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        }
        else
        {
            // make the car respond to gravity when it is not grounded
            sphereRigidbody.AddForce(new Vector3(0, -gravity, moveInput));//transform.up * -gravity);
        }
    }

    void Update()
    { 
        MovementInput();
        TurnVehicle();
        MoveCarBodyWithSphere();
        
        if (isDead) return;

        if (Input.GetKeyDown("space"))
        {
            if (!CheckIfWaitingOnProjectileDetonation())
            {
                fighter.FireWeapon();
            }
            
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            fighter.EnableShield(true);
            fighter.AffectShieldLife(-Time.deltaTime);
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            fighter.EnableShield(false);
            
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!CheckIfWaitingOnProjectileDetonation())
            {
                fighter.CycleWeapon();
            }
        }
    }

    private void MovementInput()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        if (moveInput > 0)
        {
            moveInput *= forwardSpeed;
        }
        else
        {
            moveInput *= reverseSpeed;
        }

        if (isDead)
            moveInput = 0;
    }

    void TurnVehicle()
    {
        turnInput = Input.GetAxisRaw("Horizontal");
        if (isDead)
        {
            turnInput = 0;
        }
        float newRotation = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0, newRotation, 0, Space.World);
    }

    void MoveCarBodyWithSphere()
    {
        // With your car game object, be sure that the car body and sphere start in exactly the same position
        // or else things go wrong pretty quickly. The next line is making the car body follow the spehere.
        
        transform.position = sphereRigidbody.transform.position;
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(transform.position, distanceCheck, groundLayers, QueryTriggerInteraction.Ignore);
        if (isGrounded)
        {
           // print("I am grounded, yo");
        }
        else
        {
           // print("well, well, it appears I'm not touching what I believe to be the ground, dude");
        }
    }

    private bool CheckIfWaitingOnProjectileDetonation()
    {
        return isWaitingOnDetonation;
    }

    public void setIsWaitingOnDetonation(bool isWaiting)
    {
        isWaitingOnDetonation = isWaiting;
    }

    public float GetTurnInput()
    {
        return turnInput;
    }

    public void Die()
    {
        isDead = true;
    }

}

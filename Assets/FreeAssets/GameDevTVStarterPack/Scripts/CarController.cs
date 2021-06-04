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
    [SerializeField] EndScreenUI endScreenUI;
    [SerializeField] GameObject pauseScreen;
    //[SerializeField] AudioClip accelAudio;
    AiCarManager aiCarManager;
    [SerializeField] AudioSource engineAudioSource;
    ParticleEmissionStopper emissionStopper;
    
    Fighter fighter;

    float moveInput;
    float turnInput;
    bool isGrounded;
    bool isDead = false;
    bool isWaitingOnDetonation = false;
    bool paused = false;
    bool winScreenUp = false;

    

    void Awake()
    {
        fighter = GetComponent<Fighter>();
        aiCarManager = FindObjectOfType<AiCarManager>();
        emissionStopper = GetComponent<ParticleEmissionStopper>();
        
    }

    void Start()
    {
        // this simply is making sure we don't have issues with the car body following the sphere
        sphereRigidbody.transform.parent = null;
        endScreenUI.gameObject.SetActive(false);
        aiCarManager.endMatch += EndMatch;

        pauseScreen.SetActive(false);
        winScreenUp = false;
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
        if (winScreenUp) return;

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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            paused = !paused;
            if (paused)
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pauseScreen.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    private void MovementInput()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        if (moveInput > 0)
        {
            moveInput *= forwardSpeed;
            
            PlayEngineSounds();
            PlayAfterburner(true);
            
        }
        else if (Mathf.Approximately(moveInput, 0))
        {
            if (engineAudioSource.isPlaying)
            {
                engineAudioSource.Stop();
               
            }
            if (emissionStopper.IsEmitting())
            {
                PlayAfterburner(false);
            }
        }
        else
        {
            moveInput *= reverseSpeed;
            PlayEngineSounds();
            PlayAfterburner(true);
        }


        if (isDead)
            moveInput = 0;
    }

    void PlayAfterburner(bool showEffect)
    {
        if (showEffect && !emissionStopper.IsEmitting())
        {
            emissionStopper.StartAllParticleEmission();
        }
        else if (!showEffect && emissionStopper.IsEmitting())
        {
            emissionStopper.StopAllParticleEmission();
        }
        
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

    void PlayEngineSounds()
    {
        if (!engineAudioSource.isPlaying)
            {
                engineAudioSource.Play();
            }
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
        endScreenUI.gameObject.SetActive(true);
        endScreenUI.SetPlayerWins(false);
    }

    private void EndMatch() // player wins
    {
        endScreenUI.gameObject.SetActive(true);
        endScreenUI.SetPlayerWins(true);
        winScreenUp = true;
    }

}

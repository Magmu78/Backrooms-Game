using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = 30f;
    public float groundDistance = 0.4f;
    float vertical;
    float horizontal;
    public bool isGrounded;

    public Transform groundCheck;

    [Header("Footsteps Parameters")]
    public AudioSource footAudioSource;
    public AudioClip[] footsteps;
    //public float minDistance = 0.2f;
    private float footstepsTimer = 0;

    CharacterController characterController;

    Camera playerCamera;

    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 movement = transform.forward * vertical + transform.right * horizontal;

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(movement * speed * Time.deltaTime);
        HandleFootsteps();

        characterController.Move(velocity);
    }

    public void HandleFootsteps()
    {
        if (!isGrounded || vertical == 0 && horizontal == 0) return;
        footstepsTimer -= Time.deltaTime;

        if(footstepsTimer <= 0)
        {
            if(Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 2.5f))
            {
                footAudioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length - 1)]);
            }
            footstepsTimer = 0.5f;
        }
    }
}

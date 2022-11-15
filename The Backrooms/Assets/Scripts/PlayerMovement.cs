using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = 30f;
    public float groundDistance = 0.4f;
    public bool isGrounded;

    public Transform groundCheck;

    CharacterController characterController;

    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 movement = transform.forward * vertical + transform.right * horizontal;

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(movement * speed * Time.deltaTime);

        characterController.Move(velocity * Time.deltaTime);
    }
}

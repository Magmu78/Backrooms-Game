using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = 30f;
    public float groundDistance = 0.4f;
    float vertical;
    float horizontal;
    public bool isGrounded;
    public float moveSpeed;
    public float sprintSpeed;
    public float SprintFOV;
    public float FOV;
    public float sprintTime;
    public float EEStayTime;
    public Text questText;
    public int foundEasterEggs = 0;
    public Transform[] allEEPositions;
    public Transform spawnLocation;
    public Transform startPos;

    public PostProcessProfile profile;

    public GameObject pauseMenu;

    public GameManager manager;

    [HideInInspector]
    public bool isPausing;

    bool explored1;
    bool explored2;
    bool explored3;

    bool isInAEE;
    bool foundAllEE;

    bool isCursorVisible = false;

    public float sprintFootstepTimer;

    public bool isSprinting;

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
            velocity.y = 0;
        }

        if(explored1 && explored2 && explored3)
        {
            foundAllEE = true;
        }
        else
        {
            foundAllEE = false;
        }

        Vector3 movement = transform.forward * vertical + transform.right * horizontal;

        velocity.y += gravity * deltaTime;
        
        characterController.Move(movement * (isSprinting ? sprintSpeed : moveSpeed) * deltaTime);
        HandleFootsteps();

        characterController.Move(velocity * deltaTime);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, SprintFOV, sprintTime);
        }
        else
        {
            isSprinting = false;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, FOV, sprintTime);
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(!isCursorVisible)
            {
                Cursor.lockState = CursorLockMode.None;
                isCursorVisible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                isCursorVisible = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResumeGame();
        }

        if(!foundAllEE)
        {
            questText.text = "Easter Eggs trouvés : " + foundEasterEggs.ToString() + "/3";
        }
        else
        {
            questText.text = "Vous avez fini toutes les quêtes !";
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit, 2.5f))
            {
                Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward, Color.red, 2.5f);

                switch(raycastHit.transform.tag)
                {
                    case "EasterEggs/e1":
                        transform.position = allEEPositions[0].position;
                        if(!explored1)
                        {
                            foundEasterEggs++;
                            explored1 = true;
                        }
                        isInAEE = true;

                        break;

                    case "EasterEggs/e2":
                        transform.position = allEEPositions[1].position;
                        if (!explored2)
                        {
                            foundEasterEggs++;
                            explored2 = true;
                        }
                        isInAEE = true;

                        break;

                    case "EasterEggs/e3":
                        transform.position = allEEPositions[2].position;
                        if (!explored3)
                        {
                            foundEasterEggs++;
                            explored3 = true;
                        }
                        isInAEE = true;

                        break;
                }
            }
        }   
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

            footstepsTimer = (isSprinting ? sprintFootstepTimer : 0.5f);
        }
    }

    public void PauseResumeGame()
    {
        if (!isPausing)
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            GetComponentInChildren<MouseLook>().enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
            profile.GetSetting<DepthOfField>().active = true;
            isPausing = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            GetComponentInChildren<MouseLook>().enabled = true;
            GetComponent<PlayerMovement>().enabled = true;
            pauseMenu.SetActive(false);
            profile.GetSetting<DepthOfField>().active = false;
            isPausing = false;
        }
    }
}

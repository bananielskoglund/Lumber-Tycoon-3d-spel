using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float crouchingSpeed = 3f;
    public float walkingSpeed = 5f;
    public float jumpSpeed = 7f;
    public float runningSpeed = 9f;
    public float gravity = 30.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public Transform playerTransform;
    public Animator animator;

    [Header("Vision Settings")]
    public GameObject waterObject;
    public GameObject playerObject;

    float curSpeedX;
    float curSpeedY;
    bool isCrouching = false;
    bool isRunning = false;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    // [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        curSpeedY = canMove ? walkingSpeed * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetAxis("Vertical") > 0 && !isRunning)
        {
            animator.SetBool("isWalkingForward", true);
        }
        else 
        {
            animator.SetBool("isWalkingForward", false);
        }
        if (Input.GetAxis("Vertical") < 0 && !isRunning)
        {
            animator.SetBool("isWalkingBack", true);
        }
        else 
        {
            animator.SetBool("isWalkingBack", false);
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < Input.GetAxis("Horizontal"))
        {
            animator.SetBool("isWalkingRight", true);
        }
        else 
        {
            animator.SetBool("isWalkingRight", false);
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < Input.GetAxis("Horizontal")*-1)
        {
            animator.SetBool("isWalkingLeft", true);
        }
        else 
        {
            animator.SetBool("isWalkingLeft", false);
        }

        if (canMove){
            if (Input.GetKey(KeyCode.LeftShift) && (Input.GetAxis("Vertical") > 0))
            {
                curSpeedX = runningSpeed * Input.GetAxis("Vertical");
                animator.SetBool("isRunningForward", true);
                isRunning = true;
            }
            else{
                curSpeedX = walkingSpeed * Input.GetAxis("Vertical");
                animator.SetBool("isRunningForward", false);
                isRunning = false;
            }

            if (Input.GetKey(KeyCode.LeftControl) && characterController.isGrounded)
            {
                curSpeedX = crouchingSpeed * Input.GetAxis("Vertical");
                playerTransform.position = new Vector3(playerTransform.position.x, transform.position.y-0.3f, playerTransform.position.z);
                isCrouching = true;
            }
            else{
                playerTransform.position = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
                isCrouching = false;
            }
        }
        else
        {
            curSpeedX = 0;
        }

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded && !isCrouching && !this.animator.GetCurrentAnimatorStateInfo(0).IsName("Jump Idle"))
        {
            moveDirection.y = jumpSpeed;
            animator.SetBool("isJumping", true);
        }
        else
        {
            moveDirection.y = movementDirectionY;
            animator.SetBool("isJumping", false);
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (playerObject.transform.position.y < waterObject.transform.position.y)
        {
            Debug.Log("UNDER WATER");
        }

    }
}
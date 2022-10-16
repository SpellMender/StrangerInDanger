using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController controller;

    // Movement Variables
    public float moveSpeed = 5f;
    public float runMultiplier = 1.5f;
    float runModifier = 1f;
    Vector3 movementVector;
    float originalStepOffset;

    // Jumping Variables
    public float jumpForce = 4f;
    public float gravity = 9.81f;
    public float groundDetectionGravity = 1f;
    float ySpeed;

    // Mouse Aiming variables
    public float lookSensitivity = 2f;
    public float minLookAngle = -90.0f;
    public float maxLookAngle = 90.0f;
    float rotX;

    // Flashlight
    public Light flashLight;
    public float dimSpeed = 0.05f;
    public float dimRange = 0.05f;
    public float brightest;
    public bool touched;
    bool fadeIn;

    public Transform spawnPoint;

    public bool disabled;

    // Start is called before the first frame update (3 lines)
    void Start()
    {
        //Flashlight Memory
        brightest = flashLight.intensity;

        // Acquires the Character Controller component attached to this game object
        controller = GetComponent<CharacterController>();
        // Saves the stepOffset of the Character Controller for use later on
        originalStepOffset = controller.stepOffset;

        // Locks the cursor to Game Window (Esc. key frees it in editor)
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame (2 lines)
    void Update()
    {
        if (!disabled)
        {
            //Fade Flashlight
            FadeLight();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Moves Character Controller consistent with the framerate at the given speed and XZ Input Direction
            controller.Move(Time.deltaTime * MovementVectorXYZ());

            // Rotates camera with mouse input
            transform.eulerAngles = AimDirection();
        }
    }

    // 3D vector calculated for all directional input (3 lines)
    Vector3 MovementVectorXYZ()
    {
        movementVector = MovementXZ();
        movementVector.y = JumpY();

        // Return InputDirection (normalized to ignore magnitude)
        return movementVector;
    }

    // XZ vector calculated from movement input (5 lines)
    Vector3 MovementXZ()
    {
        // Reset direction vector to prevent acceleration
        Vector3 inputDirectionXZ = Vector3.zero;

        // Shift key makes us run
        if (Input.GetKeyDown(KeyCode.LeftShift)) { runModifier = runMultiplier;}
        if (Input.GetKeyUp(KeyCode.LeftShift)) { runModifier = 1f;}

        // Adds XZ of Forward Vector (Blue Arrow) according to Vertical Input
        //  - forwardXZ has a 0 magnitude when facing directly up or down
        inputDirectionXZ += Input.GetAxisRaw("Vertical") * runModifier * new Vector3(transform.forward.x, 0f, transform.forward.z);
        // Adds XZ of Negative Up Vector (Reverse Green Arrow) according to Vertical Input and Forward Y
        //  - Negative UpVectorXZ is on the same facing plane as ForwardVectorXZ
        //  - Forward Y multiplier prevents input reversal when ForwardXZ has 0 magnitude
        inputDirectionXZ += transform.forward.y * Input.GetAxisRaw("Vertical") * -new Vector3(transform.up.x, 0f, transform.up.z);
        // Adds XZ of Right Vector (Red Arrow) according to Horizontal Input
        inputDirectionXZ += Input.GetAxisRaw("Horizontal") * new Vector3(transform.right.x, 0f, transform.right.z);
        
        // XZ input returned at the magnitude of the move speed
        return moveSpeed * inputDirectionXZ;
    }

    // Y speed calculated from jump input (7 lines)
    // ### CREDIT ### | KETRA GAMES | https://youtu.be/ynh7b-AUSPE | https://dotnetfiddle.net/0GDAm3
    float JumpY()
    {
        // Apply gravity
        ySpeed -= Time.deltaTime * gravity;
        // Prevents Character Controller from attempting to step while jumping
        controller.stepOffset = 0f;
        // Checks grounded state of Character Controller component
        if (controller.isGrounded)
        {
            // Allows Character Controller to step while grounded
            controller.stepOffset = originalStepOffset;
            // Prevents Character Controller from becoming ungrounded before jumping
            ySpeed = -groundDetectionGravity;
            // Applies jumpForce when the jump key is pressed
            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpForce;
            }
        }

        // returns the ySpeed for this frame given gravity and jumpForce
        return ySpeed;
    }

    // Acquires facing angles from mouse input (4 lines)
    // ### CREDIT ### | COOPERATIVE CATERPILLAR | https://www.codegrepper.com/profile/cooperative-caterpillar-5g8vth1p43ts
    Vector3 AimDirection()
    {
        // get the mouse inputs
        float y = Input.GetAxis("Mouse X") * lookSensitivity;
        rotX += Input.GetAxis("Mouse Y") * lookSensitivity;

        // clamp the vertical rotation
        rotX = Mathf.Clamp(rotX, minLookAngle, maxLookAngle);

        // rotate the camera
        return new Vector3(-rotX, transform.eulerAngles.y + y, 0f);
    }

    public void FadeLight()
    {
        //if (touched)
        //{
        //    transform.position = new Vector3(68f, 2f, 8f);
        //    //// Fade Out Light
        //    //if (flashLight.intensity > dimRange + dimSpeed && !fadeIn)
        //    //{
        //    //    flashLight.intensity -= dimSpeed;
        //    //}

        //    // Fade In Light
        //    //if (flashLight.intensity < brightest)
        //    //{
        //    //    flashLight.intensity += dimSpeed;
        //    //}
        //    //else
        //    //{
        //    //    touched = false;
        //    //}
        //    //else
        //    //{
        //    //    Time.timeScale = 1;
        //    //    fadeIn = false;
        //    //    touched = false;
        //    //}

        //    //// Return to spawn
        //    //if (flashLight.intensity <= dimRange && flashLight.intensity >= -dimRange)
        //    //{
        //    //    transform.position = spawnPoint.position;
        //    //    flashLight.intensity = dimRange + dimSpeed;
        //    //    fadeIn = true;
        //    //}
        //}
    }

}
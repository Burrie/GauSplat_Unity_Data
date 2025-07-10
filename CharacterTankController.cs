using UnityEngine;
using UnityEngine.InputSystem; // Required for the New Input System
using UnityEngine.UI; // Required for UI Text display (optional)

[RequireComponent(typeof(Rigidbody))]
public class CharacterTankController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 100.0f;

    [SerializeField] private Text commandText; // Optional: UI text to display commands
    [SerializeField] private GameObject okPanel; // Optional: UI text to display commands

    private Rigidbody rb;
    private PlayerControls playerControls;
    private Vector2 inputVector = Vector2.zero;


    [SerializeField] bool isTutorial = true; // Flag to indicate if this is a tutorial

    ushort count = 0;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerControls = new PlayerControls();
    }


    private void Start()
    {
        if (isTutorial)
        {
            okPanel.SetActive(false); // Hide the panel at the start

            // Optional: Initialize the command text if you have a UI Text component
            if (commandText != null)
            {
                commandText.text = "Press W";
            }
            else
            {
                Debug.LogWarning("Command Text is not assigned. Commands will not be displayed.");
            }
        }
    }

    // OnEnable is called when the object becomes enabled and active.
    private void OnEnable()
    {
        // Subscribe to the events
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;
        playerControls.Player.PrimaryAction.performed += OnPrimaryAction;

        // Enable the action map
        playerControls.Player.Enable();
    }

    // OnDisable is called when the object becomes disabled.
    private void OnDisable()
    {
        // Unsubscribe from the events
        playerControls.Player.Move.performed -= OnMove;
        playerControls.Player.Move.canceled -= OnMove;
        playerControls.Player.PrimaryAction.performed -= OnPrimaryAction;

        // Disable the action map
        playerControls.Player.Disable();
    }

    // This single function now handles both performed and canceled events for movement.
    private void OnMove(InputAction.CallbackContext context)
    {
        // When a key is pressed (performed) or released (canceled), 
        // this reads the current value of the composite binding.
        // It will be (0,1) for W, (-1,0) for A, etc. If no key is pressed, it's (0,0).
        inputVector = context.ReadValue<Vector2>();

        // Detect which key was pressed or released
        if (context.control != null && isTutorial == true)
        {
            string key = context.control.name.ToUpper();
            if (key == "W" || key == "A" || key == "S" || key == "D")
            {
                //if (context.performed)
                //    Debug.Log($"{key} was pressed");
                //else if (context.canceled)
                //    Debug.Log($"{key} was released");


                if (count == 0) //Press W
                {
                    if (key == "W")
                    {
                        count++;
                        commandText.text = "Press S"; // Update command text
                    }
                }
                else if (count == 1) //Press S
                {
                    if (key == "S")
                    {
                        count++;
                        commandText.text = "Press A"; // Update command text
                    }
                }
                else if (count == 2) //Press A
                {
                    if (key == "A")
                    {
                        count++;
                        commandText.text = "Press D"; // Update command text
                    }
                }
                else if (count == 3) //Press D
                {
                    if (key == "D")
                    {
                        count = 0; // Reset the count after completing the sequence
                        commandText.text = "Type \"ok\""; // Reset command text
                        okPanel.SetActive(true); // Show the panel

                        // Here you can trigger any action you want after the sequence is completed.
                    }
                }
            }
        }
    }

    // This function is called by the event when the "PrimaryAction" is triggered.
    private void OnPrimaryAction(InputAction.CallbackContext context)
    {
        Debug.Log("Action Triggered!");
        // Your event-driven action logic goes here.
    }

    // FixedUpdate is the correct place for Rigidbody physics operations.
    private void FixedUpdate()
    {
        // --- MOVEMENT (Forward/Backward) ---
        // We use the Y axis of our inputVector for moving.
        // transform.forward gives the blue-axis direction the character is currently facing.
        Vector3 move = transform.forward * inputVector.y * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // --- ROTATION (Left/Right) ---
        // We use the X axis of our inputVector for rotating.
        // This calculates the rotation angle around the Y-axis (up).
        float turn = inputVector.x * rotateSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}
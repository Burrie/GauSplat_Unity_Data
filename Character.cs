using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 100.0f;

    // References to our actions
    private PlayerInput _playerInput;
    private InputAction _moveAction; // For continuous movement
    private InputAction _pressWAction; // For the event
    private InputAction _pressAAction; // For the event
    private InputAction _pressSAction; // For the event
    private InputAction _pressDAction; // For the event

    // --- EVENT FUNCTIONS ---
    // These functions are now called automatically by the Input System
    // when the corresponding key is pressed.

    private void PressW()
    {
        Debug.Log("W was pressed! Playing forward sound, showing dust particles, etc.");
        // Add any code here you want to run ONCE when W is pressed.
    }

    private void PressA()
    {
        Debug.Log("A was pressed! Triggering left turn signal light.");
        // Add any code here you want to run ONCE when A is pressed.
    }

    private void PressS()
    {
        Debug.Log("S was pressed! Playing reverse beep sound.");
        // Add any code here you want to run ONCE when S is pressed.
    }

    private void PressD()
    {
        Debug.Log("D was pressed! Triggering right turn signal light.");
        // Add any code here you want to run ONCE when D is pressed.
    }

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        // Get references to all the actions by name, if they exist
        _moveAction = _playerInput.actions.FindAction("Move");
        _pressWAction = _playerInput.actions.FindAction("PressW");
        _pressAAction = _playerInput.actions.FindAction("PressA");
        _pressSAction = _playerInput.actions.FindAction("PressS");
        _pressDAction = _playerInput.actions.FindAction("PressD");

        if (_moveAction == null)
            Debug.LogWarning("Move action not found in Input Actions asset.");
        if (_pressWAction == null)
            Debug.LogWarning("PressW action not found in Input Actions asset.");
        if (_pressAAction == null)
            Debug.LogWarning("PressA action not found in Input Actions asset.");
        if (_pressSAction == null)
            Debug.LogWarning("PressS action not found in Input Actions asset.");
        if (_pressDAction == null)
            Debug.LogWarning("PressD action not found in Input Actions asset.");
    }

    private void OnEnable()
    {
        // Subscribe only if the action exists
        if (_pressWAction != null)
            _pressWAction.performed += context => PressW();
        if (_pressAAction != null)
            _pressAAction.performed += context => PressA();
        if (_pressSAction != null)
            _pressSAction.performed += context => PressS();
        if (_pressDAction != null)
            _pressDAction.performed += context => PressD();
    }

    private void OnDisable()
    {
        // Unsubscribe only if the action exists
        if (_pressWAction != null)
            _pressWAction.performed -= context => PressW();
        if (_pressAAction != null)
            _pressAAction.performed -= context => PressA();
        if (_pressSAction != null)
            _pressSAction.performed -= context => PressS();
        if (_pressDAction != null)
            _pressDAction.performed -= context => PressD();
    }

    void Update()
    {
        // --- CONTINUOUS MOVEMENT ---
        // We still read the Vector2 from the "Move" action every frame
        // to ensure smooth, responsive movement and rotation.
        if (_moveAction != null)
        {
            Vector2 moveInput = _moveAction.ReadValue<Vector2>();

            // Move the tank based on the Y-axis of the input (W/S)
            transform.position += transform.forward * moveInput.y * moveSpeed * Time.deltaTime;

            // Rotate the tank based on the X-axis of the input (A/D)
            transform.Rotate(Vector3.up, moveInput.x * rotateSpeed * Time.deltaTime);
        }
    }
}
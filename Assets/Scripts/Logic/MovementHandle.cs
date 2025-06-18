using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Components")]
    private PlayerMover playerMover;

    // Input values
    private Vector2 movementInput;

    private void Awake()
    {
        // Get the PlayerMover component
        playerMover = GetComponent<PlayerMover>();

        if (playerMover == null)
        {
            Debug.LogError("PlayerInputHandler: No PlayerMover component found on " + gameObject.name);
        }
    }

    private void Update()
    {
        // Get input using Unity's built-in Input Manager
        ReadMovementInput();

        // Call the Move method on PlayerMover with the current input
        if (playerMover != null)
        {
            playerMover.Move(movementInput);
        }
    }

    private void ReadMovementInput()
    {
        // Use Unity's default input axes (these are built into Unity)
        float horizontal = Input.GetAxis("Horizontal"); // A/D keys or Left/Right arrows
        float vertical = Input.GetAxis("Vertical");     // W/S keys or Up/Down arrows

        movementInput = new Vector2(horizontal, vertical);
    }

    // Public method to get current movement input
    public Vector2 GetMovementInput()
    {
        return movementInput;
    }
}
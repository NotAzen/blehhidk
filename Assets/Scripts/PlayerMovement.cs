using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // --------------------------------------------------------------------------------- //
    // PUBLIC VARIABLES

    public float acceleration = 1f;

    [Tooltip("Multiply velocity each frame to simulate friction (0..1). 1 = no friction")] // wow look a tooltip idk ill probably make this useful
    [Range(0f, 1f)]
    public float frictionCoefficient = 0.9f;

    [Tooltip("Sudden dash forward. Larger numbers go farther.")]
    public float dashStrength = 10f;

    // --------------------------------------------------------------------------------- //
    // PRIVATE VARIABLES

    private Vector3 inputAcceleration;  // acceleration vector
    private Vector3 inputVelocity;      // velocity vector
    private Vector3 additionalVelocity; // velocity vector added by other means (like dashing)

    private Vector2 moveInput;          // input vector
    private bool dashRequested;         // dash button boolean

    // --------------------------------------------------------------------------------- //

    // movement input handler
    public void OnMove(InputValue value)
    {
        // read input vector and scale it to the acceleration vector
        moveInput = value.Get<Vector2>();
    }

    // dash input handler
    public void OnDash(InputValue value)
    {
        // ummm idk apparently >0.5f for buttons detects presses so like yeah
        if (value.Get<float>() > 0.5f)
        {
            dashRequested = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // calculate player acceleration based on input
        inputAcceleration = new Vector3(moveInput.x, moveInput.y, 0f) * acceleration;

        // update player velocity based on acceleration
        inputVelocity += inputAcceleration * Time.deltaTime;

        // dashing :3
        if (dashRequested)
        {
            // add a sudden burst of velocity in the direction of movement input
            Vector3 dashDirection = new Vector3(moveInput.x, moveInput.y, 0f).normalized;
            additionalVelocity = dashDirection * dashStrength;

            // reset dash request
            dashRequested = false;
        }

        // apply some friction to slow down over time
        inputVelocity *= Mathf.Pow(frictionCoefficient, Time.deltaTime);
        additionalVelocity *= Mathf.Pow(0.0001f, Time.deltaTime);

        // add additional velocity (like from dashing) to player velocity
        Vector3 trueVelocity = inputVelocity + additionalVelocity;

        // if the velocity is a nonzero vector, move the player
        if (trueVelocity != Vector3.zero)
        {
            transform.position += trueVelocity * Time.deltaTime;
        }
    }
}

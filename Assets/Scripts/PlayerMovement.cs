using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 1f;
    [Tooltip("Multiply velocity each frame to simulate friction (0..1). 1 = no friction")] // wow look a tooltip idk ill probably make this useful
    [Range(0f, 1f)]
    public float frictionCoefficient = 0.9f;

    private Vector3 playerAcceleration; // acceleration vector
    private Vector3 playerVelocity;     // velocity vector

    private Vector2 moveInput;          // input vector

    // movement input handler
    public void OnMove(InputValue value)
    {
        // read input vector and scale it to the acceleratoin vector
        moveInput = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        // calculate player acceleration based on input
        playerAcceleration = new Vector3(moveInput.x, moveInput.y, 0f) * acceleration;

        // update player velocity based on acceleration
        playerVelocity += playerAcceleration * Time.deltaTime;

        // apply some friction to slow down over time
        playerVelocity *= Mathf.Pow(frictionCoefficient, Time.deltaTime);

        // if the velocity is a nonzero vector, move the player
        if (playerVelocity != Vector3.zero)
        {
            transform.position += playerVelocity * Time.deltaTime;
        }
    }
}

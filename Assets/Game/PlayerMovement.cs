using UnityEngine;

public sealed class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    public float acceleration;
    public float runMultiplier = 1.5f;
    public float maxVelocity = 5f;

    public InputActions input;

    public void Awake()
    {
        input = new InputActions();
    }

    public void Update()
    {
        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();
        var delta = acceleration * Time.deltaTime * moveInput;

        var maxVel = maxVelocity;
        var running = input.Player.Run.IsPressed();
        if (running)
            maxVel *= runMultiplier;

        var vel = rb.velocity + delta;
        vel.x = Mathf.Clamp(vel.x, -maxVel, maxVel);
        vel.y = Mathf.Clamp(vel.y, -maxVel, maxVel);
        rb.velocity = vel;
    }
}
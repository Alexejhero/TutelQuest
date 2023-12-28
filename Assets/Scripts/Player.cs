using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Player : MonoBehaviour
{
    public float acceleration;
    public float maxVelocity;
    public InputActions _input;
    public Rigidbody2D rb;
    private Vector2 _moveInput;
    private void Awake()
    {
        _input = new InputActions();
    }

    public void Update()
    {
        _moveInput = _input.Player.Move.ReadValue<Vector2>();
        var vel = rb.velocity + acceleration * Time.deltaTime * _moveInput;
        vel.x = Mathf.Clamp(vel.x, -maxVelocity, maxVelocity);
        vel.y = Mathf.Clamp(vel.y, -maxVelocity, maxVelocity);
        rb.velocity = vel;
    }

    public void OnEnable()
    {
        _input.Player.Enable();
    }

    public void OnDisable()
    {
        _input.Player.Disable();
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Player : MonoBehaviour
{
    public static Player main;

    public float acceleration;
    public float runMultiplier = 1.5f;
    public float maxVelocity = 5f;
    public InputActions _input;
    public Rigidbody2D rb;
    public float health = 100f;
    private Vector2 _moveInput;
    private Vector3 _desiredCamOffset;

	private Vector3 camVelocity = Vector3.zero;

	[SerializeField]
	[Range(0.01f,0.9f)]
	private float camSmooth = 0.35f;

	private void Awake()
    {
        main = this;
        _input = new InputActions();
        _desiredCamOffset = Camera.main.transform.position - transform.position;
    }

    public void Update()
    {
        UpdateInput();
        UpdateHealth();
        LerpCamera();
    }

    private void UpdateInput()
    {
        _moveInput = _input.Player.Move.ReadValue<Vector2>();
        var delta = acceleration * Time.deltaTime * _moveInput;

        var maxVel = maxVelocity;
        var running = _input.Player.Run.IsPressed();
        if (running)
            maxVel *= runMultiplier;

        var vel = rb.velocity + delta;
        vel.x = Mathf.Clamp(vel.x, -maxVel, maxVel);
        vel.y = Mathf.Clamp(vel.y, -maxVel, maxVel);
        rb.velocity = vel;
    }

    private void UpdateHealth()
    {
        if (health <= 0)
        {
            Debug.LogWarning("dead");
            health = 100f;
        }
    }

    private void LerpCamera()
    {
		var desiredCamPos = transform.position;
        var camPos = Camera.main.transform.position;
		desiredCamPos.z = camPos.z;
		camPos = Vector3.SmoothDamp(camPos, desiredCamPos, ref camVelocity, camSmooth);
		Camera.main.transform.position = camPos;
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

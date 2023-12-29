using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Player : MonoBehaviour
{
    public Living living;
    public PlayerMovement movement;

    public void OnEnable()
    {
        Camera.main.GetComponent<FollowTransform>().target = transform;
        movement.input.Player.Enable();
        GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void OnDisable()
    {
        movement.input.Player.Disable();
        GetComponent<SpriteRenderer>().sortingOrder = -1;
    }
}

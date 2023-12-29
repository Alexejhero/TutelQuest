using UnityEngine;

public sealed class Player : MonoBehaviour
{
    public static Player activePlayer;
    public Character character;
    public Living living;
    public PlayerMovement movement;

    public void OnEnable()
    {
        Camera.main.GetComponent<FollowTransform>().target = transform;
        movement.input.Player.Enable();
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        activePlayer = this;
    }

    public void OnDisable()
    {
        movement.input.Player.Disable();
        GetComponent<SpriteRenderer>().sortingOrder = -1;
    }
}

using UnityEngine;

public class Player : MonoBehaviour
{
    private void Update()
    {
        Vector2 movementResult = Vector2.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) movementResult.y += 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) movementResult.y -= 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) movementResult.x -= 1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) movementResult.x += 1;
        transform.position += (Vector3)movementResult.normalized * Time.deltaTime;
    }
}

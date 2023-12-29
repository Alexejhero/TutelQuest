using UnityEngine;

public class LavaPool : MonoBehaviour
{
    public float damage;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        var living = other.GetComponent<Living>();
        if (living)
        {
            living.health -= damage;
            Debug.LogWarning($"{name} damaged {living.name} for {damage} ({living.health} health remaining)");
        }
    }

}

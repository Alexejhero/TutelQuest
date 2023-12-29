using UnityEngine;

public class LavaPool : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other == Player.main.GetComponent<Collider2D>())
        {
            Player.main.health -= damage;
            Debug.LogWarning($"{damage} {Player.main.health}");
        }
    }

}

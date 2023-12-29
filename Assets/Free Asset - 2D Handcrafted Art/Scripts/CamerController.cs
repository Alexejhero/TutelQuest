using UnityEngine;

namespace SchizoQuest.Free_Asset___2D_Handcrafted_Art.Scripts
{
    public class CamerController : MonoBehaviour {
        public float speed;
        public float clampLeft;
        public float clampRight;

        private float cameraX;

        // Use this for initialization
        void Start () {
            cameraX = transform.position.x;
		
        }
	
        // Update is called once per frame
        void Update () {
            if (UnityEngine.Input.GetKey(KeyCode.RightArrow) && transform.position.x < clampRight)
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
            if (UnityEngine.Input.GetKey(KeyCode.LeftArrow) && transform.position.x > clampLeft)
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }
            if (UnityEngine.Input.GetKey(KeyCode.Space))
            {
                Debug.Log(cameraX);
            }
        }
    }
}

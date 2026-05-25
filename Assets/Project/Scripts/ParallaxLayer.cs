using UnityEngine;

namespace Project.Scripts
{
    public class ParallaxLayer : MonoBehaviour
    {
        public float moveSpeed = 1f;
        public float resetPositionX = -20f;
        public float startPositionX = 20f;

        void Update()
        {
            moveSpeed = GameManager.instance.speedUp ? 7 : 1;
            transform.Translate(Vector3.left * (moveSpeed * Time.deltaTime));
            if (transform.position.x <= resetPositionX)
            {
                transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
            }
        }
    }
}
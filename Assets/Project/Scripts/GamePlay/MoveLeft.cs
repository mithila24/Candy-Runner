using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts
{
    public class MoveLeft : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float limit = -15f;
        public float originalSpeed;

        private void Start()
        {
            originalSpeed = moveSpeed;
        }

        void Update()
        {
            moveSpeed = GameManager.instance.speedUp ? 15 : originalSpeed;
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            if (transform.position.x <= limit)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
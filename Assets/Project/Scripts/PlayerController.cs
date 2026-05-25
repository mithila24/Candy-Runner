using System.Collections;
using UnityEngine;
using Spine.Unity;

namespace Project.Scripts
{
    public class PlayerController : MonoBehaviour
    {
    
        public float jumpForce = 12f;
        public int maxJumps = 3;
        private int jumpCount = 0;
        private Rigidbody2D rb;
        private SkeletonAnimation skeletonAnimation;
        
        public AudioClip candySound;
        public AudioClip obstacleSound;
        public AudioClip powerUpSound;
        public AudioClip jumpSound;
        public ParticleSystem candyParticle;
        public ParticleSystem hitParticle;
        public ParticleSystem speedParticle;
        public ParticleSystem sheildParticle;
        
        private ParticleSystem candyEffect;
        private ParticleSystem hitEffect;
        public bool isShieldOn;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            PlayAnimation("run", true);
            candyEffect = Instantiate(candyParticle);
            hitEffect = Instantiate(hitParticle);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Jump();
            }
        }

        void Jump()
        {
            
            if (jumpCount >= maxJumps) return;
            if(GameManager.instance.gameover)return;
            PlayAnimation("idle", true);
            GameManager.instance.audioSource.PlayOneShot(jumpSound);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
         
        }

        void PlayAnimation(string animationName, bool loop)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop );
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                jumpCount = 0;
                PlayAnimation("run", true);
            }
           
        }

        public IEnumerator ApplyShield()
        {
            isShieldOn = true;
            sheildParticle.Play();
            Debug.Log("Shield Applied");
            yield return new WaitForSeconds(10f);
            sheildParticle.Stop();
            isShieldOn = false;
        }

        public IEnumerator ApplySpeed()
        {
            GameManager.instance.speedUp = true;
            speedParticle.Play();
            Debug.Log("SpeedUp");
            yield return new WaitForSeconds(15f);
            speedParticle.Stop();
            GameManager.instance.speedUp  = false;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(GameManager.instance.gameover)return;
            if (collision.CompareTag("Candy"))
            {
                candyEffect.transform.position = collision.transform.position;
                GameManager.instance.AddScore(1);
                candyEffect.Play();
                GameManager.instance.audioSource.PlayOneShot(candySound);
                collision.gameObject.SetActive(false);
               
            }
            
            if (collision.CompareTag("Obs"))
            {
                if(isShieldOn)return;
                hitEffect.transform.position = collision.transform.position;
                hitEffect.Play();
                collision.gameObject.SetActive(false);
                GameManager.instance.audioSource.PlayOneShot(obstacleSound);
                GameManager.instance.HitObstacle();
               
            }
            
            if (collision.CompareTag("shield"))
            {
                StartCoroutine(ApplyShield());
                GameManager.instance.audioSource.PlayOneShot(powerUpSound);
                collision.gameObject.SetActive(false);
            }

            if (collision.CompareTag("boost"))
            {
                StartCoroutine(ApplySpeed());
                GameManager.instance.audioSource.PlayOneShot(powerUpSound);
                collision.gameObject.SetActive(false);
            }

        }
    }
}
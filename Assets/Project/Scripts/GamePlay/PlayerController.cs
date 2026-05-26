using System.Collections;
using UnityEngine;
using Spine.Unity;

namespace Project.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float jumpForce = 12f;
        public int maxJumps = 3;

        private int jumpCount = 0;

        private Rigidbody2D rb;
        private SkeletonAnimation skeletonAnimation;

        [Header("Audio")]
        public AudioClip candySound;
        public AudioClip obstacleSound;
        public AudioClip powerUpSound;
        public AudioClip jumpSound;

        [Header("Particles")]
        public ParticleSystem candyParticle;
        public ParticleSystem hitParticle;
        public ParticleSystem speedParticle;
        public ParticleSystem sheildParticle;

        private ParticleSystem candyEffect;
        private ParticleSystem hitEffect;

        [Header("PowerUps")]
        public bool isShieldOn;

        private Coroutine shieldRoutine;
        private Coroutine speedRoutine;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();

            skeletonAnimation = GetComponent<SkeletonAnimation>();

            // NULL SAFETY
            if(rb == null)
            {
                Debug.LogError("Rigidbody2D Missing!");
            }

            if(skeletonAnimation == null)
            {
                Debug.LogError("SkeletonAnimation Missing!");
            }

            PlayAnimation("run", true);

            // CREATE PARTICLE INSTANCES
            if(candyParticle != null)
            {
                candyEffect = Instantiate(candyParticle);
            }

            if(hitParticle != null)
            {
                hitEffect = Instantiate(hitParticle);
            }
        }

        private void Update()
        {
            if(GameManager.instance == null)
            {
                return;
            }

            if(GameManager.instance.gameover)
            {
                return;
            }

            // INPUT
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Jump();
            }
        }

        private void Jump()
        {
            // MAX JUMPS CHECK
            if (jumpCount >= maxJumps)
            {
                return;
            }

            // GAMEOVER CHECK
            if(GameManager.instance.gameover)
            {
                return;
            }

            PlayAnimation("idle", true);

            // PLAY SOUND
            if(GameManager.instance.audioSource != null && jumpSound != null)
            {
                GameManager.instance.audioSource.PlayOneShot(jumpSound);
            }

            // RESET Y VELOCITY
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            // APPLY FORCE
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }

        private void PlayAnimation(string animationName, bool loop)
        {
            if(skeletonAnimation == null)
            {
                return;
            }

            skeletonAnimation
                .AnimationState
                .SetAnimation(
                    0,
                    animationName,
                    loop
                );
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                jumpCount = 0;
                PlayAnimation("run", true);
            }
        }

        // SHIELD POWERUP
        public IEnumerator ApplyShield()
        {
            // RESTART TIMER IF ALREADY ACTIVE
            if(shieldRoutine != null)
            {
                StopCoroutine(shieldRoutine);
            }

            isShieldOn = true;

            Debug.Log("Shield Applied");

            if(sheildParticle != null)
            {
                sheildParticle.Play();
            }

            yield return new WaitForSeconds(10f);

            isShieldOn = false;

            if(sheildParticle != null)
            {
                sheildParticle.Stop();
            }

            Debug.Log("Shield Ended");
        }

        // SPEED POWERUP
        public IEnumerator ApplySpeed()
        {
            // RESTART TIMER IF ALREADY ACTIVE
            if(speedRoutine != null)
            {
                StopCoroutine(speedRoutine);
            }

            GameManager.instance.speedUp = true;

            Debug.Log("Speed Boost Applied");

            if(speedParticle != null)
            {
                speedParticle.Play();
            }

            yield return new WaitForSeconds(15f);

            GameManager.instance.speedUp = false;

            if(speedParticle != null)
            {
                speedParticle.Stop();
            }

            Debug.Log("Speed Boost Ended");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(GameManager.instance == null)
            {
                return;
            }

            if(GameManager.instance.gameover)
            {
                return;
            }

            // CANDY
            if (collision.CompareTag("Candy"))
            {
                if(candyEffect != null)
                {
                    candyEffect.transform.position = collision.transform.position;
                    candyEffect.Play();
                }

                GameManager.instance.AddScore(1);

                // SOUND
                if(GameManager.instance.audioSource != null && candySound != null)
                {
                    GameManager.instance.audioSource.PlayOneShot(candySound);
                }

                collision.gameObject.SetActive(false);
            }

            // OBSTACLE
            if (collision.CompareTag("Obs"))
            {
                // SHIELD PROTECTION
                if(isShieldOn)
                {
                    collision.gameObject.SetActive(false);
                    Debug.Log("Obstacle Blocked By Shield");
                    return;
                }

                if(hitEffect != null)
                {
                    hitEffect.transform.position = collision.transform.position;
                    hitEffect.Play();
                }
                collision.gameObject.SetActive(false);

                // SOUND
                if(GameManager.instance.audioSource != null && obstacleSound != null)
                {
                    GameManager.instance.audioSource.PlayOneShot(obstacleSound);
                }

                GameManager.instance.HitObstacle();
            }

            // SHIELD POWERUP
            if (collision.CompareTag("shield"))
            {
                if(shieldRoutine != null)
                {
                    StopCoroutine(shieldRoutine);
                }

                shieldRoutine = StartCoroutine(ApplyShield());

                if(GameManager.instance.audioSource != null && powerUpSound != null)
                {
                    GameManager.instance.audioSource.PlayOneShot(powerUpSound);
                }

                collision.gameObject.SetActive(false);
            }

            // BOOST POWERUP
            if (collision.CompareTag("boost"))
            {
                if(speedRoutine != null)
                {
                    StopCoroutine(speedRoutine);
                }

                speedRoutine = StartCoroutine(ApplySpeed());

                if(GameManager.instance.audioSource != null && powerUpSound != null)
                {
                    GameManager.instance.audioSource.PlayOneShot(powerUpSound);
                }

                collision.gameObject.SetActive(false);
            }
        }

        // RESET PLAYER STATE
        public void ResetPlayer()
        {
            jumpCount = 0;

            isShieldOn = false;

            if(speedParticle != null)
            {
                speedParticle.Stop();
            }

            if(sheildParticle != null)
            {
                sheildParticle.Stop();
            }

            GameManager.instance.speedUp = false;

            rb.linearVelocity = Vector2.zero;

            PlayAnimation("run", true);

            Debug.Log("Player Reset");
        }
    }
}
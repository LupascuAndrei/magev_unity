using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Simple WASD movement controller with left-click attack
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public Character Character;
        public float MoveSpeed = 5f;
        
        private Vector3 _movement = Vector3.zero;
        private bool _isAttacking = false;
        private bool _canMove = true;

        public void Start()
        {
            // Set character to ready state
            if (Character != null)
            {
                Character.Animator.SetBool("Ready", true);
            }
        }

        public void Update()
        {
            // Only handle input and movement when game is not paused
            if (Time.timeScale > 0 && _canMove)
            {
                HandleInput();
                HandleMovement();
                UpdateAnimation();
            }
        }

        public void SetCanMove(bool canMove)
        {
            _canMove = canMove;
        }

        private void HandleInput()
        {
            // Get WASD input
            Vector2 direction = Vector2.zero;

            if (Input.GetKey(KeyCode.W)) direction.y = 1;
            if (Input.GetKey(KeyCode.S)) direction.y = -1;
            if (Input.GetKey(KeyCode.A)) direction.x = -1;
            if (Input.GetKey(KeyCode.D)) direction.x = 1;

            // Normalize diagonal movement
            if (direction.magnitude > 1)
            {
                direction.Normalize();
            }

            _movement = new Vector3(direction.x, direction.y, 0) * MoveSpeed;

            // Handle attack on left click
            if (Input.GetMouseButtonDown(0) && Character != null)
            {
                Character.Slash();
                _isAttacking = true;
                StartCoroutine(ResetAttackFlag());
            }

            // Flip character based on movement direction
            if (direction.x != 0 && Character != null)
            {
                Character.transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
            }
        }

        private void HandleMovement()
        {
            // Move the player GameObject
            transform.position += _movement * Time.deltaTime;
        }

        private void UpdateAnimation()
        {
            if (Character == null) return;

            // Set animation state based on movement
            if (_isAttacking)
            {
                // Attack animation is playing
                return;
            }

            if (_movement.magnitude > 0.1f)
            {
                Character.SetState(CharacterState.Run);
            }
            else
            {
                Character.SetState(CharacterState.Idle);
            }
        }

        private System.Collections.IEnumerator ResetAttackFlag()
        {
            yield return new UnityEngine.WaitForSeconds(0.5f);
            _isAttacking = false;
        }
    }
}


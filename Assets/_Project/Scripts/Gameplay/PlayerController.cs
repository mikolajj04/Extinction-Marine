using UnityEngine;
using UnityEngine.InputSystem;

namespace ExtinctionMarine.Gameplay
{
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;

        private Rigidbody2D rb;
        private Vector2 moveInput;

        private void Awake()
        {
            
            rb = GetComponent<Rigidbody2D>();
        }

        public void OnMove(InputValue value)
        {
         
            moveInput = value.Get<Vector2>();
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MovePlayer()
        {
           
            Vector2 movement = moveInput.normalized * moveSpeed;

            rb.linearVelocity = movement;
        }
    }
} 
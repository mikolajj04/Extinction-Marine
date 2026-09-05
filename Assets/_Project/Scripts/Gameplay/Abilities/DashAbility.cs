using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ExtinctionMarine.Gameplay.Controllers;
using ExtinctionMarine.Gameplay.Systems;
using UnityEngine;
using UnityEngine.InputSystem;


namespace ExtinctionMarine.Gameplay.Abilities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class DashAbility : MonoBehaviour
    {
        [Header("Dash Mechanics")]
        [SerializeField] private float dashSpeed = 20f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 4f;
        private bool isDashUnlocked = false;

        public bool IsDashing { get; private set; }
        private bool canDash = true;
        private Rigidbody2D rb;
        private PlayerController playerController;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerController = GetComponent<PlayerController>();
        }
        private void Start()
        {
            ArmorySaveData save = SaveSystem.Load<ArmorySaveData>("marine_armory.json");

            isDashUnlocked = save.IsDashUnlocked && save.EquippedAbility == "DASH";
        }
        public void OnDash(InputValue value)
        {
            if(isDashUnlocked && canDash && !playerController.IsDead && value.isPressed)
            {
                StartCoroutine(DashRoutine());
            }
        }

        private IEnumerator DashRoutine()
        {
            IsDashing = true;
            canDash = false;
            Vector2 dashDirection = playerController.MoveInput.normalized;
            if (dashDirection == Vector2.zero)
            {
                dashDirection = playerController.AimDirection.normalized;
            }
            if (dashDirection == Vector2.zero) dashDirection = Vector2.right;
            rb.linearVelocity = dashDirection * dashSpeed;
            yield return new WaitForSeconds(dashDuration);
            IsDashing = false;
            rb.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;

        }
    }
}

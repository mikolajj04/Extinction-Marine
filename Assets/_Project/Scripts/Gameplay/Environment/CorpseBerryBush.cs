using UnityEngine;
using ExtinctionMarine.Gameplay.Controllers;

namespace ExtinctionMarine.Gameplay.Environment
{
    [RequireComponent(typeof(Collider2D))]
    public class CorpseBerry : MonoBehaviour
    {
        [Header("Corpse Berry Settings")]
        [SerializeField] private float effectAmount = 15f;
        [Tooltip("Drop empty bush sprite here")]
        [SerializeField] private Sprite emptyBushSprite;

        private SpriteRenderer spriteRenderer;
        private bool hasBerries = true;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!hasBerries) return;

            if (collision.TryGetComponent<PlayerController>(out var player))
            {
                bool isToxic = Random.value > 0.6f;

                if (isToxic)
                {
                    player.ApplyDamage(effectAmount);
                    Debug.LogWarning($"[Environment] Corpse Berry was TOXIC! Marine lost {effectAmount} HP.");
                }
                else
                {
                    player.ApplyHeal(effectAmount);
                    Debug.Log($"[Environment] Corpse Berry was safe. Marine recovered {effectAmount} HP.");
                }

                hasBerries = false;
                if (spriteRenderer != null && emptyBushSprite != null)
                {
                    spriteRenderer.sprite = emptyBushSprite;
                }
            }
        }
    }
}
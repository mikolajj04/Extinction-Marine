using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay.Controllers;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Environment
{
    [RequireComponent(typeof(Collider2D))]
    public class BerryBush : MonoBehaviour
    {
        [Header("Bush Settings")]
        [SerializeField] private float healAmount = 5f;
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
               
                player.ApplyHeal(healAmount);

               
                hasBerries = false;
                if (spriteRenderer != null && emptyBushSprite != null)
                {
                    spriteRenderer.sprite = emptyBushSprite;
                }

                Debug.Log($"[Environment] Marine consumed sweet berries. Recovered {healAmount} HP.");
            }


        }
    }
}

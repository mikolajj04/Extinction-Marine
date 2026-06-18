using UnityEngine;
using System;

namespace ExtinctionMarine.Gameplay
{
    public class ExpGem : MonoBehaviour
    {
        [SerializeField] private float expAmount = 10f; 
        private Action<ExpGem> returnToPool;

        public void Initialize(Vector3 position, Action<ExpGem> onCollectCallback)
        {
            transform.position = position;
            returnToPool = onCollectCallback;
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
           
            if (collision.TryGetComponent<PlayerController>(out var player))
            {
                player.AddExperience(expAmount); 

                gameObject.SetActive(false);     
                returnToPool?.Invoke(this);     
            }
        }
    }
}

using UnityEngine;

namespace ExtinctionMarine.Gameplay.Systems
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        [Header("Audio Sources")]
        [SerializeField] private AudioSource uiSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource gemSource;
        [SerializeField] private AudioSource movementSource;

        [Header("UI Clips")]
        [SerializeField] private AudioClip hoverClip;
        [SerializeField] private AudioClip clickClip;

        [Header("Gameplay Clips")]
        [SerializeField] private AudioClip shotClip;
        [SerializeField] private AudioClip hitClip;
        [SerializeField] private AudioClip expGemPickingClip;
        [SerializeField] private AudioClip playerHurtClip;
        [SerializeField] private AudioClip levelUpClip;
        [SerializeField] private AudioClip HealingBerryClip;

        [Header("Movement")]
        [SerializeField] private AudioClip footstepClip;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        //UI
        public void PlayUIHover() => uiSource.PlayOneShot(hoverClip);
        public void PlayUIClick() => uiSource.PlayOneShot(clickClip);


        //GAMEPLAY
        public void PlayShoot()
        {
            if (hitClip == null) return;
            sfxSource.pitch = Random.Range(0.8f, 1.5f);
            sfxSource.PlayOneShot(shotClip, 0.3f);
        }
        public void PlayHit()
        {
            if (hitClip == null) return;
            sfxSource.pitch = Random.Range(0.9f, 1.2f);
            sfxSource.PlayOneShot(hitClip, 0.3f);
        }

        public void PlayPlayerHurt()
        {
            if (playerHurtClip == null) return;
            sfxSource.pitch = Random.Range(0.5f, 1.5f);
            sfxSource.PlayOneShot(playerHurtClip, 1.0f);
        }

        public void PlayExpGemPickup()
        {
            if (expGemPickingClip == null) return;
            gemSource.pitch = Random.Range(0.95f, 1.1f);
            gemSource.PlayOneShot(expGemPickingClip, 0.25f);
        }
        public void PlayLevelUp()
        {
            if (levelUpClip == null) return;
            sfxSource.PlayOneShot(levelUpClip, 0.6f);
        }
        public void PlayHealingBerry()
        {
            if (HealingBerryClip == null) return;
            gemSource.pitch = Random.Range(0.8f, 1f);
            sfxSource.PlayOneShot(HealingBerryClip, 0.5f);
        }
        public void PlayFootstep()
        {
            if (footstepClip == null) return;

            movementSource.pitch = Random.Range(0.5f, 1.5f);
            movementSource.PlayOneShot(footstepClip, 0.5f);
        }
    }
}

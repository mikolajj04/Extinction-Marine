
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Systems
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        private float currentMusicVolume = 1.0f;
        private float currentSfxVolume = 1.0f;
        [Header("Music Settings")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip menuMusicClip;
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
        [SerializeField] private AudioClip healingBerryClip;
        [SerializeField] private AudioClip marineDeathClip;

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
            SettingsSaveData data = SaveSystem.Load<SettingsSaveData>("settings.json");
            currentMusicVolume = data.MusicVolume;
            currentSfxVolume = data.SfxVolume;
            ApplyVolumes();
        }
        //MUSIC
        public void PlayMenuMusic()
        {
            if (musicSource == null || menuMusicClip == null) return;

            if (musicSource.isPlaying && musicSource.clip == menuMusicClip) return;

            musicSource.clip = menuMusicClip;
            musicSource.loop = true;      
            musicSource.pitch = 1.0f;     
            musicSource.Play();
        }

        
        public void StopMusic()
        {
            if (musicSource != null)
            {
                musicSource.Stop();
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
            if (healingBerryClip == null) return;
            gemSource.pitch = Random.Range(0.8f, 1f);
            sfxSource.PlayOneShot(healingBerryClip, 0.5f);
        }
        public void PlayFootstep()
        {
            if (footstepClip == null) return;

            movementSource.pitch = Random.Range(0.5f, 1.5f);
            movementSource.PlayOneShot(footstepClip, 0.5f);
        }
        public void PlayMarineDeath()
        {
            if (marineDeathClip == null) return;
            sfxSource.PlayOneShot(marineDeathClip, 1f);
        }

        //SETTINGS
        public void SetMusicVolume(float volume)
        {
            currentMusicVolume = Mathf.Clamp01(volume);
            ApplyVolumes();
        }
        public void SetSfxVolume(float volume)
        {
            currentSfxVolume = Mathf.Clamp01(volume);
            ApplyVolumes();
        }
        private void ApplyVolumes()
        {
            if (musicSource != null) musicSource.volume = 0.5f * currentMusicVolume;
            if (uiSource != null) uiSource.volume = 1.0f * currentSfxVolume;
            if (sfxSource != null) sfxSource.volume = 1.0f * currentSfxVolume;
            if (gemSource != null) gemSource.volume = 1.0f * currentSfxVolume;
            if (movementSource != null) movementSource.volume = 1.0f * currentSfxVolume;
        }

    }
}

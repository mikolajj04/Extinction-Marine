using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace ExtinctionMarine.Gameplay.Controllers
{
    public class SettingsUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        private float lastSfxSamplePlayTime = 0f;
        private float sfxSampleCooldown = 0.25f;


        private void OnEnable()
        {
            SettingsSaveData data = SaveSystem.Load<SettingsSaveData>("settings.json");
            if (musicSlider != null) musicSlider.value = data.MusicVolume;
            if (sfxSlider != null) sfxSlider.value = data.SfxVolume;
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        }

        private void OnDisable()
        {
            musicSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.RemoveAllListeners();
        }

        private void OnMusicVolumeChanged(float value)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(value);
            }
            SaveSettings();
        }
        private void OnSfxVolumeChanged(float value)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetSfxVolume(value);
            }
            SaveSettings();

            if (Time.unscaledTime - lastSfxSamplePlayTime >= sfxSampleCooldown)
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayShoot();
                }
                lastSfxSamplePlayTime = Time.unscaledTime;
            }
        }
        private void SaveSettings()
        {
            SettingsSaveData data = SaveSystem.Load<SettingsSaveData>("settings.json");
            data.MusicVolume = musicSlider.value;
            data.SfxVolume = sfxSlider.value;
            SaveSystem.Save(data, "settings.json");
        }
    }
}

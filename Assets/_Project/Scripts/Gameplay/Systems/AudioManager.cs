using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Systems
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        [Header("Audio Sources")]
        [SerializeField] private AudioSource uiSource;

        [Header("UI Clips")]
        [SerializeField] private AudioClip hoverClip;
        [SerializeField] private AudioClip clickClip;
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

        public void PlayUIHover() => uiSource.PlayOneShot(hoverClip);
        public void PlayUIClick() => uiSource.PlayOneShot(clickClip);
    }
}

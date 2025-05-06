using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{

    public class AudioManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private AudioAsset m_audioAsset;
        [SerializeField] private AudioSource m_bgmAudioSource;
        [SerializeField] private AudioSource m_sfxAudioSource;

        #endregion Variables

        #region Unity Methods
        private void Awake()
        {
            _Init();
        }
        private void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.OnToggleMusic, Callback_On_Toggle_Music);
            GlobalEventHandler.AddListener(EventID.OnToggleSFX, Callback_On_Toggle_SFX);
            GlobalEventHandler.AddListener(EventID.RequestToPlayBGMWithId, Callback_On_BGM_Requested_With_Id);
            GlobalEventHandler.AddListener(EventID.RequestToPlaySFXWithId, Callback_On_SFX_Requested_With_Id);

            GlobalEventHandler.AddListener(EventID.RequestToPlayBGMWithClip, Callback_On_BGM_Requested_With_Clip);
            GlobalEventHandler.AddListener(EventID.RequestToPlaySFXWithClip, Callback_On_SFX_Requested_With_Clip);

            GlobalEventHandler.AddListener(EventID.RequestToStopSFX, Callback_On_SFX_Stop_Requested);

        }

        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.OnToggleMusic, Callback_On_Toggle_Music);
            GlobalEventHandler.RemoveListener(EventID.OnToggleSFX, Callback_On_Toggle_SFX);
            GlobalEventHandler.RemoveListener(EventID.RequestToPlayBGMWithId, Callback_On_BGM_Requested_With_Id);
            GlobalEventHandler.RemoveListener(EventID.RequestToPlaySFXWithId, Callback_On_SFX_Requested_With_Id);

            GlobalEventHandler.RemoveListener(EventID.RequestToPlayBGMWithClip, Callback_On_BGM_Requested_With_Clip);
            GlobalEventHandler.RemoveListener(EventID.RequestToPlaySFXWithClip, Callback_On_SFX_Requested_With_Clip);

            GlobalEventHandler.RemoveListener(EventID.RequestToStopSFX, Callback_On_SFX_Stop_Requested);
        }

        #endregion Unity Methods

        #region Private Methods

        private void _Init()
        {
            if (!(PlayerPrefs.HasKey(Konstants.MUSIC_KEY) && PlayerPrefs.HasKey(Konstants.SFX_KEY)))
            {
                PlayerPrefs.SetInt(Konstants.MUSIC_KEY, 1);
                PlayerPrefs.SetInt(Konstants.SFX_KEY, 1);
            }
            m_bgmAudioSource = gameObject.AddComponent<AudioSource>();
            m_sfxAudioSource = gameObject.AddComponent<AudioSource>();
            m_bgmAudioSource.playOnAwake = false;
            m_sfxAudioSource.playOnAwake = false;
            m_sfxAudioSource.loop = false;
            m_bgmAudioSource.loop = true;
            m_bgmAudioSource.volume = .9f;
            GlobalVariables.IsMusicEnabled = PlayerPrefs.GetInt(Konstants.MUSIC_KEY, 1) == 1;
            GlobalVariables.IsSFXEnabled = PlayerPrefs.GetInt(Konstants.SFX_KEY, 1) == 1;
            m_bgmAudioSource.mute = !GlobalVariables.IsMusicEnabled;
            m_sfxAudioSource.mute = !GlobalVariables.IsSFXEnabled;
        }
        private void _PlaySFX(AudioID audioID)
        {
            AudioClip clip = m_audioAsset.GetAudioClipByID(audioID);
            if (clip)
                m_sfxAudioSource.PlayOneShot(clip);
            else
                Debug.LogError($"_PLAY SFX Null::");
        }

        private void _PlaySFX(AudioClip clip)
        {
            if (clip)
                m_sfxAudioSource.PlayOneShot(clip);
            else
                Debug.LogError($"_PLAY SFX Null::");
        }
        private void _StopSFX()
        {
            m_sfxAudioSource.Stop();
        }
        private void _PlayBGM(AudioID audioID)
        {
            AudioClip clip = m_audioAsset.GetAudioClipByID(audioID);
            if (clip)
            {
                m_bgmAudioSource.clip = clip;
                m_bgmAudioSource.Play();
            }
            else
                Debug.Log($"_PLAY BGM Null::");
        }
        private void _PlayBGM(AudioClip clip)
        {
            if (clip)
            {
                m_bgmAudioSource.clip = clip;
                m_bgmAudioSource.Play();
            }
            else
                Debug.Log($"_PLAY BGM Null::");
        }
        private void _ToggleMusic(bool value)
        {
            m_bgmAudioSource.mute = !value;
            PlayerPrefs.SetInt(Konstants.MUSIC_KEY, value ? 1 : 0);
        }
        private void _ToggleSound(bool value)
        {
            m_sfxAudioSource.mute = !value;
            PlayerPrefs.SetInt(Konstants.SFX_KEY, value ? 1 : 0);
        }
        #endregion Private Methods


        #region Callbacks

        private void Callback_On_SFX_Stop_Requested(object args)
        {
            _StopSFX();
        }
        private void Callback_On_SFX_Requested_With_Id(object args)
        {
            _PlaySFX((AudioID)args);
        }

        private void Callback_On_BGM_Requested_With_Id(object args)
        {
            _PlayBGM((AudioID)args);

        }
        private void Callback_On_SFX_Requested_With_Clip(object args)
        {
            _PlaySFX((AudioClip)args);
        }

        private void Callback_On_BGM_Requested_With_Clip(object args)
        {
            _PlayBGM((AudioClip)args);
        }

        private void Callback_On_Toggle_SFX(object args)
        {
            _ToggleSound((bool)args);
        }

        private void Callback_On_Toggle_Music(object args)
        {
            _ToggleMusic((bool)args);
        }
        #endregion Callbacks
    }

}

using AYellowpaper.SerializedCollections;
using System.Collections;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "newAudioAsset", menuName = "SO/Audio/AudioAsset")]
    public class AudioAsset : ScriptableObject
    {

        [SerializeField] private SerializedDictionary<AudioID, AudioClip> _audioDatabase;

        public AudioClip GetAudioClipByID(AudioID id)
        {
            return _audioDatabase[id];
        }
        public AudioClip GetAudioClipByID(int id)
        {
            return _audioDatabase[(AudioID)id];
        }

    }

    public enum AudioID
    {
        LevelCompleteSFX,
        ButtonClickSFX,
        MatchFailSFX,
        MatchSuccessSFX,
    }

}
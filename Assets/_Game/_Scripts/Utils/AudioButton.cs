using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    [RequireComponent(typeof(Button))]
    public class AudioButton : MonoBehaviour
    {
        [SerializeField] private AudioID audioID = AudioID.ButtonClickSFX;
        private Button _button;
        private void OnEnable()
        {
            if (TryGetComponent(out _button))
            {
                _button.onClick.AddListener(_PlaySFX);
            }
        }
        private void OnDisable()
        {
            if (_button)
                _button.onClick.RemoveListener(_PlaySFX);
        }
        private void _PlaySFX()
        {
            GlobalEventHandler.TriggerEvent(EventID.RequestToPlaySFXWithId, audioID);
        }
    }
}
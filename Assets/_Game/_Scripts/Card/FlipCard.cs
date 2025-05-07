using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardGame
{

    /// <summary>
    /// This class handles the input.
    /// Maintains card state like Revealed, Hidden, Matched.
    /// Handles the animations.
    /// </summary>
    public class FlipCard : BaseCard
    {

        [SerializeField] private Transform _frontFaceParent;
        [SerializeField] private GameObject _questionMark;

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!GlobalVariables.canTakeInput) return;
            if (CurrentState is not CardState.Hidden) return;
            RevealTheCard(() =>
            {
                GlobalEventHandler.TriggerEvent(EventID.RequestToPlaySFXWithId, AudioID.CardFlip);
                GlobalEventHandler.TriggerEvent(EventID.OnCardRevealed, this);
            });
        }


        public override void RevealTheCard(System.Action onRevealed = null)
        {
            _cardState = CardState.Revealed;
            transform.DORotate(Vector3.up * 90, .2f).onComplete += () =>
            {
                _questionMark.SetActive(false);
                _frontFaceParent.gameObject.SetActive(true);
                transform.DORotate(Vector3.zero, .2f).onComplete += () =>
                {
                    onRevealed?.Invoke();
                };
            };
        }
        public override void HideTheCard()
        {
            transform.DORotate(Vector3.up * 90, .2f).onComplete += () =>
              {
                  _questionMark.SetActive(true);
                  _frontFaceParent.gameObject.SetActive(false);
                  transform.DORotate(Vector3.zero, .2f);
                  _cardState = CardState.Hidden;
              };
        }


        public override void OnMatchSuccess()
        {
            _cardState = CardState.Matched;
            DOVirtual.DelayedCall(0.5f, () =>
            {
                _canvasGroup.DOFade(0, .25f);
            });
        }
        public override void OnMatchFail()
        {
            transform.DOShakeRotation(0.2f, Vector3.forward * 10).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad).onComplete += () =>
            {
                HideTheCard();
            };
        }

    }

}
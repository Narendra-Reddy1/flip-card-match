using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardGame
{

    public class FlipCard : BaseCard
    {

        [SerializeField] private Transform _frontFaceParent;

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!GlobalVariables.canTakeInput) return;
            if (CurrentState is not CardState.Hidden) return;
            Flip();
        }
        public override void OnPointerUp(PointerEventData eventData) { }



        public override void ShowFrontFace()
        {
            _cardState = CardState.Revealed;
            transform.DORotate(Vector3.up * 90, .2f).onComplete += () =>
            {
                _frontFaceParent.gameObject.SetActive(true);
                transform.DORotate(Vector3.zero, .2f);
            };
        }
        public override void ShowBackFace()
        {
            transform.DORotate(Vector3.up * 90, .2f).onComplete += () =>
              {
                  _frontFaceParent.gameObject.SetActive(false);
                  transform.DORotate(Vector3.zero, .2f);
                  _cardState = CardState.Hidden;
              };
        }



        public override void Flip()
        {
            ShowFrontFace();
            GlobalEventHandler.TriggerEvent(EventID.RequestToPlaySFXWithId, AudioID.CardFlip);
            GlobalEventHandler.TriggerEvent(EventID.OnCardFlipped, this);
        }
        public override void OnMatchSuccess()
        {
            _cardState = CardState.Matched;
            Debug.Log($">>Card State MATCHED:>>>");
            DOVirtual.DelayedCall(0.5f, () =>
            {
                //Show relted animation...
                _canvasGroup.DOFade(0, .25f);
                Debug.Log("success uid" + UniqueId + " " + _iconId);
            });
        }
        public override void OnMatchFail()
        {
            //Show related animation....
            transform.DOShakeRotation(0.2f, Vector3.forward * 10).SetDelay(0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad).onComplete += () =>
            {
                ShowBackFace();
                Debug.Log("FAIL uid" + UniqueId + " " + _iconId);
            };
        }

    }

}
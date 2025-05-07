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
            //if (!GlobalVariables.canTakeInput) return;
            if (CurrentState is CardState.Revealed || CurrentState is CardState.Matched) return;
            _cardState = CardState.Revealed;
            Flip();
        }
        public override void OnPointerUp(PointerEventData eventData) { }



        public override void ShowFrontFace()
        {
            transform.DORotate(Vector3.up * 90, .2f).onComplete += () =>
            {
                _frontFaceParent.gameObject.SetActive(true);
                transform.DORotate(Vector3.zero, .2f);
                _cardState = CardState.Revealed;
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
            GlobalEventHandler.TriggerEvent(EventID.OnCardFlipped, this);
        }
        public override void OnMatchSuccess()
        {
            DOVirtual.DelayedCall(0.25f, () =>
            {

                //Show relted animation...
                _cardState = CardState.Matched;
                _canvasGroup.DOFade(0, .25f);
                Debug.Log("success uid" + UniqueId + " " + _iconId);
            });
        }
        public override void OnMatchFail()
        {
            //Show related animation....
            DOVirtual.DelayedCall(0.25f, () =>
            {
                transform.DOShakeRotation(0.5f, Vector3.forward * 10).onComplete += () =>
                {
                    ShowBackFace();
                    Debug.Log("FAIL uid" + UniqueId + " " + _iconId);
                };
            });
        }

    }

}
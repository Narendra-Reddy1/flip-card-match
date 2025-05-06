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
        private Vector3 _rightAngle = new Vector3(0, 90, 0);
        public override void Init(int uniqueId, int iconId, Sprite targetIcon, Sprite backIcon)
        {
            this._uniqueId = uniqueId;
            this._iconId = iconId;
            _targetIcon = targetIcon;
            _backFaceIcon = backIcon;
            _targetImg.sprite = _targetIcon;
        }


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
            transform.DORotate(_rightAngle, .2f).onComplete += () =>
            {
                _frontFaceParent.gameObject.SetActive(true);
                transform.DORotate(Vector3.zero, .2f);
                _cardState = CardState.Revealed;
            };
        }
        public override void ShowBackFace()
        {
            transform.DORotate(_rightAngle, .2f).onComplete += () =>
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
            //Show relted animation...
            _cardState = CardState.Matched;
            Debug.Log("success uid" + UniqueId + " " + _iconId);
        }
        public override void OnMatchFail()
        {
            //Show related animation....
            DOVirtual.DelayedCall(.5f, () =>
            {
                ShowBackFace();
            });
            Debug.Log("FAIL uid" + UniqueId + " " + _iconId);
        }

    }

}
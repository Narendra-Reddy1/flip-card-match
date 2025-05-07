using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardGame
{

    public class BaseCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] protected Image _targetImg;
        [SerializeField] protected CanvasGroup _canvasGroup;
        protected int _uniqueId;
        protected int _iconId;
        protected Sprite _targetIcon;
        protected Sprite _backFaceIcon;
        protected CardState _cardState = CardState.Hidden;


        public CardState CurrentState => _cardState;
        public bool IsInteractable => CurrentState == CardState.Hidden;
        public int UniqueId => _uniqueId;
        public int IconId => _iconId;
        public Sprite TargetIcon => _targetIcon;
        public Sprite BackFaceIcon => _backFaceIcon;

        public virtual void Init(int uniqueId, int iconId, Sprite targetIcon, Sprite backIcon)
        {
            this._uniqueId = uniqueId;
            this._iconId = iconId;
            _targetIcon = targetIcon;
            _backFaceIcon = backIcon;
            _targetImg.sprite = _targetIcon;
        }
        public virtual void Init(int uniqueId, int iconId, Sprite targetIcon)
        {
            this._uniqueId = uniqueId;
            this._iconId = iconId;
            _targetIcon = targetIcon;
            _targetImg.sprite = _targetIcon;
        }
        public virtual void Init(int uniqueId, int iconId)
        {
            this._uniqueId = uniqueId;
            this._iconId = iconId;
        }
        public virtual void SetIcons(Sprite targetSprite, Sprite backFaceIcon = null)
        {
            _targetIcon = targetSprite;
            _backFaceIcon = backFaceIcon;
            _targetImg.sprite = _targetIcon;
        }


        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {

        }

        public virtual void Flip()
        {

        }

        public virtual void OnMatchSuccess()
        {

        }
        public virtual void OnMatchFail()
        {

        }
        public virtual void ShowFrontFace()
        {

        }
        public virtual void ShowBackFace() { }
    }

}
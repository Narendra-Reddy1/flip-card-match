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
        protected bool _isFlipped = false;
        protected bool _isInteractable = false;

        public bool IsInteractable => _isInteractable;

        public int UniqueId => _uniqueId;
        public int IconId => _iconId;
        public Sprite TargetIcon => _targetIcon;
        public Sprite BackFaceIcon => _backFaceIcon;

        public virtual void Init(int uniqueId, int iconId, Sprite targetIcon, Sprite backIcon)
        {
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
        public virtual void ShowBackFace()
        {
        }
        public virtual void ToggleInteractability(bool value)
        {
            _isInteractable = value;
        }
    }

}
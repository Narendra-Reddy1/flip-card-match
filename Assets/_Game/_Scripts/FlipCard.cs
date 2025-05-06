using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardGame
{

    public class FlipCard : BaseCard
    {

        public override void Init(int uniqueId, int iconId, Sprite targetIcon, Sprite backIcon)
        {
            this._uniqueId = uniqueId;
            this._iconId = iconId;
            _targetIcon = targetIcon;
            _backFaceIcon = backIcon;
        }


        public override void OnPointerDown(PointerEventData eventData)
        {
            _isSelected = true;
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!_isSelected) return;
            Flip();
        }


        public override void Flip()
        {
            //update image here.
            //trigger event onFlip.
        }

    }

}
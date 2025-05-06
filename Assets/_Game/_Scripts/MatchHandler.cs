using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class MatchHandler : MonoBehaviour
    {
        [SerializeField] private CardsManger _cardsManager;
        private Queue<BaseCard> _flippedCards = new Queue<BaseCard>();

        private void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.OnCardFlipped, Callback_On_Card_Flipped);
        }
        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.OnCardFlipped, Callback_On_Card_Flipped);
        }


        private void Callback_On_Card_Flipped(object args)
        {
            BaseCard flippedCard = args as BaseCard;
            if (_flippedCards.Count == 0)
            {
                _flippedCards.Enqueue(flippedCard);
            }
            else
            {
                var oldCard = _flippedCards.Dequeue();
                if (oldCard.IconId != flippedCard.IconId)
                {
                    //GlobalVariables.canTakeInput = false;
                    oldCard.OnMatchFail();
                    flippedCard.OnMatchFail();
                    GlobalEventHandler.TriggerEvent(EventID.OnCardMatchFailed, (oldCard, flippedCard));
                }
                else
                {
                    oldCard.OnMatchSuccess();
                    flippedCard.OnMatchSuccess();
                    GlobalEventHandler.TriggerEvent(EventID.OnCardMatchSuccess, (oldCard, flippedCard));
                }
            }
        }
    }

}

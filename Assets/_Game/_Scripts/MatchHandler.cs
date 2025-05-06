using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace CardGame
{
    public class MatchHandler : MonoBehaviour
    {
        private List<BaseCard> _flippedCards = new List<BaseCard>();

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
            if (_flippedCards.Count < Konstants.MIN_CARDS_TO_MATCH - 1)
            {
                _flippedCards.Add(flippedCard);
            }
            else
            {
                _flippedCards.Add(flippedCard);
                List<BaseCard> cardsListCopy = new List<BaseCard>(_flippedCards);
                if (_flippedCards.Any(x => x.IconId != flippedCard.IconId))
                {
                    foreach (BaseCard card in _flippedCards)
                        card.OnMatchFail();
                    GlobalEventHandler.TriggerEvent(EventID.OnCardMatchFailed, cardsListCopy);
                }
                else
                {
                    foreach (BaseCard card in _flippedCards)
                        card.OnMatchSuccess();
                    GlobalEventHandler.TriggerEvent(EventID.OnCardMatchSuccess, cardsListCopy);
                }
                _flippedCards.Clear();
            }
        }
    }

}

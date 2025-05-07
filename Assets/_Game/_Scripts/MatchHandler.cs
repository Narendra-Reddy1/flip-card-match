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
                var copyList = new List<BaseCard>(_flippedCards)
                {
                    flippedCard
                };
                _flippedCards.Clear();
                if (copyList.Any(x => x.IconId != flippedCard.IconId))
                {
                    foreach (BaseCard card in copyList)
                        card.OnMatchFail();
                    Debug.Log($"Match Failed for {copyList.Count}");
                    GlobalEventHandler.TriggerEvent(EventID.RequestToPlaySFXWithId, AudioID.MatchFailSFX);
                    GlobalEventHandler.TriggerEvent(EventID.OnCardMatchFailed, copyList);
                }
                else
                {
                    foreach (BaseCard card in copyList)
                        card.OnMatchSuccess();
                    Debug.Log($"Match Found for {copyList.Count}");
                    GlobalEventHandler.TriggerEvent(EventID.RequestToPlaySFXWithId, AudioID.MatchSuccessSFX);
                    GlobalEventHandler.TriggerEvent(EventID.OnCardMatchSuccess, copyList);
                }
            }
        }
    }

}

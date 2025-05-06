using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class GameManager : MonoBehaviour
    {
        //it's better to use DI here instead of manually mapping the dependencies...
        //will see if I have the time later on.
        [SerializeField] private ScoreHandler _scoreHandler;
        [SerializeField] private CardsManager _cardsManager;

        private void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.OnCardMatchSuccess, Callback_On_Match_Success);
        }
        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.OnCardMatchSuccess, Callback_On_Match_Success);
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => PlayerDataManager.IsPlayerDataLoaded());
        }

        private void Callback_On_Match_Success(object args)
        {
            ///WHY??? 
            ///To avoid saving data when Level is completed.....
            DOVirtual.DelayedCall(0.1f, () =>
            {
                PlayerDataManager.instance.SaveLevelData(_scoreHandler.Score, _cardsManager.TotalCards);
            });
        }
    }
}

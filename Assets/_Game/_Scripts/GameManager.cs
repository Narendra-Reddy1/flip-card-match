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
        [SerializeField] private LevelDatabase _levelDatabase;

        private void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.OnCardMatchSuccess, Callback_On_Match_Success);
            GlobalEventHandler.AddListener(EventID.OnLevelStartRequested, Callback_On_Level_Start_Requested);
        }
        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.OnCardMatchSuccess, Callback_On_Match_Success);
            GlobalEventHandler.RemoveListener(EventID.OnLevelStartRequested, Callback_On_Level_Start_Requested);
        }

        private IEnumerator Start()
        {
            ScreenFader.ToggleControlledFadeAnim(true);
            yield return new WaitUntil(PlayerDataManager.IsPlayerDataLoaded);
            ScreenFader.ToggleControlledFadeAnim(false);
        }


        public void StartLevel()
        {
            GlobalVariables.isLevelComplete = false;
            if (PlayerDataManager.instance.HasLevelData())
            {
                //resume level
                GlobalEventHandler.TriggerEvent(EventID.OnLevelResumeRequested, (_levelDatabase.GetLevelSafely(GlobalVariables.highestUnlockedLevelIndex), PlayerDataManager.instance.PlayerData.levelDataModel));
            }
            else
            {
                GlobalEventHandler.TriggerEvent(EventID.OnNewLevelRequested, _levelDatabase.GetLevelSafely(GlobalVariables.highestUnlockedLevelIndex));
                //start new level
            }
        }



        private void Callback_On_Level_Start_Requested(object args)
        {
            StartLevel();
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

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    /// <summary>
    /// This class handles starting the level.
    /// It acts as a middle man between the PlayerDataManager and rest of the components 
    /// required to store persistently.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        //it's better to use DI here instead of manually mapping the dependencies...
        //will see if I have the time later on.
        [SerializeField] private ScoreHandler _scoreHandler;
        [SerializeField] private StreakHandler _streakHandler;
        [SerializeField] private CardsManager _cardsManager;
        [SerializeField] private LevelDatabase _levelDatabase;

        private void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.OnCardMatchSuccess, Callback_On_Match_Success);
            GlobalEventHandler.AddListener(EventID.OnPlayLevelRequested, Callback_On_Level_Start_Requested);
        }
        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.OnCardMatchSuccess, Callback_On_Match_Success);
            GlobalEventHandler.RemoveListener(EventID.OnPlayLevelRequested, Callback_On_Level_Start_Requested);
        }

        private IEnumerator Start()
        {
            ScreenFader.ToggleControlledFadeAnim(true);
            yield return new WaitUntil(PlayerDataManager.IsPlayerDataLoaded);
            GlobalEventHandler.TriggerEvent(EventID.OnPlayerDataLoaded);
            ScreenFader.ToggleControlledFadeAnim(false);
        }


        public void StartLevel()
        {
            GlobalVariables.isLevelComplete = false;
            _scoreHandler.ResetState();
            _streakHandler.ResetState();
            _cardsManager.ResetState();
            if (PlayerDataManager.instance.HasLevelData())
            {
                //resume level
                GlobalEventHandler.TriggerEvent(EventID.OnLevelResumeRequested, (_levelDatabase.GetLevelSafely(GlobalVariables.highestUnlockedLevelIndex), PlayerDataManager.instance.GetLevelData()));
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
            DOVirtual.DelayedCall(0.3f, () =>
            {
                PlayerDataManager.instance.SaveLevelData(_scoreHandler.Score, _streakHandler.HighestStreak, _cardsManager.TotalCards);
            });
        }
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    /// <summary>
    /// It is a centralized class that handles UI related to multiple components.
    /// Switching to Level and Home UI, Showing popups etc.
    /// </summary>
    public class UIManager : MonoBehaviour
    {

        [Header("Home Screen")]
        [SerializeField] private Canvas _homeScreen;
        [SerializeField] private Button _playBtn;
        [SerializeField] private TextMeshProUGUI _homeScreenLevelTxt;

        [Space]
        [Header("Gameplay")]
        [SerializeField] private Canvas _gameplayScreen;
        [SerializeField] private TextMeshProUGUI _inGameLevelTxt;

        [Header("Win Popup")]
        [SerializeField] private WinPopup _winpopup;
        [SerializeField] private ScoreHandler _scoreHandler;
        [SerializeField] private StreakHandler _streakHandler;



        private void OnEnable()
        {
            _playBtn.onClick.AddListener(_OnPlayBtnClicked);
            GlobalEventHandler.AddListener(EventID.OnPlayerDataLoaded, Callback_On_Player_Data_Loaded);
            GlobalEventHandler.AddListener(EventID.OnLevelQuitRequested, Callback_On_HomeScreen_Requested);
            GlobalEventHandler.AddListener(EventID.OnGameStartRequested, Callback_On_Start_Game_Requested);
            GlobalEventHandler.AddListener(EventID.OnLevelComplete, Callback_On_Level_Complete);
        }
        private void OnDisable()
        {
            _playBtn.onClick.RemoveListener(_OnPlayBtnClicked);
            GlobalEventHandler.RemoveListener(EventID.OnPlayerDataLoaded, Callback_On_Player_Data_Loaded);
            GlobalEventHandler.RemoveListener(EventID.OnLevelQuitRequested, Callback_On_HomeScreen_Requested);
            GlobalEventHandler.RemoveListener(EventID.OnGameStartRequested, Callback_On_Start_Game_Requested);
            GlobalEventHandler.RemoveListener(EventID.OnLevelComplete, Callback_On_Level_Complete);
        }

        private void _OnPlayBtnClicked()
        {
            GlobalEventHandler.TriggerEvent(EventID.OnGameStartRequested);
        }
        private void _InitializeHomeScreen()
        {
            _homeScreenLevelTxt.SetText($"Level {GlobalVariables.highestUnlockedLevel}");
        }

        private void _GoToLevel()
        {
            _homeScreen.enabled = false;
            _gameplayScreen.enabled = true;
            _inGameLevelTxt.SetText(GlobalVariables.highestUnlockedLevel.ToString());
        }

        private void _GoToHomeScreen()
        {
            _homeScreen.enabled = true;
            _gameplayScreen.enabled = false;
            _InitializeHomeScreen();
        }

        private void _ShowWinPopup()
        {
            _winpopup.Init(_scoreHandler.Score, _streakHandler.HighestStreak, _StartLevel);
            _winpopup.ShowPopup();
        }
        private void _StartLevel()
        {
            GlobalEventHandler.TriggerEvent(EventID.RequestQuickLoadingScreen);
            DOVirtual.DelayedCall(0.1f, () => ///tiny hack to avoid level flicker while fading....
            {
                _GoToLevel();
                GlobalEventHandler.TriggerEvent(EventID.OnPlayLevelRequested);
            });
        }

        private void Callback_On_Player_Data_Loaded(object args)
        {
            _GoToHomeScreen();
        }
        private void Callback_On_Start_Game_Requested(object args)
        {
            _StartLevel();
        }

        private void Callback_On_Level_Complete(object args)
        {
            _ShowWinPopup();
        }

        private void Callback_On_HomeScreen_Requested(object args)
        {
            _GoToHomeScreen();
        }
    }
}

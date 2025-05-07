using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class UIManager : MonoBehaviour
    {

        [Header("Home Screen")]
        [SerializeField] private Canvas _homeScreen;
        [SerializeField] private Button _playBtn;
        [SerializeField] private TextMeshProUGUI _levelTxt;




        [Space]
        [Header("Gameplay")]
        [SerializeField] private Canvas _gameplayScreen;


        private void OnEnable()
        {
            _playBtn.onClick.AddListener(_OnPlayBtnClicked);
            GlobalEventHandler.AddListener(EventID.OnLevelQuitRequested, Callback_On_HomeScreen_Requested);
        }
        private void OnDisable()
        {
            _playBtn.onClick.RemoveListener(_OnPlayBtnClicked);
            GlobalEventHandler.RemoveListener(EventID.OnLevelQuitRequested, Callback_On_HomeScreen_Requested);
        }


        private void _OnPlayBtnClicked()
        {
            GlobalEventHandler.TriggerEvent(EventID.RequestQuickLoadingScreen);
            DOVirtual.DelayedCall(0.1f, () => ///tiny hack to avoid level flicker while fading....
            {
                _GoToLevel();
                GlobalEventHandler.TriggerEvent(EventID.OnLevelStartRequested);
            });
        }
        private void _InitializeHomeScreen()
        {
            _levelTxt.SetText($"Level {GlobalVariables.highestUnlockedLevelIndex + 1}");
        }

        private void _GoToLevel()
        {
            _homeScreen.enabled = false;
            _gameplayScreen.enabled = true;
        }

        private void _GoToHomeScreen()
        {
            _homeScreen.enabled = true;
            _gameplayScreen.enabled = false;
            _InitializeHomeScreen();
        }



        private void Callback_On_HomeScreen_Requested(object args)
        {
            _GoToHomeScreen();
        }
    }
}

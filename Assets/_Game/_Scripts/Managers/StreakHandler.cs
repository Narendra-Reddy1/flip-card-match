using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class StreakHandler : MonoBehaviour
    {
        [SerializeField] private Image _streakFillbar;
        [SerializeField] private TextMeshProUGUI _streakTxt;
        private byte _streakCounter = 0;
        private byte _highestStreak = 0;


        public byte HighestStreak => _highestStreak;

        private void OnEnable()
        {
            _ResetMatchMultiplier();
            GlobalEventHandler.AddListener(EventID.OnCardMatchSuccess, Callback_On_Match_Success);
            GlobalEventHandler.RemoveListener(EventID.OnLevelResumeRequested, Callback_On_Level_Resume_Requested);
        }
        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.OnCardMatchSuccess, Callback_On_Match_Success);
            GlobalEventHandler.RemoveListener(EventID.OnLevelResumeRequested, Callback_On_Level_Resume_Requested);
        }
        public void ResetState()
        {
            _ResetMatchMultiplier();
            _highestStreak = 0;
        }
        private void _StartFillingDown()
        {
            DOTween.Kill(_streakFillbar);
            float remainingFillbar = _streakFillbar.fillAmount * 100;
            _streakTxt.transform.DOPunchScale(Vector3.one * .2f, .2f, 1).SetEase(Ease.Linear);
            _streakFillbar.DOFillAmount(1, 0.15f).onComplete += () =>
            {
                _streakFillbar.DOFillAmount(0, _GetStreakTimer(_streakCounter)).SetUpdate(true).onComplete += () =>
                {
                    _ResetMatchMultiplier();
                };
            };
            _streakTxt.SetText($"{_streakCounter + 1}X");
        }
        private float _GetStreakTimer(byte streakCounter)
        {
            if (streakCounter <= 5) return Konstants.DEFAULT_SCORE_MULTIPLIER_TIMER_IN_SECONDS;
            if (streakCounter <= 8) return Konstants.DEFAULT_SCORE_MULTIPLIER_TIMER_IN_SECONDS - Konstants.SCORE_MULTIPLIER_DECAY_RATE;
            else
                return Konstants.SCORE_MULTIPLIER_LOWEST_TIMER;
        }

        private void _ResetMatchMultiplier()
        {
            _streakCounter = 0;
            DOTween.Kill(_streakFillbar);
            DOTween.Kill(_streakTxt.transform);
            _streakFillbar.fillAmount = 0;
            _streakTxt.SetText($"X{_streakCounter + 1}");
        }
        private void _OnNewMatchMade()
        {
            _StartFillingDown();
            _streakCounter++;
            if (_streakCounter > _highestStreak)
                _highestStreak = _streakCounter;
        }

        private void Callback_On_Level_Resume_Requested(object args)
        {
            (LevelDataSO leveldata, LevelDataModel savedLevelData) = ((LevelDataSO, LevelDataModel))args;
            _highestStreak = savedLevelData.highestStreak;
        }
        private void Callback_On_Match_Success(object args)
        {
            _OnNewMatchMade();
        }
    }
}

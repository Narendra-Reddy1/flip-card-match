using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CardGame
{
    public class ScoreHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoretxt;

        private int _score;

        public int Score => _score;

        private void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.OnCardMatchSuccess, Callback_On_Match_Success);
            GlobalEventHandler.AddListener(EventID.OnLevelResumeRequested, Callback_On_Level_Resume_Requested);
        }
        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.OnCardMatchSuccess, Callback_On_Match_Success);
            GlobalEventHandler.RemoveListener(EventID.OnLevelResumeRequested, Callback_On_Level_Resume_Requested);
        }

        public void ResetState()
        {
            _score = 0;
            _scoretxt.SetText("0");
        }

        private void Callback_On_Level_Resume_Requested(object args)
        {
            (LevelDataSO leveldata, LevelDataModel savedLevelData) = ((LevelDataSO, LevelDataModel))args;
            _score = savedLevelData.score;
            _scoretxt.SetText(savedLevelData.score.ToString());
        }
        private void Callback_On_Match_Success(object args)
        {

            _score += Konstants.SCORE_PER_MATCH;
            int currentBalance = _score - Konstants.SCORE_PER_MATCH;
            int newBal = _score;
            DOTween.To(() => currentBalance, (x) => currentBalance = x, newBal, .35f).SetDelay(0.05f).onUpdate += () =>
            {
                _scoretxt.SetText(currentBalance.ToString());
            };
        }

    }
}

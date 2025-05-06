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

        private void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.OnCardMatchSuccess, Callback_On_Match_Success);
        }
        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.OnCardMatchSuccess, Callback_On_Match_Success);
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

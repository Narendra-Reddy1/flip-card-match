using System.Collections;
using UnityEngine;

namespace CardGame
{
    public class Konstants
    {
        public const int ICON_REVEAL_TIME_IN_SECONDS = 3;

        public const int SCORE_PER_MATCH = 10;

        public const int MIN_CARDS_TO_MATCH = 2;

        //Streak
        public const float DEFAULT_SCORE_MULTIPLIER_TIMER_IN_SECONDS = 4;
        public const float SCORE_MULTIPLIER_DECAY_RATE = 0.75f;//how much the timer should be reduced.
        public const float SCORE_MULTIPLIER_LOWEST_TIMER = 1.75f;//Lowest time for star multiplier.


        //PlayerPref Keys
        public const string MUSIC_KEY = "music_key";
        public const string SFX_KEY = "sfx_key";


        public const string IS_FIRST_SESSION = "IsFirstSession";
    }
}
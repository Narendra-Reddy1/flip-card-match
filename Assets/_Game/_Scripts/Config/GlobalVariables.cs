using System.Collections;
using UnityEngine;

namespace CardGame
{
    public static class GlobalVariables
    {

        public static bool canTakeInput = true;

        public static bool isLevelComplete = false;

        //Audio
        public static bool IsMusicEnabled;
        public static bool IsSFXEnabled;

        //PLayerData
        public static int highestUnlockedLevelIndex;
        public static int highestUnlockedLevel => highestUnlockedLevelIndex + 1;
    }
}
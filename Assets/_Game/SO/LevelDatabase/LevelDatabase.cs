using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "newLevelDatabase", menuName = "SO/LevelDatabase")]
    public class LevelDatabase : ScriptableObject
    {
        [SerializeField] private List<LevelDataSO> _levels;



        public LevelDataSO GetLevelSafely(int level)
        {
            level = Mathf.Abs(level);
            return _levels[level % _levels.Count];
        }
        public LevelDataSO GetLevel(int level)
        {
            if (level < 0 || level > _levels.Count) return null;
            return _levels[level];
        }

    }
}

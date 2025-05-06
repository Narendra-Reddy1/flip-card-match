using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "newLevelData", menuName = "SO/Levels/New Level")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private Vector2Int _gridSize;

        [SerializeField] private int _uniqueSetsToSpawn;


        public int UniqueSets => _uniqueSetsToSpawn;
        public int MaxUniquSets => (_gridSize.x * _gridSize.y) / 2;
        public Vector2Int GridSize => _gridSize;




        private void OnValidate()
        {
            int totalCount = _gridSize.x * _gridSize.y;
            if (totalCount % 2 != 0)
            {
                Debug.LogError($"Total Item Count should not be a ODD number. Give appropriate grid size");
            }
            if (_uniqueSetsToSpawn > (totalCount) / 2)
            {
                Debug.LogError($"Unique Set count should not be more than half of the totalCount. Resetting it..Max {totalCount / 2}");
                _uniqueSetsToSpawn = totalCount / 2;
            }
        }

    }
}

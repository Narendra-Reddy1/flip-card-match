using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "newLevelData", menuName = "SO/Levels/New Level")]
    public class LevelDataSO : ScriptableObject
    {
        [SerializeField] private Vector2Int _gridSize;

        [SerializeField] private int _uniqueSetsToSpawn;


        public int UniqueSets => _uniqueSetsToSpawn;
        public int MaxUniquSets => (_gridSize.x * _gridSize.y) / Konstants.MIN_CARDS_TO_MATCH;
        public Vector2Int GridSize => _gridSize;




        private void OnValidate()
        {
            int totalCount = _gridSize.x * _gridSize.y;
            if (totalCount % Konstants.MIN_CARDS_TO_MATCH != 0)
            {
                Debug.LogError($"Total Item Count should not be a ODD number. Give appropriate grid size");
                Debug.ClearDeveloperConsole();
                Debug.Break();
            }
            if (_uniqueSetsToSpawn > (totalCount) / Konstants.MIN_CARDS_TO_MATCH)
            {
                Debug.LogError($"Unique Set count should not be more than half of the totalCount. Resetting it..Max {totalCount / Konstants.MIN_CARDS_TO_MATCH}");
                _uniqueSetsToSpawn = totalCount / Konstants.MIN_CARDS_TO_MATCH;
                Debug.ClearDeveloperConsole();
                Debug.Break();
            }
        }

    }
}

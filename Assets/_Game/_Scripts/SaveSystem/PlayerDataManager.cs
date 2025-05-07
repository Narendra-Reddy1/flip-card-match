using System.Collections.Generic;
using UnityEngine;
using System;
namespace CardGame
{
    public partial class PlayerDataManager : MonoBehaviour
    {

        #region SINGLETON
        public static PlayerDataManager instance { get; private set; }

        #endregion

        #region Variables

        private PlayerData _playerData;
        public PlayerData PlayerData => _playerData;

        private static bool _isPlayerDataLoaded = false;
        //public string levelKey { get; private set; }

        public event Func<bool> OnDataInitialized;


        private const string IS_FIRST_SESSION = "IsFirstSession";
        #endregion

        #region Unity Built-In Methods

        private void Awake()
        {
            if (instance != this && instance != null)
            {
                Destroy(gameObject);
            }
            else if (instance == null)
            {
                instance = this;
            }
        }

        public void Start()
        {
            _Init();
        }

        private void OnEnable()
        {
            OnDataInitialized += IsPlayerDataLoaded;
        }
        private void OnDisable()
        {
            OnDataInitialized -= IsPlayerDataLoaded;
        }

        #endregion

        #region Custom Methods
        public static bool IsPlayerDataLoaded()
        {
            return _isPlayerDataLoaded;
        }

        /// <summary>
        /// This method is responsible for saving the player data.
        /// </summary>
        public void SaveData()
        {
            _playerData.levelIndex = GlobalVariables.highestUnlockedLevelIndex;
            DataSerializer.Save("playerData.dat", _playerData);
            Debug.Log("Done with Saving....");
        }

        /// <summary>
        /// This method is responsible for loading the player data.
        /// </summary>
        public void LoadData()
        {
            _playerData = DataSerializer.Load<PlayerData>("playerData.dat");

            if (_playerData == null)
            {
                Debug.Log(" Error with Loading..");
                _playerData = new PlayerData();
                _AddFirstStartData();
                SaveData();
            }
            GlobalVariables.highestUnlockedLevelIndex = _playerData.levelIndex;
            Debug.Log("Done with Loading");
        }

        private void _Init()
        {
            if (!PlayerPrefs.HasKey(IS_FIRST_SESSION))
            {
                //add default data....
                _AddFirstStartData();
            }
            else
            {
                //Load Data
                LoadData();
            }
            _isPlayerDataLoaded = true;
            OnDataInitialized?.Invoke();
            //Save Data here
            SaveData();
        }

        private void _AddFirstStartData()
        {
            PlayerPrefs.SetInt(IS_FIRST_SESSION, 1);
            _playerData = new PlayerData();
            //Add Default Data here.....
        }

        #endregion
    }

    /// <summary>
    /// This is persistant class to store the player data.
    /// </summary>
    [Serializable]
    public partial class PlayerData
    {
        public int levelIndex;
        public ILevelDataModel levelDataModel;
        public PlayerData()
        {
            levelIndex = 0;
            levelDataModel = null;
        }
    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using System.IO;

namespace Ribbons.Storage
{
    public class StorageManager
    {
        PlayerData[] playerDataArray;

        // State variable
        public PlayerData CurrentPlayerData;

        public StorageManager() { throw new NotImplementedException(); }

        /// <summary>
        /// Loads player data.
        /// </summary>
        /// <param name="player">The player number.</param>
        /// <returns>A copy of the loaded player data.</returns>
        public void LoadPlayer(int player)
        {
            CurrentPlayerData = playerDataArray[player];
            CurrentPlayerData.ID = player;
        }
        /// <summary>
        /// Saves the active player data. Temporary values are reset.
        /// </summary>
        public void SavePlayer()
        {
            CurrentPlayerData.ResetLevelSpecificVariables();
            playerDataArray[CurrentPlayerData.ID] = CurrentPlayerData;
        }
    }

    

    /// <summary>
    /// Per-player saved data.
    /// </summary>
    public class PlayerData
    {
        public int ID;
        LevelProgressData[] levelProgressArray;

        // State variable
        public LevelProgressData CurrentLevelData;

        public void ResetLevelSpecificVariables()
        {
            for (int i = 0; i < levelProgressArray.Length; i++)
                levelProgressArray[i].ResetLevelSpecificVariables();
        }

        public void LoadLevelProgress(int level)
        {
            CurrentLevelData = levelProgressArray[level];
            CurrentLevelData.ID = level;
        }
        /// <summary>
        /// Saved level data. Temporary values are reset.
        /// </summary>
        /// <param name="player">The level number.</param>
        /// <param name="playerData">The level data.</param>
        public void SaveLevelProgress()
        {
            CurrentLevelData.ResetLevelSpecificVariables();
            levelProgressArray[CurrentLevelData.ID] = CurrentLevelData;
        }
    }

    /// <summary>
    /// Per-level saved data.
    /// </summary>
    public class LevelProgressData
    {
        public int ID;

        // Variables persisting between levels
        public bool[] BowsCollected;
        public bool Completed;

        // Variables persisting between runs
        public int LatestCheckpointReached;

        public void ResetLevelSpecificVariables()
        {
            LatestCheckpointReached = 0;
        }

        // Variables persisting only within a run
        public bool[] GemsActivated;

        public void ResetRunSpecificVariables()
        {
            for (int i = 0; i < GemsActivated.Length; i++)
                GemsActivated[i] = false;
        }
    }
}

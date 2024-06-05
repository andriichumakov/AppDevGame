using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;

namespace AppDevGame
{
    public class SaveManager
    {
        private static string saveDirectory = "saves";
        private static int maxSaveSlots = 3;

        static SaveManager()
        {
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }
        }

        public static void SaveGame(GameState gameState, int slot)
        {
            try
            {
                string saveFilePath = Path.Combine(saveDirectory, $"savegame_{slot}.json");
                string jsonString = JsonSerializer.Serialize(gameState, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(saveFilePath, jsonString);
                MainApp.Log("Game saved successfully.");
            }
            catch (Exception ex)
            {
                MainApp.Log("Failed to save game: " + ex.Message);
            }
        }

        public static GameState LoadGame(int slot)
        {
            try
            {
                string saveFilePath = Path.Combine(saveDirectory, $"savegame_{slot}.json");
                if (File.Exists(saveFilePath))
                {
                    string jsonString = File.ReadAllText(saveFilePath);
                    GameState gameState = JsonSerializer.Deserialize<GameState>(jsonString);
                    MainApp.Log("Game loaded successfully.");
                    return gameState;
                }
                else
                {
                    MainApp.Log("Save file not found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MainApp.Log("Failed to load game: " + ex.Message);
                return null;
            }
        }

        public static int MaxSaveSlots => maxSaveSlots;

        public static bool SaveSlotIsEmpty(int slot)
        {
            string saveFilePath = Path.Combine(saveDirectory, $"savegame_{slot}.json");
            return !File.Exists(saveFilePath);
        }
    }

    public class GameState
    {
        public Vector2 playerPosition { get; set; }
        public int playerHealth { get; set; }
        public int coinsCollected { get; set; }
        public string currentLevel { get; set; }
        public List<EntityState> entities { get; set; }
    }

    public class EntityState
    {
        public string EntityType { get; set; }
        public Vector2 Position { get; set; }
        public int Health { get; set; }
    }
}
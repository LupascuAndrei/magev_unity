using System.IO;
using UnityEngine;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;

namespace Assets.Scripts
{
    /// <summary>
    /// Handles character customization persistence
    /// </summary>
    public class CharacterCustomizationManager : MonoBehaviour
    {
        private const string SaveFileName = "character_data.json";
        private string SaveFilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

        /// <summary>
        /// Saves character data to file
        /// </summary>
        public void SaveCharacter(Character character)
        {
            if (character == null)
            {
                Debug.LogError("Cannot save: Character is null");
                return;
            }

            try
            {
                string json = character.ToJson();
                File.WriteAllText(SaveFilePath, json);
                Debug.Log($"Character saved to: {SaveFilePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save character: {e.Message}");
            }
        }

        /// <summary>
        /// Loads character data from file
        /// </summary>
        public void LoadCharacter(Character character)
        {
            if (character == null)
            {
                Debug.LogError("Cannot load: Character is null");
                return;
            }

            if (!File.Exists(SaveFilePath))
            {
                Debug.Log("No saved character data found. Using default character.");
                return;
            }

            try
            {
                string json = File.ReadAllText(SaveFilePath);
                character.FromJson(json);
                Debug.Log($"Character loaded from: {SaveFilePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load character: {e.Message}");
            }
        }

        /// <summary>
        /// Checks if save file exists
        /// </summary>
        public bool HasSavedData()
        {
            return File.Exists(SaveFilePath);
        }

        /// <summary>
        /// Deletes saved character data
        /// </summary>
        public void DeleteSavedData()
        {
            if (File.Exists(SaveFilePath))
            {
                File.Delete(SaveFilePath);
                Debug.Log("Character data deleted");
            }
        }
    }
}


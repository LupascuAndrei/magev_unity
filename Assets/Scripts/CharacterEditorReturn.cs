using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using Assets.HeroEditor.Common.Scripts.EditorScripts;

namespace Assets.Scripts
{
    /// <summary>
    /// Handles returning to DebugScene from CharacterEditor scene
    /// </summary>
    public class CharacterEditorReturn : MonoBehaviour
    {
        public Character Character;
        public static string ReturnSceneName = "DebugScene";

        public void Start()
        {
            // Load character data if it was saved
            if (CharacterEditor.CharacterJson != null && Character != null)
            {
                Character.FromJson(CharacterEditor.CharacterJson);
            }
        }

        public void Update()
        {
            // Return to DebugScene on ESC key
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ReturnToDebugScene();
            }
        }

        /// <summary>
        /// Called by Back button to return to DebugScene
        /// </summary>
        public void ReturnToDebugScene()
        {
            #if UNITY_EDITOR
            if (UnityEditor.EditorBuildSettings.scenes.All(i => !i.path.Contains(ReturnSceneName)))
            {
                Debug.LogWarning($"Please add '{ReturnSceneName}.unity' to Build Settings!");
                return;
            }
            #endif

            if (!string.IsNullOrEmpty(ReturnSceneName))
            {
                SceneManager.LoadScene(ReturnSceneName);
            }
            else
            {
                Debug.LogWarning("ReturnSceneName is not set!");
            }
        }
    }
}


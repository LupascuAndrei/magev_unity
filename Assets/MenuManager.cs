using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using Assets.HeroEditor.Common.Scripts.EditorScripts;
using System.Linq;

namespace Assets.Scripts
{
    /// <summary>
    /// Manages game menu with character customization
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        [Header("References")]
        public GameObject MenuPanel;
        public GameObject CharacterEditorPanel;
        public Character PlayerCharacter;
        
        [Header("Buttons")]
        public Button EditPlayerButton;
        public Button ResumeButton;
        public Button SaveButton;
        public Button CloseEditorButton;
        
        private bool _isMenuOpen = false;
        private bool _isEditorOpen = false;
        private CharacterCustomizationManager _customizationManager;

        private void Start()
        {
            _customizationManager = GetComponent<CharacterCustomizationManager>();
            
            if (MenuPanel != null) MenuPanel.SetActive(false);
            if (CharacterEditorPanel != null) CharacterEditorPanel.SetActive(false);
            
            // Setup button listeners
            if (EditPlayerButton != null)
                EditPlayerButton.onClick.AddListener(OnEditPlayer);
            
            if (ResumeButton != null)
                ResumeButton.onClick.AddListener(OnResume);
            
            if (SaveButton != null)
                SaveButton.onClick.AddListener(OnSaveCharacter);
            
            if (CloseEditorButton != null)
                CloseEditorButton.onClick.AddListener(OnCloseEditor);
            
            // Load saved character
            if (_customizationManager != null && PlayerCharacter != null)
            {
                _customizationManager.LoadCharacter(PlayerCharacter);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isEditorOpen)
                {
                    OnCloseEditor();
                }
                else if (_isMenuOpen)
                {
                    OnResume();
                }
                else
                {
                    OpenMenu();
                }
            }
        }

        private void OpenMenu()
        {
            _isMenuOpen = true;
            Debug.Log("Menu opened! Press ESC to close.");
            
            if (MenuPanel != null)
            {
                MenuPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("MenuPanel is not set! Pausing game anyway.");
            }
            
            Time.timeScale = 0f;
        }

        private void OnResume()
        {
            _isMenuOpen = false;
            Debug.Log("Menu closed! Game resumed.");
            
            if (MenuPanel != null)
            {
                MenuPanel.SetActive(false);
            }
            
            Time.timeScale = 1f;
        }

        private void OnEditPlayer()
        {
            // Save current scene name for return
            CharacterEditorReturn.ReturnSceneName = SceneManager.GetActiveScene().name;
            
            // Save character data to JSON so it can be loaded in CharacterEditor scene
            if (PlayerCharacter != null)
            {
                CharacterEditor.CharacterJson = PlayerCharacter.ToJson();
            }
            
            // Check if CharacterEditor scene is in Build Settings
            #if UNITY_EDITOR
            if (!UnityEditor.EditorBuildSettings.scenes.Any(i => i.path.Contains("CharacterEditor") && i.enabled))
            {
                UnityEditor.EditorUtility.DisplayDialog("Scene Not Found", 
                    "Please add 'CharacterEditor.unity' to Build Settings!\n\n" +
                    "File -> Build Settings -> Add Open Scenes", "OK");
                return;
            }
            #endif
            
            // Load CharacterEditor scene
            SceneManager.LoadScene("CharacterEditor");
        }

        private void OnCloseEditor()
        {
            // This method is no longer used since we're loading a separate scene
            // But keeping it for compatibility in case CharacterEditorPanel is still referenced
            _isEditorOpen = false;
            if (CharacterEditorPanel != null) CharacterEditorPanel.SetActive(false);
            if (MenuPanel != null) MenuPanel.SetActive(true);
            Time.timeScale = 1f;
        }

        private void OnSaveCharacter()
        {
            if (_customizationManager != null && PlayerCharacter != null)
            {
                _customizationManager.SaveCharacter(PlayerCharacter);
                Debug.Log("Character saved successfully!");
            }
        }

        private void OnApplicationQuit()
        {
            // Auto-save on quit
            if (_customizationManager != null && PlayerCharacter != null)
            {
                _customizationManager.SaveCharacter(PlayerCharacter);
            }
        }
    }
}


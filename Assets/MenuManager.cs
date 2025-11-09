using UnityEngine;
using UnityEngine.UI;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using Assets.HeroEditor.Common.Scripts.EditorScripts;

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
        public CharacterEditor CharacterEditor;
        public GameObject CharacterEditorHuman; // Human GameObject inside CharacterEditor
        
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
            
            // Disable Human GameObject at start
            if (CharacterEditorHuman != null)
            {
                CharacterEditorHuman.SetActive(false);
            }
            
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
            
            // Connect CharacterEditor to PlayerCharacter when it becomes active
            if (CharacterEditor != null && PlayerCharacter != null)
            {
                CharacterEditor.Character = PlayerCharacter;
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
            // Activate CharacterEditor panel in current scene instead of loading a new scene
            _isEditorOpen = true;
            
            // Hide menu panel
            if (MenuPanel != null)
            {
                MenuPanel.SetActive(false);
            }
            
            // Activate CharacterEditor panel
            if (CharacterEditorPanel != null)
            {
                CharacterEditorPanel.SetActive(true);
            }
            
            // Enable Human GameObject
            if (CharacterEditorHuman != null)
            {
                CharacterEditorHuman.SetActive(true);
            }
            
            // Connect CharacterEditor to PlayerCharacter
            if (CharacterEditor != null && PlayerCharacter != null)
            {
                CharacterEditor.Character = PlayerCharacter;
            }
            
            // Pause game while editing
            Time.timeScale = 0f;
        }

        private void OnCloseEditor()
        {
            _isEditorOpen = false;
            
            // Disable Human GameObject
            if (CharacterEditorHuman != null)
            {
                CharacterEditorHuman.SetActive(false);
            }
            
            // Hide CharacterEditor panel
            if (CharacterEditorPanel != null)
            {
                CharacterEditorPanel.SetActive(false);
            }
            
            // Show menu panel
            if (MenuPanel != null)
            {
                MenuPanel.SetActive(true);
            }
            
            // Resume game
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


using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using Assets.HeroEditor.Common.Scripts.ExampleScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor.Common.Scripts.EditorScripts
{
    /// <summary>
    /// A helper used in character editor scenes.
    /// </summary>
    public class CharacterReferencesResolver : MonoBehaviour
    {
        public CharacterEditor CharacterEditor;
        public AnimationManager AnimationManager;
        public AttackingExample AttackingExample;
        public BowExample BowExample;

        public Slider WidthSlider;
        public Slider HeightSlider;
        public Button WidthReset;
        public Button HeightReset;

        public void OnValidate()
        {
            var character = FindFirstObjectByType<Character>();

            CharacterEditor.Character = character;
            AnimationManager.Character = character;
            AttackingExample.Character = character;
            BowExample.Character = character;
        }

        public void Awake()
        {
            ConnectCharacterReferences();
        }

        public void OnEnable()
        {
            // Reconnect when GameObject becomes active
            ConnectCharacterReferences();
        }

        private void ConnectCharacterReferences()
        {
            // Ensure Character is set at runtime (OnValidate only runs in editor)
            if (CharacterEditor == null)
            {
                Debug.LogError("CharacterEditor reference is not set!");
                return;
            }

            if (CharacterEditor.Character == null)
            {
                Character character = null;
                
                // First, try to find a Character on a GameObject named "Player"
                var playerObject = GameObject.Find("Player");
                if (playerObject != null)
                {
                    // Try to get Character component from Player or its children
                    character = playerObject.GetComponent<Character>();
                    if (character == null)
                    {
                        character = playerObject.GetComponentInChildren<Character>();
                    }
                }
                
                // If not found, try to find any Character in the scene
                if (character == null)
                {
                    character = FindFirstObjectByType<Character>();
                }
                
                if (character != null)
                {
                    CharacterEditor.Character = character;
                    if (AnimationManager != null) AnimationManager.Character = character;
                    if (AttackingExample != null) AttackingExample.Character = character;
                    if (BowExample != null) BowExample.Character = character;
                }
            }

            if (CharacterEditor.Character == null)
            {
                Debug.LogWarning("Character not found! Make sure there is a Character component in the scene.");
                return;
            }

            var sculptor = CharacterEditor.Character.GetComponent<CharacterBodySculptor>();
            if (sculptor == null)
            {
                Debug.LogWarning("CharacterBodySculptor component not found on Character!");
                return;
            }

            if (WidthSlider != null)
            {
                sculptor.WidthSlider = WidthSlider;
                WidthSlider.onValueChanged.RemoveAllListeners();
                WidthSlider.onValueChanged.AddListener(sculptor.OnWidthChanged);
            }

            if (HeightSlider != null)
            {
                sculptor.HeightSlider = HeightSlider;
                HeightSlider.onValueChanged.RemoveAllListeners();
                HeightSlider.onValueChanged.AddListener(sculptor.OnHeightChanged);
            }

            if (WidthReset != null)
            {
                WidthReset.onClick.RemoveAllListeners();
                WidthReset.onClick.AddListener(sculptor.ResetWidth);
            }

            if (HeightReset != null)
            {
                HeightReset.onClick.RemoveAllListeners();
                HeightReset.onClick.AddListener(sculptor.ResetHeight);
            }
        }
    }
}
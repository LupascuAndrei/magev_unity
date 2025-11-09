using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using Assets.HeroEditor.Common.Scripts.Collections;
using HeroEditor.Common;
using HeroEditor.Common.Data;
using HeroEditor.Common.Enums;
using BodyPart = HeroEditor.Common.Enums.BodyPart;

namespace Assets.Scripts
{
    /// <summary>
    /// Simple character editor for runtime customization
    /// </summary>
    public class SimpleCharacterEditor : MonoBehaviour
    {
        [Header("Character")]
        public Character Character;

        [Header("UI Buttons")]
        public Button NextWeaponButton;
        public Button PreviousWeaponButton;
        public Button NextArmorButton;
        public Button PreviousArmorButton;
        public Button NextHairButton;
        public Button PreviousHairButton;
        public Button RandomizeButton;

        [Header("UI Text")]
        public Text WeaponText;
        public Text ArmorText;
        public Text HairText;

        private int _currentWeaponIndex = 0;
        private int _currentArmorIndex = 0;
        private int _currentHairIndex = 0;

        private void Start()
        {
            if (Character == null)
            {
                Debug.LogError("Character not set!");
                return;
            }
            
            if (Character.SpriteCollection == null)
            {
                Debug.LogError("Character SpriteCollection not set!");
                return;
            }

            // Setup button listeners
            if (NextWeaponButton != null)
                NextWeaponButton.onClick.AddListener(() => ChangeWeapon(1));
            
            if (PreviousWeaponButton != null)
                PreviousWeaponButton.onClick.AddListener(() => ChangeWeapon(-1));
            
            if (NextArmorButton != null)
                NextArmorButton.onClick.AddListener(() => ChangeArmor(1));
            
            if (PreviousArmorButton != null)
                PreviousArmorButton.onClick.AddListener(() => ChangeArmor(-1));
            
            if (NextHairButton != null)
                NextHairButton.onClick.AddListener(() => ChangeHair(1));
            
            if (PreviousHairButton != null)
                PreviousHairButton.onClick.AddListener(() => ChangeHair(-1));
            
            if (RandomizeButton != null)
                RandomizeButton.onClick.AddListener(RandomizeCharacter);

            UpdateUI();
        }

        private void ChangeWeapon(int direction)
        {
            if (Character.SpriteCollection.MeleeWeapon1H.Count == 0) return;

            _currentWeaponIndex = (_currentWeaponIndex + direction + Character.SpriteCollection.MeleeWeapon1H.Count) 
                                  % Character.SpriteCollection.MeleeWeapon1H.Count;
            
            var weapon = Character.SpriteCollection.MeleeWeapon1H[_currentWeaponIndex];
            Character.Equip(weapon, EquipmentPart.MeleeWeapon1H);
            
            UpdateUI();
        }

        private void ChangeArmor(int direction)
        {
            if (Character.SpriteCollection.Armor.Count == 0) return;

            _currentArmorIndex = (_currentArmorIndex + direction + Character.SpriteCollection.Armor.Count) 
                                 % Character.SpriteCollection.Armor.Count;
            
            var armor = Character.SpriteCollection.Armor[_currentArmorIndex];
            Character.Equip(armor, EquipmentPart.Armor);
            
            UpdateUI();
        }

        private void ChangeHair(int direction)
        {
            if (Character.SpriteCollection.Hair.Count == 0) return;

            _currentHairIndex = (_currentHairIndex + direction + Character.SpriteCollection.Hair.Count) 
                                % Character.SpriteCollection.Hair.Count;
            
            var hair = Character.SpriteCollection.Hair[_currentHairIndex];
            Character.SetBody(hair, BodyPart.Hair);
            
            UpdateUI();
        }

        private void RandomizeCharacter()
        {
            // Randomize weapon
            if (Character.SpriteCollection.MeleeWeapon1H.Count > 0)
            {
                var randomWeapon = Character.SpriteCollection.MeleeWeapon1H[Random.Range(0, Character.SpriteCollection.MeleeWeapon1H.Count)];
                Character.Equip(randomWeapon, EquipmentPart.MeleeWeapon1H);
            }

            // Randomize armor
            if (Character.SpriteCollection.Armor.Count > 0)
            {
                var randomArmor = Character.SpriteCollection.Armor[Random.Range(0, Character.SpriteCollection.Armor.Count)];
                Character.Equip(randomArmor, EquipmentPart.Armor);
            }

            // Randomize hair
            if (Character.SpriteCollection.Hair.Count > 0)
            {
                var randomHair = Character.SpriteCollection.Hair[Random.Range(0, Character.SpriteCollection.Hair.Count)];
                Character.SetBody(randomHair, BodyPart.Hair);
            }

            // Randomize helmet
            if (Character.SpriteCollection.Helmet.Count > 0 && Random.value > 0.5f)
            {
                var randomHelmet = Character.SpriteCollection.Helmet[Random.Range(0, Character.SpriteCollection.Helmet.Count)];
                Character.Equip(randomHelmet, EquipmentPart.Helmet);
            }
            else
            {
                Character.UnEquip(EquipmentPart.Helmet);
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            if (WeaponText != null && Character.SpriteCollection.MeleeWeapon1H.Count > 0)
            {
                var weapon = Character.SpriteCollection.MeleeWeapon1H[_currentWeaponIndex];
                WeaponText.text = $"Weapon: {weapon.Name} ({_currentWeaponIndex + 1}/{Character.SpriteCollection.MeleeWeapon1H.Count})";
            }

            if (ArmorText != null && Character.SpriteCollection.Armor.Count > 0)
            {
                var armor = Character.SpriteCollection.Armor[_currentArmorIndex];
                ArmorText.text = $"Armor: {armor.Name} ({_currentArmorIndex + 1}/{Character.SpriteCollection.Armor.Count})";
            }

            if (HairText != null && Character.SpriteCollection.Hair.Count > 0)
            {
                var hair = Character.SpriteCollection.Hair[_currentHairIndex];
                HairText.text = $"Hair: {hair.Name} ({_currentHairIndex + 1}/{Character.SpriteCollection.Hair.Count})";
            }
        }

        public void EquipRandomWeapon()
        {
            if (Character.SpriteCollection.MeleeWeapon1H.Count > 0)
            {
                var weapon = Character.SpriteCollection.MeleeWeapon1H[Random.Range(0, Character.SpriteCollection.MeleeWeapon1H.Count)];
                Character.Equip(weapon, EquipmentPart.MeleeWeapon1H);
            }
        }
    }
}


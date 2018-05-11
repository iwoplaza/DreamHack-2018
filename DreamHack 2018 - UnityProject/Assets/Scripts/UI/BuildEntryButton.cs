using Game.Building;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(Button))]
    public class BuildEntryButton : MonoBehaviour
    {
        [SerializeField] Image m_iconImage;
        [SerializeField] Text m_titleText;
        [SerializeField] ColorBlock m_selectedColors;

        public BuildCataloguePanel ParentPanel { get; private set; }
        public BuildEntry BuildEntry { get; private set; }
        public bool Selected { get; private set; }

        Button m_button;
        ColorBlock m_defaultColors;

        public void Awake()
        {
            m_button = GetComponent<Button>();
            m_defaultColors = m_button.colors;
        }

        public void Setup(BuildCataloguePanel panel, BuildEntry entry)
        {
            ParentPanel = panel;
            BuildEntry = entry;

            m_titleText.text = entry.DisplayName;
        }

        public void OnPressed()
        {
            ParentPanel.ChooseEntry(BuildEntry);
        }

        public void Select()
        {
            m_button.colors = m_selectedColors;
            m_button.interactable = false;
            m_titleText.color = Color.white;
        }

        public void Deselect()
        {
            m_button.colors = m_defaultColors;
            m_button.interactable = true;
            m_titleText.color = Color.black;
        }
    }
}
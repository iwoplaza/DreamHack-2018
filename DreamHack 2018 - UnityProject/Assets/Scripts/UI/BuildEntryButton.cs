using Game.Building;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BuildEntryButton : MonoBehaviour
    {
        [SerializeField] Image m_iconImage;
        [SerializeField] Text m_titleText;

        public BuildCataloguePanel ParentPanel { get; private set; }
        public BuildEntry BuildEntry { get; private set; }

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
    }
}
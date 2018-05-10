using Game.Building;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BuildCategoryButton : MonoBehaviour
    {
        [SerializeField] Image m_iconImage;

        public BuildCataloguePanel ParentPanel { get; private set; }
        public BuildCategory BuildCategory { get; private set; }

        public void Setup(BuildCataloguePanel panel, BuildCategory category)
        {
            ParentPanel = panel;
            BuildCategory = category;
            Sprite sprite = Resources.FindIcon(category.IconName);
            m_iconImage.sprite = sprite;
        }

        public void OnPressed()
        {
            ParentPanel.ChooseCategory(BuildCategory);
        }
    }
}
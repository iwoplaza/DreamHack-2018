using Game.Building;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BuildCataloguePanel : MonoBehaviour
    {
        [SerializeField] Transform m_categoriesHolder;
        [SerializeField] Transform m_entriesHolder;
        [SerializeField] Text m_pageTitleText;
        [SerializeField] Button m_backButton;
        [Header("Prefabs")]
        [SerializeField] GameObject m_categoryButtonPrefab;
        [SerializeField] GameObject m_entryButtonPrefab;

        List<BuildCategoryButton> m_categoryButtonObjects;
        List<BuildEntryButton> m_entryButtonObjects;

        public CataloguePage CurrentPage { get; private set; }
        public BuildCategory CurrentCategory { get; private set; }

        // Use this for initialization
        void Start()
        {
            CurrentPage = CataloguePage.NONE;
            gameObject.SetActive(false);
            WorldController.Instance.RegisterModeChangeHandler(OnModeChange);

            m_categoryButtonObjects = new List<BuildCategoryButton>();
            m_entryButtonObjects = new List<BuildEntryButton>();
        }

        void Show()
        {
            BuildCatalogue buildCatalogue = WorldController.Instance.MainState.BuildCatalogue;
            if (buildCatalogue == null)
                return;

            foreach (BuildCategoryButton button in m_categoryButtonObjects)
            {
                Destroy(button.gameObject);
            }
            m_categoryButtonObjects.Clear();

            foreach (BuildCategory category in buildCatalogue.Categories)
            {
                GameObject categoryButtonObject = Instantiate(m_categoryButtonPrefab, m_categoriesHolder);
                BuildCategoryButton button = categoryButtonObject.GetComponent<BuildCategoryButton>();
                button.Setup(this, category);
                m_categoryButtonObjects.Add(button);
            }

            gameObject.SetActive(true);
            ShowPage(CataloguePage.CATEGORIES);
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ChooseCategory(BuildCategory category)
        {
            CurrentCategory = category;
            ShowPage(CataloguePage.ENTRIES);
        }

        public void ChooseEntry(BuildEntry entry)
        {
            BuildModeManager buildModeManager = WorldController.Instance.MainState.BuildModeManager;
            if (buildModeManager != null)
            {
                buildModeManager.Hold(entry.PropType, entry.PropVariation);
            }
        }

        public void GoBack()
        {
            if(CurrentPage == CataloguePage.ENTRIES)
            {
                ShowPage(CataloguePage.CATEGORIES);
            }
        }

        public void ShowPage(CataloguePage page)
        {
            if (page != CurrentPage)
            {
                CurrentPage = page;
                switch (CurrentPage)
                {
                    case CataloguePage.CATEGORIES:
                        m_categoriesHolder.gameObject.SetActive(true);
                        m_entriesHolder.gameObject.SetActive(false);
                        m_pageTitleText.text = "Categories";
                        m_backButton.interactable = false;
                        break;
                    case CataloguePage.ENTRIES:
                        m_categoriesHolder.gameObject.SetActive(false);
                        m_entriesHolder.gameObject.SetActive(true);
                        m_pageTitleText.text = CurrentCategory.DisplayName;
                        m_backButton.interactable = true;

                        foreach (BuildEntryButton button in m_entryButtonObjects)
                        {
                            Destroy(button.gameObject);
                        }
                        m_entryButtonObjects.Clear();

                        foreach (BuildEntry entry in CurrentCategory.Entries)
                        {
                            GameObject entryButtonObject = Instantiate(m_entryButtonPrefab, m_entriesHolder);
                            BuildEntryButton button = entryButtonObject.GetComponent<BuildEntryButton>();
                            button.Setup(this, entry);
                            m_entryButtonObjects.Add(button);
                        }

                        break;
                    default:
                        Hide();
                        break;
                }
            }
        }

        void OnModeChange(PlayMode newMode)
        {
            if(newMode == PlayMode.BUILD_MODE)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public enum CataloguePage
        {
            NONE,
            CATEGORIES,
            ENTRIES
        }
    }
}
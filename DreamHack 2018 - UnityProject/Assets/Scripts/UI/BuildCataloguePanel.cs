using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class BuildCataloguePanel : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            gameObject.SetActive(false);
            WorldController.Instance.RegisterModeChangeHandler(OnModeChange);
        }

        void Show()
        {
            gameObject.SetActive(true);
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

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
    }
}
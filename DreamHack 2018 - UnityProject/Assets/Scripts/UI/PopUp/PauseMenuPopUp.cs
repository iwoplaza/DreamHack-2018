using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.PopUp
{
    public class PauseMenuPopUp : PopUpWindow
    {
        public override bool IsSingluar { get { return true; } }
        public override bool ShouldCloseOnFocusLost { get { return false; } }

        void Awake()
        {

        }

        public void Controls()
        {
            ControlsPopUp.Create(m_gameHud).Open();
        }

        public void QuitToMainMenu()
        {
            ApplicationState.Instance.GoToMainMenu();
        }

        public static PauseMenuPopUp Create(GameHUD gameHud)
        {
            GameObject prefab = Resources.PopUps.Find("PauseMenu");
            if (prefab == null)
            {
                return null;
            }
            GameObject gameObject = Instantiate(prefab, gameHud.transform);
            PauseMenuPopUp popUp = gameObject.GetComponent<PauseMenuPopUp>();
            return popUp;
        }
    }
}
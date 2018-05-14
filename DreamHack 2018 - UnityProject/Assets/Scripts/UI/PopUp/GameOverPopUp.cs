using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.PopUp
{
    public class GameOverPopUp : PopUpWindow
    {
        public override bool IsSingluar { get { return true; } }
        public override bool ShouldCloseOnFocusLost { get { return false; } }
        public override bool PausesGame { get { return true; } }

        public void QuitToMainMenu()
        {
            ApplicationState.Instance.GameOver(WorldController.Instance.MainState);
        }

        public static GameOverPopUp Create(GameHUD gameHud)
        {
            GameObject prefab = Resources.PopUps.Find("GameOver");
            if (prefab == null)
            {
                return null;
            }
            GameObject gameObject = Instantiate(prefab, gameHud.transform);
            GameOverPopUp popUp = gameObject.GetComponent<GameOverPopUp>();
            return popUp;
        }
    }
}
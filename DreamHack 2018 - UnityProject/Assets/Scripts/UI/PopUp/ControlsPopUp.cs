using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.PopUp
{
    public class ControlsPopUp : PopUpWindow
    {
        public override bool IsSingluar { get { return true; } }
        public override bool ShouldCloseOnFocusLost { get { return false; } }
        public override bool PausesGame { get { return true; } }

        public static ControlsPopUp Create(GameHUD gameHud)
        {
            GameObject prefab = Resources.PopUps.Find("Controls");
            if (prefab == null)
            {
                return null;
            }
            GameObject gameObject = Instantiate(prefab, gameHud.transform);
            ControlsPopUp popUp = gameObject.GetComponent<ControlsPopUp>();
            return popUp;
        }
    }
}
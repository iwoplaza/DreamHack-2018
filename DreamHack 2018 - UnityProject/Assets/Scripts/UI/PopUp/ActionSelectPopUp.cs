using Game.Acting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.PopUp
{
    public class ActionSelectPopUp : PopUpWindow
    {
        public override bool IsSingluar { get { return false; } }
        public override bool ShouldCloseOnFocusLost { get { return true; } }

        public void Populate(ISubject subject)
        {

        }

        public static ActionSelectPopUp Create(GameHUD gameHud)
        {
            GameObject prefab = Resources.PopUps.Find("ActionSelect");
            if (prefab == null)
            {
                return null;
            }
            GameObject gameObject = Instantiate(prefab, gameHud.transform);
            ActionSelectPopUp popUp = gameObject.GetComponent<ActionSelectPopUp>();
            return popUp;
        }
    }
}
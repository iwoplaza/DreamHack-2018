using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.PopUp
{
    public class WorkerDetailsPopUp : PopUpWindow
    {
        public override bool IsSingluar { get { return true; } }

        public static GameObject Create(GameHUD gameHud)
        {
            GameObject prefab = Resources.PopUps.Find("WorkerDetails");
            if(prefab == null)
            {
                return null;
            }
            GameObject gameObject = Instantiate(prefab, gameHud.transform);
            return gameObject;
        }
    }
}
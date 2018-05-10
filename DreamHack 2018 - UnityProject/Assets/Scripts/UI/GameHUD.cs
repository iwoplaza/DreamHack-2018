using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameHUD : MonoBehaviour
    {
        public FocusPanel FocusPanel { get; private set; }
        public TimePanel TimePanel { get; private set; }

        void Awake()
        {
            FocusPanel = GetComponentInChildren<FocusPanel>();
            TimePanel = GetComponentInChildren<TimePanel>();
        }
    }
}
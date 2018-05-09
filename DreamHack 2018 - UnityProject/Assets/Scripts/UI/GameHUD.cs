using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameHUD : MonoBehaviour
    {
        public WorkerPanel WorkerPanel { get; private set; }

        public Text timeText;
        public Text dayText;

        void Awake()
        {
            WorkerPanel = GetComponentInChildren<WorkerPanel>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            timeText.text = WorldController.Instance.MainState.TimeSystem.TimeString;
            dayText.text = WorldController.Instance.MainState.TimeSystem.DayCount.ToString();
        }
    }
}
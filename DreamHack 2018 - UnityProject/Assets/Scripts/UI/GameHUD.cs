using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class GameHUD : MonoBehaviour
    {
        public WorkerPanel WorkerPanel { get; private set; }

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

        }
    }
}
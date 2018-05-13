using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class WorkerListEntry : MonoBehaviour
    {
        public WorkerList WorkerList { get; private set; }
        public Worker Worker { get; private set; }

        Button m_button;

        void Awake()
        {
            m_button = GetComponentInChildren<Button>();
        }

        public void Setup(WorkerList workerList, Worker worker)
        {
            WorkerList = workerList;
            Worker = worker;
        }

        public void Select()
        {
            WorkerList.Select(Worker);
        }

        public void OnSelected()
        {
            m_button.interactable = false;
        }

        public void Deselect()
        {
            m_button.interactable = true;
        }
    }
}
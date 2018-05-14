using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class WorkerList : MonoBehaviour
    {
        [SerializeField] GameObject m_entryPrefab;
        [SerializeField] Transform m_entriesHolder;

        public List<WorkerListEntry> WorkerListEntries { get; private set; }
        public GameState GameState { get; private set; }

        public void Setup(GameState gameState)
        {
            GameState = gameState;
            

            GameState.Focus.RegisterEventHandler(Focus.EventType.FOCUS_GAIN, OnFocusGained);
            GameState.Focus.RegisterEventHandler(Focus.EventType.FOCUS_LOSS, OnFocusLost);
            GameState.RegisterOnWorkersChangeHandler(OnWorkersChange);

            Setup();
        }

        void Setup()
        {
            Debug.Log("Setting up Worker List...");

            if (WorkerListEntries != null)
            {
                foreach (WorkerListEntry entry in WorkerListEntries)
                {
                    Destroy(entry.gameObject);
                }
            }

            WorkerListEntries = new List<WorkerListEntry>();

            foreach (Worker worker in GameState.Workers)
            {
                GameObject gameObject = Instantiate(m_entryPrefab, m_entriesHolder);
                WorkerListEntry entry = gameObject.GetComponent<WorkerListEntry>();
                entry.Setup(this, worker);
                WorkerListEntries.Add(entry);
            }
        }

        public void Select(Worker worker)
        {
            GameState.Focus.On(worker);
        }

        void OnSelected(Worker worker)
        {
            foreach (WorkerListEntry entry in WorkerListEntries)
            {
                if (entry.Worker == worker)
                {
                    entry.OnSelected();
                }
                else
                {
                    entry.Deselect();
                }
            }
        }

        void OnFocusGained(IFocusTarget focusTarget)
        {
            if(focusTarget is Worker)
            {
                OnSelected(focusTarget as Worker);
            }
        }

        void OnFocusLost(IFocusTarget focusTarget)
        {
            foreach (WorkerListEntry entry in WorkerListEntries)
            {
                entry.Deselect();
            }
        }

        void OnWorkersChange(GameState gameState)
        {
            Setup();
        }
    }
}
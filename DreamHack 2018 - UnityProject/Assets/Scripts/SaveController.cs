using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SaveController : MonoBehaviour
    {
        public static SaveController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Save(GameState gameStateToSave)
        {
            gameStateToSave.PrepareForSerialization();
            string jsonData = JsonUtility.ToJson(gameStateToSave);
            Debug.Log(jsonData);
        }
    }
}
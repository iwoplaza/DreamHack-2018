using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace Game
{
    public class SaveController : MonoBehaviour
    {
        public static SaveController Instance { get; private set; }

        private const string SAVE_FILE_NAME = "save0.json";

        void Awake()
        {
            Instance = this;
        }

        public void Save(GameState gameStateToSave)
        {
            XElement document = new XElement("root");
            XElement gameStateElement = new XElement("GameState");
            document.Add(gameStateElement);

            gameStateToSave.Populate(gameStateElement);

            string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            File.WriteAllText(filePath, document.ToString());
        }

        public bool Load(GameState gameStateToLoad)
        {
            string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

            if (File.Exists(filePath))
            {
                XElement document = XElement.Parse(File.ReadAllText(filePath));
                XElement gameStateElement = document.Element("GameState");

                Debug.Log("Loading game state...");

                try
                {
                    gameStateToLoad.Parse(gameStateElement);
                    return true;
                }
                catch (FormatException)
                {
                    Debug.LogError("Tried to load a map with a wrong format.");
                }
                catch (ArgumentNullException)
                {
                    Debug.LogError("Tried to load a map with a wrong format.");
                }
            }

            return false;
        }
    }
}
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
        private const string SAVE_DIRECTORY = "Saves/";

        public static List<SavedGame> SavedGames { get; private set; }
        public static string SaveDirectoryPath { get { return Path.Combine(Application.persistentDataPath, SAVE_DIRECTORY); } }

        /// <summary>
        /// Called by the <see cref="ApplicationState"/>
        /// </summary>
        public static void Setup()
        {
            SavedGames = new List<SavedGame>();

            if (!Directory.Exists(SaveDirectoryPath))
            {
                Directory.CreateDirectory(SaveDirectoryPath);
            }
            else
            {
                string[] fileNames = Directory.GetFiles(SaveDirectoryPath, "save_*.xml");
                foreach (string fileName in fileNames)
                {
                    SavedGame savedGame = GetMetadata(fileName);
                    if (savedGame != null)
                    {
                        SavedGames.Add(savedGame);
                    }
                }
            }
        }

        public static void Save(GameState gameStateToSave)
        {
            XElement document = new XElement("root");
            XElement gameStateElement = new XElement("GameState");
            document.Add(gameStateElement);

            gameStateToSave.Populate(gameStateElement);

            /*string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            File.WriteAllText(filePath, document.ToString());*/
        }

        public static bool Load(GameState gameStateToLoad)
        {
            SavedGame savedGame = GetSavedGame(gameStateToLoad.WorldIdentifier);

            if (savedGame == null)
            {
                Debug.LogError("Couldn't load the " + gameStateToLoad.WorldName + " world, no appropriate SavedGame found.");
                return false;
            }

            XElement document = GetSaveData(savedGame);

            if (document != null)
            {
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

        public static SavedGame GetSavedGame(int worldIdentifier)
        {
            foreach(SavedGame savedGame in SavedGames)
            {
                if (savedGame.WorldIdentifier == worldIdentifier)
                    return savedGame;
            }
            return null;
        }

        public static XElement GetSaveData(SavedGame savedGame)
        {
            string fileName = GetFileNameForIndex(savedGame.WorldIdentifier);
            string filePath = Path.Combine(SaveDirectoryPath, fileName);

            if (File.Exists(filePath))
            {
                return XElement.Parse(File.ReadAllText(filePath));
            }

            return null;
        }

        public static SavedGame GetMetadata(string fileName)
        {
            string filePath = Path.Combine(SaveDirectoryPath, fileName);

            if (File.Exists(filePath))
            {
                XElement document = XElement.Parse(File.ReadAllText(filePath));
                if (document != null)
                {
                    XAttribute worldNameAttrib = document.Attribute("worldName");
                    XAttribute worldIdentifierAttrib = document.Attribute("worldIdentifier");

                    if (worldNameAttrib != null && worldIdentifierAttrib != null)
                    {
                        string worldName = worldNameAttrib.Value;
                        int id = int.Parse(worldIdentifierAttrib.Value);

                        return new SavedGame(id, worldName);
                    }
                }
            }

            return null;
        }

        public static bool IsPotentialSaveFile(string fileName)
        {
            string filePath = Path.Combine(Path.Combine(Application.persistentDataPath, SAVE_DIRECTORY), fileName);

            if (File.Exists(filePath))
            {
                XElement document = XElement.Parse(File.ReadAllText(filePath));
                if (document != null)
                {
                    if(document.Element("GameState") != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static string GetFileNameForIndex(int index)
        {
            return "save" + index + ".xml";
        }

        public static int FindFreeWorldIdentifier()
        {
            int freeSaveIndex = 0;

            string fileDirectoryPath = Path.Combine(Application.persistentDataPath, SAVE_DIRECTORY);
            string[] fileNames = Directory.GetFiles(fileDirectoryPath, "save*.xml");

            bool free = false;
            while (!free)
            {
                free = true;
                foreach (string fileName in fileNames)
                {
                    if (fileName == GetFileNameForIndex(freeSaveIndex))
                    {
                        free = false;
                        freeSaveIndex++;
                        continue;
                    }
                }

                if (free)
                    break;
            }

            return freeSaveIndex;
        }

        public static string FindFreeSaveName()
        {
            return GetFileNameForIndex(FindFreeWorldIdentifier());
        }
    }
}
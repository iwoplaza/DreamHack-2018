using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Resources
    {
        public static Dictionary<string, GameObject> TileObjectPrefabs { get; private set; }

        public static GameObject WorkerPrefab { get; private set; }

        public static void LoadAll()
        {
            GameObject[] tileObjectPrefabs = UnityEngine.Resources.LoadAll<GameObject>("TileObjects");
            TileObjectPrefabs = new Dictionary<string, GameObject>();
            foreach (GameObject prefab in tileObjectPrefabs)
                TileObjectPrefabs.Add(prefab.name, prefab);

            WorkerPrefab = UnityEngine.Resources.Load<GameObject>("Worker");
        }

        public static GameObject FindTileObjectPrefab(string name)
        {
            if (!TileObjectPrefabs.ContainsKey(name))
            {
                Debug.LogError("Couldn't find a prefab (" + name + ").");
                return null;
            }
            return TileObjectPrefabs[name];
        }
    }
}
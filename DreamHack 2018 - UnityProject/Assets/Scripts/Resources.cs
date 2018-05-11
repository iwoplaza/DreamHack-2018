using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Resources
    {
        public static Dictionary<string, GameObject> TileObjectPrefabs { get; private set; }
        public static Dictionary<string, GameObject> TileFloorPrefabs { get; private set; }
        public static Dictionary<string, GameObject> EnvironmentObjectPrefabs { get; private set; }
        public static Dictionary<string, Sprite> Icons { get; private set; }

        public static GameObject WorkerPrefab { get; private set; }
        public static GameObject ChunkPrefab { get; private set; }
        public static GameObject TileDisplayPrefab { get; private set; }
        public static GameObject TileDisplayHoverPrefab { get; private set; }

        public static void LoadAll()
        {
            GameObject[] tileObjectPrefabs = UnityEngine.Resources.LoadAll<GameObject>("TileObjects");
            TileObjectPrefabs = new Dictionary<string, GameObject>();
            foreach (GameObject prefab in tileObjectPrefabs)
                TileObjectPrefabs.Add(prefab.name, prefab);

            GameObject[] tileFloorPrefabs = UnityEngine.Resources.LoadAll<GameObject>("TileFloors");
            TileFloorPrefabs = new Dictionary<string, GameObject>();
            foreach (GameObject prefab in tileFloorPrefabs)
                TileFloorPrefabs.Add(prefab.name, prefab);

            GameObject[] environmentObjectPrefabs = UnityEngine.Resources.LoadAll<GameObject>("EnvironmentObjects");
            EnvironmentObjectPrefabs = new Dictionary<string, GameObject>();
            foreach (GameObject prefab in environmentObjectPrefabs)
                EnvironmentObjectPrefabs.Add(prefab.name, prefab);

            Sprite[] icons = UnityEngine.Resources.LoadAll<Sprite>("Icons");
            Icons = new Dictionary<string, Sprite>();
            foreach (Sprite image in icons)
                Icons.Add(image.name, image);

            ChunkPrefab = UnityEngine.Resources.Load<GameObject>("ChunkObject");
            WorkerPrefab = UnityEngine.Resources.Load<GameObject>("Worker");
            TileDisplayPrefab = UnityEngine.Resources.Load<GameObject>("TileDisplay");
            TileDisplayHoverPrefab = UnityEngine.Resources.Load<GameObject>("TileDisplayHover");
        }

        public static GameObject FindTileObjectPrefab(string name)
        {
            if (!TileObjectPrefabs.ContainsKey(name))
            {
                Debug.LogError("Couldn't find a TileObject prefab (" + name + ").");
                return null;
            }
            return TileObjectPrefabs[name];
        }

        public static GameObject FindTileFloorPrefab(string name)
        {
            if (!TileFloorPrefabs.ContainsKey(name))
            {
                Debug.LogError("Couldn't find a TileFloor prefab (" + name + ").");
                return null;
            }
            return TileFloorPrefabs[name];
        }

        public static GameObject FindEnvironmentObjectPrefab(string name)
        {
            if (!EnvironmentObjectPrefabs.ContainsKey(name))
            {
                Debug.LogError("Couldn't find a prefab (" + name + ").");
                return null;
            }
            return EnvironmentObjectPrefabs[name];
        }

        public static Sprite FindIcon(string name)
        {
            if (!Icons.ContainsKey(name))
            {
                Debug.LogError("Couldn't find the icon (" + name + ").");
                return null;
            }
            return Icons[name];
        }
    }
}
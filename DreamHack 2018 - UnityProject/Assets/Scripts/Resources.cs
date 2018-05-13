using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Resources
    {
        public static ResourceCollection<GameObject> TileObjectPrefabs { get; private set; }
        public static ResourceCollection<GameObject> TileFloorPrefabs { get; private set; }
        public static ResourceCollection<GameObject> EnvironmentObjectPrefabs { get; private set; }
        public static ResourceCollection<GameObject> PopUps { get; private set; }
        public static ResourceCollection<Sprite> Icons { get; private set; }
        public static ResourceCollection<AudioClip> Sounds { get; private set; }

        public static GameObject WorkerPrefab { get; private set; }
        public static GameObject ChunkPrefab { get; private set; }
        public static GameObject TileDisplayPrefab { get; private set; }
        public static GameObject TileDisplayHoverPrefab { get; private set; }

        public static void LoadAll()
        {
            TileObjectPrefabs = new ResourceCollection<GameObject>("TileObject prefab", "TileObjects");
            TileFloorPrefabs = new ResourceCollection<GameObject>("TileFloor prefab", "TileFloors");
            EnvironmentObjectPrefabs = new ResourceCollection<GameObject>("EnvironmentObject prefab", "EnvironmentObjects");
            PopUps = new ResourceCollection<GameObject>("PopUp prefab", "PopUps");
            Icons = new ResourceCollection<Sprite>("Icon", "Icons");
            Sounds = new ResourceCollection<AudioClip>("Sound", "Sounds");

            ChunkPrefab = UnityEngine.Resources.Load<GameObject>("ChunkObject");
            WorkerPrefab = UnityEngine.Resources.Load<GameObject>("Worker");
            TileDisplayPrefab = UnityEngine.Resources.Load<GameObject>("TileDisplay");
            TileDisplayHoverPrefab = UnityEngine.Resources.Load<GameObject>("TileDisplayHover");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class WorldController : MonoBehaviour
    {
        public static WorldController Instance { get; private set; }

        [SerializeField]
        protected GameObject m_tilePrefab;

        public GameObject TilePrefab { get { return m_tilePrefab; } }
        public TileMap MainTileMap { get; set; }

        // Use this for initialization
        void Start()
        {
            Instance = this;

            MainTileMap = new TileMap(10, 10);
            MainTileMap.CreateGameObject();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
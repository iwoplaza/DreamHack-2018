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
        public GameState MainState { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        // Use this for initialization
        void Start()
        {
            MainState = new GameState();
            MainState.TileMap.CreateGameObject();
            SaveController.Instance.Save(MainState);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
using Game.TileObjects;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Game
{
    public class GameState
    {
        public TileMap TileMap { get; set; }

        public GameState()
        {
            TileMap = new TileMap(10, 10);
        }

        public void Parse(XElement element)
        {
            if (element == null)
                return;

            XElement tileMapElement = element.Element("TileMap");
            TileMap.Parse(tileMapElement);
        }

        public void Populate(XElement element)
        {
            XElement tileMapElement = new XElement("TileMap");
            element.Add(tileMapElement);
            TileMap.Populate(tileMapElement);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using Utility.Noise;

namespace Game.Environment
{
    [System.Serializable]
    public class MetalMap
    {
        [SerializeField] FractalChain m_metalFractalChain;
        [SerializeField] ushort m_maximumMetalValue;

        public FractalChain MetalFractalChain { get { return m_metalFractalChain; } }
        public Vector2Int Dimensions { get; private set; }
        public ushort[,] Array { get; private set; }

        public void GenerateMap(Vector2Int worldSize, string worldSeed)
        {
            Dimensions = worldSize;
            m_metalFractalChain.GenerateMap(worldSize, worldSeed);

            Array = new ushort[Dimensions.x, Dimensions.y];
            for(int x = 0; x < Dimensions.x; ++x)
                for (int y = 0; y < Dimensions.y; ++y)
                    Array[x, y] = (ushort) 0;
        }

        public void PopulateMapForNewWorld()
        {
            for (int x = 0; x < Dimensions.x; ++x)
                for (int y = 0; y < Dimensions.y; ++y)
                    Array[x, y] = (ushort) Mathf.FloorToInt(MetalFractalChain.CurrentNoise[x, y] * m_maximumMetalValue);
        }

        public void Parse(XElement element)
        {
            IEnumerable tileElements = element.Elements("Tile");
            foreach (XElement tileElement in tileElements)
            {
                XAttribute xAttrib = tileElement.Attribute("x");
                XAttribute yAttrib = tileElement.Attribute("y");
                XAttribute metalAttrib = tileElement.Attribute("metalAmount");

                if (xAttrib != null && yAttrib != null && metalAttrib != null)
                {
                    int x = int.Parse(xAttrib.Value);
                    int y = int.Parse(yAttrib.Value);
                    int metalAmount = int.Parse(metalAttrib.Value);
                    Array[x, y] = (ushort) metalAmount;
                }
            }

            for (int x = 0; x < Dimensions.x; ++x)
            {
                for (int y = 0; y < Dimensions.y; ++y)
                {
                    MetalFractalChain.SetValueNoApply(x, y, Mathf.Clamp01((float)Array[x, y] / m_maximumMetalValue));
                }
            }
            MetalFractalChain.ApplyTexture();
        }

        public void Populate(XElement element)
        {
            for (int x = 0; x < Dimensions.x; ++x)
            {
                for (int y = 0; y < Dimensions.y; ++y)
                {
                    ushort metalAmount = Array[x, y];
                    if (metalAmount == 0)
                        continue;

                    XElement tileElement = new XElement("Tile");
                    element.Add(tileElement);
                    tileElement.SetAttributeValue("x", x);
                    tileElement.SetAttributeValue("y", y);
                    tileElement.SetAttributeValue("metalAmount", metalAmount);
                }
            }
        }

        public ushort MetalAmountAt(TilePosition position)
        {
            if(position.X < 0 || position.X >= Dimensions.x ||
                position.Z < 0 || position.Z >= Dimensions.y)
            {
                return 0;
            }

            return Array[position.X, position.Z];
        }

        public bool SetMetalAmountAt(TilePosition position, ushort amount)
        {
            if (position.X < 0 || position.X >= Dimensions.x ||
                position.Z < 0 || position.Z >= Dimensions.y)
            {
                return false;
            }

            Array[position.X, position.Z] = amount;
            MetalFractalChain.SetValue(position.X, position.Z, Mathf.Clamp01((float) amount / m_maximumMetalValue));

            return true;
        }
    }
}
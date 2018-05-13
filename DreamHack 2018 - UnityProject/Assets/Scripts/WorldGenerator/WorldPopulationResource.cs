using Game;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Environment
{
    public static class WorldPopulationResource
    {
        public enum PopulationType
        {
            VEGETATION_DESERT,
            VEGETATION_GREEN,
            ROCK_SMALL,
            ROCK_LARGE
        }

        private static List<GameObject> m_desertVegetation;
        private static List<GameObject> m_greenVegetation;        
        private static List<GameObject> m_RocksSmall;
        private static List<GameObject> m_RocksLarge;


        public static void UpdatePopulationDirectory()
        {
            m_desertVegetation = new List<GameObject>();
            m_greenVegetation = new List<GameObject>();         
            m_RocksSmall = new List<GameObject>();            
            m_RocksLarge = new List<GameObject>();            

            for(int i = 0; i < 99; i++)
            {
                if(Resources.TileObjectPrefabs.ContainsKey("PlantDesert_" + i.ToString()))
                {
                    GameObject temp = Resources.TileObjectPrefabs.Find("PlantDesert_" + i.ToString());
                    if(temp != null)
                    {
                        m_desertVegetation.Add(temp);
                    }
                }
            }
            for(int i = 0; i < 99; i++)
            {
                if(Resources.TileObjectPrefabs.ContainsKey("RockSmall_" + i.ToString()))
                {     
                    GameObject temp = Resources.TileObjectPrefabs.Find("RockSmall_" + i.ToString());
                    if(temp != null)
                    {
                        m_RocksSmall.Add(temp);
                    }
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                if(Resources.TileObjectPrefabs.ContainsKey("RockLarge_" + i.ToString()))
                {     
                    GameObject temp = Resources.TileObjectPrefabs.Find("RockLarge_" + i.ToString());
                    if(temp != null)
                    {
                        m_RocksLarge.Add(temp);
                    }
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                if(Resources.TileObjectPrefabs.ContainsKey("PlantGreen_" + i.ToString()))
                {     
                    GameObject temp = Resources.TileObjectPrefabs.Find("PlantGreen_" + i.ToString());
                    if(temp != null)
                    {
                        m_greenVegetation.Add(temp);
                    }
                }
            }
        }

        public static GameObject GetResources(PopulationType type)
        {
            switch(type)
            {
                case PopulationType.VEGETATION_DESERT:
                    return m_desertVegetation[Random.Range(0,m_desertVegetation.Count)];
                case PopulationType.VEGETATION_GREEN:
                    return m_greenVegetation[Random.Range(0,m_greenVegetation.Count)];
                case PopulationType.ROCK_SMALL:
                    return m_RocksSmall[Random.Range(0,m_RocksSmall.Count)];
                case PopulationType.ROCK_LARGE:
                    return m_RocksLarge[Random.Range(0,m_RocksLarge.Count)];                
            }
            return null;
        }
    }
}
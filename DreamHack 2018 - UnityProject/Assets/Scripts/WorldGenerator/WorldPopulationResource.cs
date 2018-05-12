using Game;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Environment
{
    public static class WorldPopulationResource
    {
        public enum PopulationType
        {
            VEGETATION_DESERT
        }

        private static List<GameObject> m_desertVegetation;        


        public static void UpdatePopulationDirectory()
        {
            m_desertVegetation = new List<GameObject>();            

            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindTileObjectPrefab("PlantDesert_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_desertVegetation.Add(temp);
                    i = 100;                        
                }
            }            
        }

        public static GameObject GetResources(PopulationType type)
        {
            switch(type)
            {
                case PopulationType.VEGETATION_DESERT:
                    return m_desertVegetation[Random.Range(0,m_desertVegetation.Count)];                    
            }
            return null;
        }
    }
}
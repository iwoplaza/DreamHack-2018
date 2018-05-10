using Game;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Environment
{
    public static class CliffMeshResource
    {
        public enum MeshType
        {
            GROUND,
            GROUND_CLIFF,
            EDGE_CLIFF_STRAIGHT,
            EDGE_CLIFF_DIAGONAL,
            EDGE_CLIFF_SHARPCORNER,
            EDGE_CLIFF_CORNER
        }

        private static List<Mesh> m_groundMeshes;
        private static List<Mesh> m_groundCliffMeshes;
        private static List<Mesh> m_edgeCliffStraightMeshes;
        private static List<Mesh> m_edgeCliffDiagonalMeshes;
        private static List<Mesh> m_edgeCliffSharpCornerMeshes;
        private static List<Mesh> m_edgeCliffCornerMeshes;

        public static void UpdateMeshDictionary()
        {
            m_groundMeshes.Clear();
            m_groundCliffMeshes.Clear();
            m_edgeCliffStraightMeshes.Clear();
            m_edgeCliffDiagonalMeshes.Clear();
            m_edgeCliffSharpCornerMeshes.Clear();
            m_edgeCliffCornerMeshes.Clear();

            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("Ground_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_groundMeshes.Add(temp.GetComponent<MeshFilter>().mesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("GroundCliff_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_groundCliffMeshes.Add(temp.GetComponent<MeshFilter>().mesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeStraight_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffStraightMeshes.Add(temp.GetComponent<MeshFilter>().mesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeDiagonal_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffDiagonalMeshes.Add(temp.GetComponent<MeshFilter>().mesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeSharp_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffSharpCornerMeshes.Add(temp.GetComponent<MeshFilter>().mesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeCorner_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffCornerMeshes.Add(temp.GetComponent<MeshFilter>().mesh);
                    i = 100;                        
                }
            }
        }

        public static Mesh GetResources(MeshType type)
        {
            switch(type)
            {
                case MeshType.GROUND:
                    return m_groundMeshes[Random.Range(0,m_groundMeshes.Count)];                    
                case MeshType.GROUND_CLIFF:
                    return m_groundCliffMeshes[Random.Range(0,m_groundCliffMeshes.Count)];
                case MeshType.EDGE_CLIFF_STRAIGHT:
                    return m_edgeCliffStraightMeshes[Random.Range(0,m_edgeCliffStraightMeshes.Count)];
                case MeshType.EDGE_CLIFF_CORNER:
                    return m_edgeCliffCornerMeshes[Random.Range(0,m_edgeCliffCornerMeshes.Count)];
                case MeshType.EDGE_CLIFF_DIAGONAL:
                    return m_edgeCliffDiagonalMeshes[Random.Range(0,m_edgeCliffDiagonalMeshes.Count)];
                case MeshType.EDGE_CLIFF_SHARPCORNER:
                    return m_edgeCliffSharpCornerMeshes[Random.Range(0,m_edgeCliffSharpCornerMeshes.Count)];
            }
            return null;
        }
    }
}
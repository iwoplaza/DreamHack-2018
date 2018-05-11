using Game;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Environment
{
    public static class WorldMeshResource
    {
        public enum MeshType
        {
            GROUND,
            GROUND_CLIFF,
            CLIFF_LONE,
            EDGE_CLIFF_STRAIGHT,
            EDGE_CLIFF_STRAIGHT_CASE1,
            EDGE_CLIFF_STRAIGHT_CASE2,
            EDGE_CLIFF_STRAIGHT_CASE3,
            EDGE_CLIFF_STRAIGHT_LONE,
            EDGE_CLIFF_STRAIGHT_LONE_END,
            EDGE_CLIFF_DIAGONAL,
            EDGE_CLIFF_DIAGONAL_CASE1,
            EDGE_CLIFF_SHARPCORNER,
            EDGE_CLIFF_CORNER_CASE1,
            EDGE_CLIFF_CORNER_CASE21,
            EDGE_CLIFF_CORNER_CASE22,
            EDGE_CLIFF_CORNER_CASE3,
            EDGE_CLIFF_CORNER_CASE4

        }

        private static List<Mesh> m_groundMeshes;
        private static List<Mesh> m_groundCliffMeshes;
        private static List<Mesh> m_cliffLoneMeshes;
        private static List<Mesh> m_edgeCliffStraightMeshes;
        private static List<Mesh> m_edgeCliffStraightCase1Meshes;
        private static List<Mesh> m_edgeCliffStraightCase2Meshes;
        private static List<Mesh> m_edgeCliffStraightCase3Meshes;
        private static List<Mesh> m_edgeCliffStraightLoneMeshes;
        private static List<Mesh> m_edgeCliffStraightLoneEndMeshes;
        private static List<Mesh> m_edgeCliffDiagonalMeshes;
        private static List<Mesh> m_edgeCliffDiagonalCase1Meshes;
        private static List<Mesh> m_edgeCliffSharpCornerMeshes;
        private static List<Mesh> m_edgeCliffCornerCase1Meshes;
        private static List<Mesh> m_edgeCliffCornerCase21Meshes;
        private static List<Mesh> m_edgeCliffCornerCase22Meshes;
        private static List<Mesh> m_edgeCliffCornerCase3Meshes;
        private static List<Mesh> m_edgeCliffCornerCase4Meshes;


        public static void UpdateMeshDictionary()
        {
            m_groundMeshes = new List<Mesh>();
            m_groundCliffMeshes = new List<Mesh>();
            m_cliffLoneMeshes = new List<Mesh>();
            m_edgeCliffStraightMeshes = new List<Mesh>();
            m_edgeCliffStraightCase1Meshes = new List<Mesh>();
            m_edgeCliffStraightCase2Meshes = new List<Mesh>();
            m_edgeCliffStraightCase3Meshes = new List<Mesh>();
            m_edgeCliffStraightLoneMeshes = new List<Mesh>();
            m_edgeCliffStraightLoneEndMeshes = new List<Mesh>();
            m_edgeCliffDiagonalMeshes = new List<Mesh>();
            m_edgeCliffDiagonalCase1Meshes = new List<Mesh>();
            m_edgeCliffSharpCornerMeshes = new List<Mesh>();
            m_edgeCliffCornerCase1Meshes = new List<Mesh>();
            m_edgeCliffCornerCase21Meshes = new List<Mesh>();
            m_edgeCliffCornerCase22Meshes = new List<Mesh>();
            m_edgeCliffCornerCase3Meshes = new List<Mesh>();
            m_edgeCliffCornerCase4Meshes = new List<Mesh>();

            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("Ground_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_groundMeshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("GroundCliff_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_groundCliffMeshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffLone_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_cliffLoneMeshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeStraight_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffStraightMeshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeStraightCase1_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffStraightCase1Meshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeStraightCase2_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffStraightCase2Meshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeStraightCase3_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffStraightCase3Meshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeStraightLone_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffStraightLoneMeshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeStraightLoneEnd_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffStraightLoneEndMeshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeDiagonal_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffDiagonalMeshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeDiagonalCase1_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffDiagonalCase1Meshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeSharp_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffSharpCornerMeshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeCornerCase1_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffCornerCase1Meshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeCornerCase21_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffCornerCase21Meshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeCornerCase22_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffCornerCase22Meshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeCornerCase3_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffCornerCase3Meshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
                    i = 100;                        
                }
            }
            for(int i = 0; i < 99; i++)
            {                
                GameObject temp = Resources.FindEnvironmentObjectPrefab("CliffEdgeCornerCase4_" + i.ToString());
                if(temp != null)
                {
                    if(temp.GetComponent<MeshFilter>() != null)
                        m_edgeCliffCornerCase4Meshes.Add(temp.GetComponent<MeshFilter>().sharedMesh);
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
                case MeshType.CLIFF_LONE:
                    return m_cliffLoneMeshes[Random.Range(0,m_cliffLoneMeshes.Count)];
                case MeshType.EDGE_CLIFF_STRAIGHT:
                    return m_edgeCliffStraightMeshes[Random.Range(0,
                    m_edgeCliffStraightMeshes.Count)];
                case MeshType.EDGE_CLIFF_STRAIGHT_CASE1:
                    return m_edgeCliffStraightCase1Meshes[Random.Range(0,m_edgeCliffStraightCase1Meshes.Count)];
                case MeshType.EDGE_CLIFF_STRAIGHT_CASE2:
                    return m_edgeCliffStraightCase2Meshes[Random.Range(0,m_edgeCliffStraightCase2Meshes.Count)];
                case MeshType.EDGE_CLIFF_STRAIGHT_CASE3:
                    return m_edgeCliffStraightCase3Meshes[Random.Range(0,m_edgeCliffStraightCase3Meshes.Count)];
                case MeshType.EDGE_CLIFF_STRAIGHT_LONE:
                    return m_edgeCliffStraightLoneMeshes[Random.Range(0,m_edgeCliffStraightLoneMeshes.Count)];
                case MeshType.EDGE_CLIFF_STRAIGHT_LONE_END:
                    return m_edgeCliffStraightLoneEndMeshes[Random.Range(0,m_edgeCliffStraightLoneEndMeshes.Count)];                
                case MeshType.EDGE_CLIFF_DIAGONAL:
                    return m_edgeCliffDiagonalMeshes[Random.Range(0,
                    m_edgeCliffDiagonalMeshes.Count)];
                case MeshType.EDGE_CLIFF_DIAGONAL_CASE1:
                    return m_edgeCliffDiagonalCase1Meshes[Random.Range(0,m_edgeCliffDiagonalCase1Meshes.Count)];
                case MeshType.EDGE_CLIFF_SHARPCORNER:
                    return m_edgeCliffSharpCornerMeshes[Random.Range(0,m_edgeCliffSharpCornerMeshes.Count)];
                case MeshType.EDGE_CLIFF_CORNER_CASE1:
                    return m_edgeCliffCornerCase1Meshes[Random.Range(0,m_edgeCliffCornerCase1Meshes.Count)];
                case MeshType.EDGE_CLIFF_CORNER_CASE21:
                    return m_edgeCliffCornerCase21Meshes[Random.Range(0,m_edgeCliffCornerCase21Meshes.Count)];
                case MeshType.EDGE_CLIFF_CORNER_CASE22:
                    return m_edgeCliffCornerCase22Meshes[Random.Range(0,m_edgeCliffCornerCase22Meshes.Count)];
                case MeshType.EDGE_CLIFF_CORNER_CASE3:
                    return m_edgeCliffCornerCase3Meshes[Random.Range(0,m_edgeCliffCornerCase3Meshes.Count)];
                case MeshType.EDGE_CLIFF_CORNER_CASE4:
                    return m_edgeCliffCornerCase4Meshes[Random.Range(0,m_edgeCliffCornerCase4Meshes.Count)];
            }
            return null;
        }
    }
}
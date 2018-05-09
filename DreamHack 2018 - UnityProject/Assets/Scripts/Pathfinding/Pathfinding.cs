using System.Collections.Generic;
using Utility.Heaps;
using UnityEngine;
using Game.Pathfinding.Internal;
using Game;

namespace Game.Pathfinding.Internal
{
    public static class Pathfinding
    {
        public static List<TilePosition> FindPath(PathfindingRule rule, TileMap map, TilePosition start, TilePosition end)
        {
            if(map.TileAt(start).HasObject || start == end)
            {
                Debug.Log("No path found! Either the starting position is obstructed or the start and end pos is the same!");
                return new List<TilePosition>();
            }

            float[,] weightMap = new float[map.Width,map.Height];
            for(int x = 0; x < map.Width; x++)
            {
                for(int y = 0; y < map.Height; y++)
                {
                    weightMap[x,y] = -1;
                }
            }

            FibonacciHeap<PathNode> pathHeaps = new FibonacciHeap<PathNode>();
            PathNode startNode = new PathNode(Heuristic(start,end), start);
            pathHeaps.AddToHeap(startNode.Weight, startNode);
            bool foundEndTile = false;
            PathNode endNode = null;

            // Our A* implementation
            while(pathHeaps.HeapSize > 0 && !foundEndTile)
            {
                PathNode cheapestNode = pathHeaps.GetSmallest();
                foreach(TilePosition pos in map.GetNeighbours(rule, cheapestNode.Location))
                {
                    float newWeight = cheapestNode.Weight + GetCost(cheapestNode.Location,pos);
                    if(weightMap[pos.X, pos.Z] < 0 || newWeight < weightMap[pos.X, pos.Z])
                    {
                        weightMap[pos.X, pos.Z] = newWeight;
                        PathNode newNode = new PathNode(cheapestNode, newWeight, pos);
                        if(pos == end)
                        {
                            endNode = newNode;
                            foundEndTile = true;
                            break;
                        }
                        else
                        {
                            pathHeaps.AddToHeap(newWeight + Heuristic(pos,end), newNode);
                        }
                    }
                }
            }
            if(!foundEndTile)
            {
                Debug.Log("No path found for starting point: " + start.ToString() + " end point: " + end.ToString());
                return new List<TilePosition>();
            }
            else
            {
                return TracePath(endNode, map);
            }
        }

        static List<TilePosition> TracePath(PathNode node, TileMap map)
        {
            // The path returned by RecursivePathTrace is in reversed order
            // It's corrected in the for loop
            List<TilePosition> tempPath = RecursivePathTrace(new List<TilePosition>(), map, node);
            List<TilePosition> path = new List<TilePosition>();
            for(int i = tempPath.Count - 1; i >= 0; i--)
            {
                path.Add(tempPath[i]);
            }
            return path;
        }

        static List<TilePosition> RecursivePathTrace(List<TilePosition> srcQueue, TileMap map, PathNode currentNode)
        {
            if(currentNode.ParentNode != null)
            {
                srcQueue.Add(currentNode.Location);
                return RecursivePathTrace(srcQueue, map, currentNode.ParentNode);
            }
            return srcQueue;
        }

        static float Heuristic(TilePosition start, TilePosition end)
        {
            int abs_x = Mathf.Abs(end.X - start.X);
            int abs_y = Mathf.Abs(end.Z - start.Z);
            return Mathf.Min(abs_x,abs_y) * 1.41421f + Mathf.Abs(abs_x - abs_y);
        }

        static float GetCost(TilePosition start, TilePosition end)
        {
            int x = Mathf.Abs(end.X - start.X);
            int y = Mathf.Abs(end.Z - start.Z);
            return (x + y) > 1 ? 1.41421356237f : 1;
        }

        static List<TilePosition> GetNeighbours(this TileMap map, PathfindingRule rule, TilePosition tilePos)
        {
            List<TilePosition> neighbours = new List<TilePosition>();

            if(tilePos.X > 0)
            {
                if(rule.CanPassThrough(map.TileAt(new TilePosition(tilePos.X - 1, tilePos.Z)), Direction.POSITIVE_X))
                {
                    neighbours.Add(new TilePosition(tilePos.X - 1, tilePos.Z));                    
                }
                if(!map.TileAt(new TilePosition(tilePos.X - 1, tilePos.Z)).HasObject){
                    if(tilePos.Z > 0)
                        {
                            if(!map.TileAt(new TilePosition(tilePos.X, tilePos.Z - 1)).HasObject)
                            {
                                if(!map.TileAt(new TilePosition(tilePos.X - 1, tilePos.Z - 1)).HasObject)
                                {
                                    neighbours.Add(new TilePosition(tilePos.X - 1, tilePos.Z - 1));
                                }
                            }
                        }
                    if(tilePos.Z < map.Height - 1)
                    {
                        if(!map.TileAt(new TilePosition(tilePos.X, tilePos.Z + 1)).HasObject)
                        {
                            if(!map.TileAt(new TilePosition(tilePos.X - 1, tilePos.Z + 1)).HasObject)
                            {
                                neighbours.Add(new TilePosition(tilePos.X - 1, tilePos.Z + 1));
                            }
                        }
                    }
                }
            }
            if(tilePos.Z > 0)
            {
                if(rule.CanPassThrough(map.TileAt(new TilePosition(tilePos.X, tilePos.Z - 1)), Direction.POSITIVE_Z))
                {
                    neighbours.Add(new TilePosition(tilePos.X, tilePos.Z - 1));
                }
            }
            if(tilePos.X < map.Width - 1)
            {
                if(rule.CanPassThrough(map.TileAt(new TilePosition(tilePos.X + 1, tilePos.Z)), Direction.NEGATIVE_X))
                {
                    neighbours.Add(new TilePosition(tilePos.X + 1, tilePos.Z));
                }
                if(!map.TileAt(new TilePosition(tilePos.X + 1, tilePos.Z)).HasObject)
                {
                    if(tilePos.Z > 0)
                    {
                        if(!map.TileAt(new TilePosition(tilePos.X, tilePos.Z - 1)).HasObject)
                        {
                            if(!map.TileAt(new TilePosition(tilePos.X + 1, tilePos.Z - 1)).HasObject)
                            {
                                neighbours.Add(new TilePosition(tilePos.X + 1, tilePos.Z - 1));
                            }
                        }
                    }
                    if(tilePos.Z < map.Height - 1)
                    {
                        if(!map.TileAt(new TilePosition(tilePos.X, tilePos.Z + 1)).HasObject)
                        {
                            if(!map.TileAt(new TilePosition(tilePos.X + 1, tilePos.Z + 1)).HasObject)
                            {
                                neighbours.Add(new TilePosition(tilePos.X + 1, tilePos.Z + 1));

                            }
                        }                
                    }
                }
            }
            if(tilePos.Z < map.Height - 1)
            {
                if(rule.CanPassThrough(map.TileAt(new TilePosition(tilePos.X, tilePos.Z + 1)),Direction.NEGATIVE_Z))
                {
                    neighbours.Add(new TilePosition(tilePos.X, tilePos.Z + 1));
                }
            }
            return neighbours;
        }
    }
}
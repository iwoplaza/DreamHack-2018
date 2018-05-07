using System.Collections.Generic;
using Utility.Heaps;
using UnityEngine;
using Game.Pathfinding.Internal;
using Game;

namespace Game.Pathfinding{
    public static class Pathfinding{
        public static Queue<TilePosition> FindPath(this Living livingObj, TileMap map, TilePosition start, TilePosition end){
            if(map.TileAt(start).HasObject || start == end){
                Debug.Log("No path found! Either the starting position is obstructed or the start and end pos is the same!");
                return new Queue<TilePosition>();
            }

            float[,] weightMap = new float[map.Width,map.Height];
            for(int x = 0; x < map.Width; x++){
                for(int y = 0; y < map.Height; y++){
                    weightMap[x,y] = -1;
                }
            }

            FibonacciHeap<PathNode> pathHeaps = new FibonacciHeap<PathNode>();
            PathNode startNode = new PathNode(Heuristic(start,end), start);
            pathHeaps.AddToHeap(startNode.Weight, startNode);
            bool foundEndTile = false;
            PathNode endNode = null;
            while(pathHeaps.HeapSize > 0 && !foundEndTile){
                PathNode cheapestNode = pathHeaps.GetSmallest();
                foreach(TilePosition pos in map.GetNeighbours(livingObj, cheapestNode.Location)){
                    float newWeight = Heuristic(pos,end) + cheapestNode.Weight;
                    if(weightMap[pos.X,pos.Y] < 0 || newWeight < weightMap[pos.X,pos.Y]){
                        weightMap[pos.X,pos.Y] = newWeight;
                        PathNode newNode = new PathNode(cheapestNode, newWeight, pos);
                        if(pos == end){
                            endNode = newNode;
                            foundEndTile = true;
                            break;
                        }else{
                            pathHeaps.AddToHeap(newNode.Weight, newNode);
                        }
                    }
                }
            }
            if(!foundEndTile){
                Debug.Log("No path found for starting point: " + start.ToString() + " end point: " + end.ToString());
                return new Queue<TilePosition>();
            }else{
                return TracePath(endNode);
            }
        }

        static Queue<TilePosition> TracePath(PathNode node){
            Queue<TilePosition> path = RecursivePathTrace(new Queue<TilePosition>(),node);
            Stack<TilePosition> pathStack = new Stack<TilePosition>();
            while(path.Count > 0){
                pathStack.Push(path.Dequeue());
            }
            while(pathStack.Count > 0){
                path.Enqueue(pathStack.Pop());
            }
            return path;
        }

        static Queue<TilePosition> RecursivePathTrace(Queue<TilePosition> srcQueue, PathNode currentNode){
            if(currentNode.ParentNode != null){
                srcQueue.Enqueue(currentNode.Location);
                return RecursivePathTrace(srcQueue, currentNode.ParentNode);
            }
            return srcQueue;
        }

        static float Heuristic(TilePosition start, TilePosition end){
            int abs_x = Mathf.Abs(end.X - start.X);
            int abs_y = Mathf.Abs(end.Y - start.Y);
            return Mathf.Min(abs_x,abs_y) * 1.41421f + Mathf.Abs(abs_x - abs_y);
        }

        static List<TilePosition> GetNeighbours(this TileMap map, Living livingObj, TilePosition tilePos){
            List<TilePosition> neighbours = new List<TilePosition>();

            if(tilePos.X > 0){
                if(map.TileAt(new TilePosition(tilePos.X - 1, tilePos.Y)).IsPassableFor(livingObj, Direction.POSITIVE_X)){
                    neighbours.Add(new TilePosition(tilePos.X - 1, tilePos.Y));
                }
                if(!map.TileAt(new TilePosition(tilePos.X - 1, tilePos.Y)).HasObject){
                    if(tilePos.Y > 0){
                        if(!map.TileAt(new TilePosition(tilePos.X - 1, tilePos.Y - 1)).HasObject){
                            neighbours.Add(new TilePosition(tilePos.X - 1, tilePos.Y - 1));
                        }
                    }
                    if(tilePos.Y < map.Height - 1){
                        if(!map.TileAt(new TilePosition(tilePos.X - 1, tilePos.Y + 1)).HasObject){
                            neighbours.Add(new TilePosition(tilePos.X - 1, tilePos.Y + 1));
                        }                
                    }
                }
            }
            if(tilePos.Y > 0){
                if(map.TileAt(new TilePosition(tilePos.X, tilePos.Y - 1)).IsPassableFor(livingObj, Direction.POSITIVE_Z)){
                    neighbours.Add(new TilePosition(tilePos.X, tilePos.Y - 1));
                }
            }
            if(tilePos.X < map.Width - 1){
                if(map.TileAt(new TilePosition(tilePos.X + 1, tilePos.Y)).IsPassableFor(livingObj, Direction.NEGATIVE_X)){
                    neighbours.Add(new TilePosition(tilePos.X + 1, tilePos.Y));
                }
                if(!map.TileAt(new TilePosition(tilePos.X + 1, tilePos.Y)).HasObject){
                    if(tilePos.Y > 0){
                        if(!map.TileAt(new TilePosition(tilePos.X + 1, tilePos.Y - 1)).HasObject){
                            neighbours.Add(new TilePosition(tilePos.X + 1, tilePos.Y - 1));
                        }
                    }
                    if(tilePos.Y < map.Height - 1){
                        if(!map.TileAt(new TilePosition(tilePos.X + 1, tilePos.Y + 1)).HasObject){
                            neighbours.Add(new TilePosition(tilePos.X + 1, tilePos.Y + 1));
                        }                
                    }
                }
            }
            if(tilePos.Y < map.Height - 1){
                if(map.TileAt(new TilePosition(tilePos.X, tilePos.Y + 1)).IsPassableFor(livingObj, Direction.NEGATIVE_Z)){
                    neighbours.Add(new TilePosition(tilePos.X, tilePos.Y + 1));
                }
            }
            return neighbours;
        }
    }
}
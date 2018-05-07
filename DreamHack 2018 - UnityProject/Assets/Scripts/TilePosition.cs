using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class TilePosition
    {
        [SerializeField] protected int m_x;
        [SerializeField] protected int m_y;

        public int X { get { return m_x; } set { m_x = value; } }
        public int Y { get { return m_y; } set { m_y = value; } }

        public Vector3 Vector3 {
            get
            {
                return new Vector3(X, 0, Y);
            }
        }

        public TilePosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //
            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            TilePosition other = obj as TilePosition;
            
            // TODO: write your implementation of Equals() here
            return (other.X == X && other.Y == Y);
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return X*3 + Y*23;
        }

        public static bool operator ==(TilePosition a, TilePosition b){
            if((object)(a) == null){
                if((object)(b) == null)
                    return true;
                else
                    return false;                
            }
            return a.Equals(b);
        }

        public static bool operator !=(TilePosition a, TilePosition b){
            if((object)a == null){
                if( (object)b == null)
                    return false;
                else
                    return true;
            }
            return !a.Equals(b);
        }

        public override string ToString(){
            return "X Pos: " + X.ToString() + " Y Pos: " + Y.ToString();
        }

        public static TilePosition FromWorldPosition(Vector3 worldPos)
        {
            TilePosition position = new TilePosition(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.z));
            return position;
        }
    }
}
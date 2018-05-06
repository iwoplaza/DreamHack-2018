using System.Collections.Generic;

namespace Utility.Heaps.Internal{
public class FibonacciNode<T>{
        
        public List<FibonacciNode<T>> ChildNodes{ get; private set; }

        public T Value { get; private set;}

        public float Weight{ get; private set; }

        public int ChildCount{ get{return ChildNodes.Count;} }

        public FibonacciNode(float weight, T obj){
            ChildNodes = new List<FibonacciNode<T>>();
            Value = obj;
            Weight = weight;
        }

        public void AddChildNode(FibonacciNode<T> node) {
            ChildNodes.Add(node);
        }

    }
}
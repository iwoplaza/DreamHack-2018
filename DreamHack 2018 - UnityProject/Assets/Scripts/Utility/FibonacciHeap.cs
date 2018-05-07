using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Heaps.Internal;

namespace Utility.Heaps
{
	public class FibonacciHeap<T>
    {
		private List<FibonacciNode<T>> m_topLevelNodes;
		private int m_lightestNodeIndex;

        public int HeapSize { get; private set; }
        public int TopNodeCount { get{ return m_topLevelNodes.Count; } }

		public FibonacciHeap()
        {
			m_topLevelNodes = new List<FibonacciNode<T>>();
		}

		public T GetSmallest()
        {
			HeapSize--;	
			T value = m_topLevelNodes[m_lightestNodeIndex].Value;
			foreach(FibonacciNode<T> childNode in m_topLevelNodes[m_lightestNodeIndex].ChildNodes)
            {
				m_topLevelNodes.Add(childNode);
			}
			m_topLevelNodes.RemoveAt(m_lightestNodeIndex);
			m_lightestNodeIndex = -1;
			RebuildHeap();
			return value;
		}

		public void AddToHeap(float weight, T obj)
        {
			HeapSize++;
			m_topLevelNodes.Add(new FibonacciNode<T>(weight, obj));
			if(m_lightestNodeIndex == -1 || m_topLevelNodes[m_lightestNodeIndex].Weight > weight)
            {
				m_lightestNodeIndex = m_topLevelNodes.Count - 1;
			}
		}

		void RebuildHeap()
        {
			if(m_topLevelNodes.Count == 0)
				return;			
			int mergeCount = 1;
			while(mergeCount > 0)
            {				
				mergeCount = MergeNodes();
			}

			float cheapestWeight = m_topLevelNodes[0].Weight;
			m_lightestNodeIndex = 0;

			for(int i = 0; i < m_topLevelNodes.Count; i++)
            {
				FibonacciNode<T> evaluatedNode = m_topLevelNodes[i];
				if(cheapestWeight > evaluatedNode.Weight)
                {
					cheapestWeight = evaluatedNode.Weight;
					m_lightestNodeIndex = i;
				}
			}
		}

		private int MergeNodes()
        {
			int mergeCount = 0;
			for(int i = m_topLevelNodes.Count - 1; i >= 1; i--)
            {				
				if(m_topLevelNodes[i].ChildCount == m_topLevelNodes[i-1].ChildCount)
                {
					if(m_topLevelNodes[i].Weight < m_topLevelNodes[i-1].Weight)
                    {
						m_topLevelNodes[i].AddChildNode(m_topLevelNodes[i-1]);
						m_topLevelNodes.RemoveAt(i-1);
					}
                    else
                    {
						m_topLevelNodes[i-1].AddChildNode(m_topLevelNodes[i]);
						m_topLevelNodes.RemoveAt(i);
					}
					mergeCount++;
				}
			}
			return mergeCount;
		}
	}
}

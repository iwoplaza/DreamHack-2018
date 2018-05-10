using Utility.Noise;
using UnityEngine;
using System.Collections.Generic;

namespace Utility.Noise.Internal
{
    [System.Serializable]
    public class FractalGeneratorChain
    {
        public enum OperatorType
        {
            ADD,
            REMOVE,
            MULTIPLY,
            DIVIDE
        }

        [SerializeField]FractalGenerator m_generator;
        public FractalGenerator CurrentGenerator { get{ return m_generator; } }
        [SerializeField]OperatorType m_operation;
        public OperatorType OperationType { get{ return m_operation; } }
    }
}
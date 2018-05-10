using Game.Acting.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Acting
{
    /// <summary>
    /// Something that can act out actions.
    /// </summary>
    public interface IActor
    {
        void PerformAction(ActionBase action, ISubject subject);
    }
}
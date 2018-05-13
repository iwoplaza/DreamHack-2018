using Game.Acting.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Acting
{
    /// <summary>
    /// An object that can be acted upon.
    /// </summary>
    public interface ISubject
    {
        Vector3 Position { get; }

        List<ActionBase> GetActionsFor(IActor actor);
    }
}
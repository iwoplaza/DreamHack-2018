using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tasks
{
    /// <summary>
    /// Used as a base for all types of Tasks.
    /// 
    /// A Task can be a part of a TaskQueue, which is mainly
    /// used by Workers to keep track of what to do now,
    /// and what to do next.
    /// </summary>
    public abstract class TaskBase
    {
        /// <summary>
        /// This is used as a super short text representation of the task in UI.
        /// </summary>
        public abstract string ActionQuickie { get; }

        /// <summary>
        /// This is used as a short text representation of the task in UI.
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// This is used as a description of the task in UI.
        /// </summary>
        public abstract string Description { get; }

        public virtual float Progress { get { return 0F; } }
        public virtual bool ShouldDisplayProgress { get { return false; } }

        public virtual void OnStart() { }
        public virtual void OnUpdate() { }
        public virtual void OnComplete() { }
        public virtual void OnCancel() { }
    }
}
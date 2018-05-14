using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public abstract class AnimatedComponent : MonoBehaviour {

		public AnimatedObject Owner { get; private set; }

		public abstract void InitializeComponent();
		
		public void RegisterComponent(AnimatedObject newOwner)
		{
			Owner = newOwner;
			Owner.RegisterListener(AnimatedObject.ListenerType.ON_ACTIVATE, OnActivate);
			Owner.RegisterListener(AnimatedObject.ListenerType.ON_DEACTIVATE, OnDeactivate);
		}

		public abstract void OnActivate();

		public abstract void OnDeactivate();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class AnimatedObject : MonoBehaviour {

		public enum ListenerType
		{
			ON_ACTIVATE,
			ON_DEACTIVATE,
		}

		public bool IsActive { get; private set;}

		[SerializeField]List<AnimatedComponent> m_animatedComponents;

		public delegate void EventListener();

		private EventListener m_onActivationListeners;

		private EventListener m_onDeactivationListeners;
		
		// Use this for initialization
		void Start () {
			IsActive = false;
			if(m_animatedComponents == null || m_animatedComponents.Count == 0)
			{
				Debug.LogWarning("There is 0 component in the list!");

			}
			else
			{
				foreach(AnimatedComponent component in m_animatedComponents)
				{
					if(component != null)
					{
						component.RegisterComponent(this);
					}
					else
					{
						Debug.LogWarning("List contains an empty object!");
					}				
				}
			}		
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void Activate()
		{
			if(IsActive)
				return;
			if(m_onActivationListeners != null)
				m_onActivationListeners();
			IsActive = true;
		}

		public void Deactivate()
		{
			if(!IsActive)
				return;
			if(m_onDeactivationListeners != null)
				m_onDeactivationListeners();
			IsActive = false;
		}

		public void RegisterListener(ListenerType _listenerType, EventListener _newDelegate)
		{
			switch(_listenerType)
			{
				case ListenerType.ON_ACTIVATE:
					m_onActivationListeners += _newDelegate;
					break;
				case ListenerType.ON_DEACTIVATE:
					m_onDeactivationListeners += _newDelegate;
					break; 
			}
		}
	}
}

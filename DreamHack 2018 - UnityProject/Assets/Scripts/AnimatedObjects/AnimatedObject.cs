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

		[SerializeField]List<AnimatedComponent> m_animatedComponents;

		public delegate void EventListener();

		private EventListener m_onActivationListeners;

		private EventListener m_onDeactivationListeners;
		
		// Use this for initialization
		void Start () {
			foreach(AnimatedComponent component in m_animatedComponents)
			{
				component.RegisterComponent(this);
			}			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void Activate()
		{
			if(m_onActivationListeners != null)
				m_onActivationListeners();
		}

		public void Deactivate()
		{			
			if(m_onDeactivationListeners != null)
				m_onDeactivationListeners();
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

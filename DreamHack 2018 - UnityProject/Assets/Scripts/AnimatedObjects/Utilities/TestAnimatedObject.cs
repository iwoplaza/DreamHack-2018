using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

[RequireComponent(typeof(AnimatedObject))]
public class TestAnimatedObject : MonoBehaviour {

	[SerializeField]bool m_active;

	AnimatedObject m_object;


	// Use this for initialization
	void Start () {
		m_active = false;
		m_object = GetComponent<AnimatedObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if(m_active != m_object.IsActive)
		{
			if(m_active)
			{
				m_object.Activate();
			}
			else
			{
				m_object.Deactivate();
			}
		}
	}
}

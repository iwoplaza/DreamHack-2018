using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class AnimatedMovingObjects : AnimatedComponent {

	[SerializeField]Vector3 m_activatedPos;
	[SerializeField]Vector3 m_activatedRot;
	[SerializeField]Vector3 m_deactivatedPos;
	[SerializeField]Vector3 m_deactivatedRot;
	[SerializeField]float m_transitionTime;

	Vector3 m_targetPos;
	Vector3 m_basePos;
	Vector3 m_targetRot;
	Vector3 m_baseRot;
	float m_currentTransitionTime;

	void Reset()
	{
		m_deactivatedPos = transform.localPosition;
		m_deactivatedRot = transform.localRotation.eulerAngles;
		m_transitionTime = 1;
	}

	void Start()
	{
		m_currentTransitionTime = m_transitionTime + 1;
	}

	void Update()
	{
		if(m_currentTransitionTime <= m_transitionTime)
		{
			transform.localRotation = Quaternion.Euler(AnimatedObjectHelper.Vector3CosineInterpolation(m_baseRot,m_targetRot,m_currentTransitionTime/m_transitionTime));
			transform.localPosition = AnimatedObjectHelper.Vector3CosineInterpolation(m_basePos,m_targetPos,m_currentTransitionTime/m_transitionTime);
			m_currentTransitionTime += Time.deltaTime;
		}
	}

	public override void OnActivate()
	{
		m_basePos = transform.localPosition;
		m_baseRot = transform.localRotation.eulerAngles;
		m_targetPos = m_activatedPos;
		m_targetRot = m_activatedRot;
		m_currentTransitionTime = 0;
	}

	public override void OnDeactivate()
	{
		m_basePos = transform.localPosition;
		m_baseRot = transform.localRotation.eulerAngles;
		m_targetPos = m_deactivatedPos;
		m_targetRot = m_deactivatedRot;
		m_currentTransitionTime = 0;
	}
}

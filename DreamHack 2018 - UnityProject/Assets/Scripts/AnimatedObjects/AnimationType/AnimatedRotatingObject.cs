using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class AnimatedRotatingObject : AnimatedComponent {

	[SerializeField]Vector3 m_rotationAxis;
	[SerializeField]float m_maxRotationSpeed;
	[SerializeField]float m_acceleration;

	float m_currentRotationSpeed;
	float m_targetRotationSpeed;
	float m_baseRotationSpeed;
	float m_interpolateTime;
	float m_currentInterpolateTime;

	void Reset()
	{
		m_rotationAxis = Vector3.one;
		m_maxRotationSpeed = 120;
		m_acceleration = 30;
	}

	void Start()
	{
		m_currentInterpolateTime = m_interpolateTime + 1;
	}

	void Update()
	{
		transform.localRotation *= Quaternion.Euler(m_rotationAxis * m_currentRotationSpeed * Time.deltaTime);

		if(m_currentInterpolateTime <= m_interpolateTime)
		{
			m_currentRotationSpeed = AnimatedObjectHelper.CosineInterpolation(m_baseRotationSpeed, m_targetRotationSpeed, m_currentInterpolateTime/m_interpolateTime);
			m_currentInterpolateTime += Time.deltaTime;
		}
	}

	public override void OnActivate()
	{
		m_targetRotationSpeed = m_maxRotationSpeed;
		m_baseRotationSpeed = m_currentRotationSpeed;
		m_interpolateTime = m_targetRotationSpeed/m_acceleration;
		m_currentInterpolateTime = 0;
	}

	public override void OnDeactivate()
	{
		m_targetRotationSpeed = 0;
		m_baseRotationSpeed = m_currentRotationSpeed;
		m_interpolateTime = m_currentRotationSpeed/m_acceleration;
		m_currentInterpolateTime = 0;
	}
}

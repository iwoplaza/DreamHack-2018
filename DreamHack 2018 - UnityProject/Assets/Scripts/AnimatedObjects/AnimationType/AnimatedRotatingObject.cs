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
	float m_interpolateTime;
	float m_currentInterpolateTime;

	void Update()
	{
		transform.localRotation *= Quaternion.Euler(m_rotationAxis * m_currentRotationSpeed * Time.deltaTime);

		if(m_interpolateTime > 0)
		{
			m_currentRotationSpeed = CosineInterpolation(m_currentRotationSpeed, m_targetRotationSpeed, 1 - m_interpolateTime/m_currentInterpolateTime);
			m_interpolateTime -= Time.deltaTime;
		}
	}

	public override void OnActivate()
	{
		m_targetRotationSpeed = m_maxRotationSpeed;
		m_interpolateTime = m_targetRotationSpeed/m_acceleration;
		m_currentInterpolateTime = m_interpolateTime;
	}

	public override void OnDeactivate()
	{
		m_targetRotationSpeed = 0;
		m_interpolateTime = m_currentRotationSpeed/m_acceleration;
		m_currentInterpolateTime = m_interpolateTime;
	}

	public static float CosineInterpolation(float f1, float f2, float t)
	{
		t = Mathf.Clamp01(t);
		float t1 = (1f - Mathf.Cos(t * Mathf.PI))/2f;
		return (f1 * (1f - t1) + (f2 * t1));
	}
}

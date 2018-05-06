using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace Game{
	public class DayNightController : EnvironmentControlComponent {

		[SerializeField]
		[Tooltip("Light intensity is calculated via the alpha channel")]
		Gradient m_SunColorGradient;

		public Light m_sun;

		public float m_sunRotation;

		private TimeSystem m_TimeSystem;
		
		public override void InitializeComponent () {
			m_TimeSystem = Game.WorldController.Instance.MainState.TimeSystem;
		}
		
		public override void UpdateComponent(){
			Color envColor = m_SunColorGradient.Evaluate(m_TimeSystem.dayProgress);
			RenderSettings.ambientLight = envColor;
			m_sun.color = envColor;
			m_sun.intensity = m_SunColorGradient.Evaluate(m_TimeSystem.dayProgress).a;
			m_sun.transform.localRotation = Quaternion.Euler(Mathf.Lerp(-90,270,m_TimeSystem.dayProgress), m_sunRotation, 0);
		}
	}
}

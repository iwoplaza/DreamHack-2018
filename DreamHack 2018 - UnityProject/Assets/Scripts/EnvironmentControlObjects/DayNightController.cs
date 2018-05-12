using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace Game
{
	public class DayNightController : EnvironmentControlComponent
    {
		[Tooltip("Light intensity is calculated via the alpha channel")]
        [SerializeField] Gradient m_sunColorGradient;
		[SerializeField] Gradient m_moonColorGradient;

		[SerializeField] Light m_sun;
		[SerializeField] Light m_moon;

        [SerializeField] float m_sunRotation;

		private TimeSystem m_timeSystem;
		
		public override void InitializeComponent()
        {
			m_timeSystem = Game.WorldController.Instance.MainState.TimeSystem;
		}
		
		public override void UpdateComponent()
        {
			Color envColor = m_sunColorGradient.Evaluate(m_timeSystem.DayProgress);
			RenderSettings.ambientLight = envColor;
			m_sun.color = envColor;
			m_sun.intensity = envColor.a;
			m_sun.transform.localRotation = Quaternion.Euler(Mathf.Lerp(-90,270,m_timeSystem.DayProgress), m_sunRotation, 0);

			Color moonColor = m_moonColorGradient.Evaluate(m_timeSystem.DayProgress);
			m_moon.color = moonColor;
			m_moon.intensity = moonColor.a;
			m_moon.transform.localRotation = Quaternion.Euler(Mathf.Lerp(-90,270,m_timeSystem.DayProgress) + 180, m_sunRotation, 0);
		}
	}
}

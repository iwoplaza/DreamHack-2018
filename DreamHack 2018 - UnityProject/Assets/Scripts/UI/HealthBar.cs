using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        Text m_healthText;

        Slider m_slider;
        HealthComponent m_healthComponent;

        void Awake()
        {
            m_slider = GetComponentInChildren<Slider>();
        }

        public void Bind(HealthComponent healthComponent)
        {
            m_healthComponent = healthComponent;
            m_healthComponent.RegisterChangeHandler(OnHealthChange);
            UpdateView();
        }

        public void Unbind()
        {
            m_healthComponent.UnregisterChangeHandler(OnHealthChange);
            m_healthComponent = null;
        }

        void OnHealthChange(int previousPoints, int newPoints)
        {
            UpdateView();
        }

        void UpdateView()
        {
            if (m_healthComponent == null)
                return;
            m_slider.value = m_healthComponent.PercentageLeft;
            m_healthText.text = Mathf.Floor(m_healthComponent.PercentageLeft * 100) + "%";
        }
    }
}
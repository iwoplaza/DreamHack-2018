using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components
{
    public class TileDisplayComponent : MonoBehaviour
    {
        Material m_material;
        bool m_showNeutralOutline = false;
        bool m_hoverOver = false;

        static Color NEUTRAL_COLOR = new Color(1, 1, 1, 0.1F);
        static Color HOVER_COLOR = new Color(0.6F, 0.8F, 1, 0.7F);

        // Use this for initialization
        void Start()
        {
            gameObject.SetActive(false);

            MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
            if(meshRenderer != null)
            {
                m_material = meshRenderer.material;
                m_material.SetColor("_Tint", NEUTRAL_COLOR);
            }
        }

        public void ShowNeutralOutline(bool show)
        {
            if(show != m_showNeutralOutline)
            {
                m_showNeutralOutline = show;
                if(m_showNeutralOutline)
                {
                    if (!m_hoverOver && m_material != null)
                    {
                        gameObject.SetActive(true);
                        m_material.SetColor("_Tint", NEUTRAL_COLOR);
                    }
                }
                else
                {
                    if (!m_hoverOver && m_material != null)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }

        public void OnMouseEnter()
        {
            m_hoverOver = true;

            if(m_material != null)
            {
                gameObject.SetActive(true);
                m_material.SetColor("_Tint", HOVER_COLOR);
            }
        }

        public void OnMouseExit()
        {
            m_hoverOver = false;

            if (m_material != null)
            {
                if(!m_showNeutralOutline)
                    gameObject.SetActive(false);
                m_material.SetColor("_Tint", NEUTRAL_COLOR);
            }
        }
    }
}
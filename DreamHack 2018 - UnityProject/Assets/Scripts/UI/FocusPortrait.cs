using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class FocusPortrait : MonoBehaviour
    {
        [SerializeField] RawImage m_portraitImage;

        public void Populate(IFocusTarget target)
        {
            Focus focus = WorldController.Instance.MainState.Focus;

            if (target.PortraitPivot != null)
            {
                gameObject.SetActive(true);
                m_portraitImage.texture = focus.PortraitTexture;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
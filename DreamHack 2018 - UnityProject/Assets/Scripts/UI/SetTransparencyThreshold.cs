using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SetTransparencyThreshold : MonoBehaviour {

	Image m_Image;

	// Use this for initialization
	void Start () {
		m_Image = GetComponent<Image>();
		m_Image.alphaHitTestMinimumThreshold = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

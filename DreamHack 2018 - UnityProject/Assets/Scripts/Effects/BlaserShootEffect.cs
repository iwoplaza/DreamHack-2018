using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Effects
{
    public class BlaserShootEffect : MonoBehaviour
    {
        [SerializeField] float m_lifetime;

        void Start()
        {
            StartCoroutine(HandleDeath());
        }

        IEnumerator HandleDeath()
        {
            yield return new WaitForSeconds(m_lifetime);
            Destroy(gameObject);
        }
    }
}
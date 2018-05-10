using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Animation
{
    [RequireComponent(typeof(Worker))]
    public class WorkerVisual : MonoBehaviour
    {

        [SerializeField] GameObject m_handHolder;
        [SerializeField] GameObject m_hipHolder;
        [SerializeField] GameObject m_blaster;

        private Animator m_animator;
        private Quaternion m_initialBlasterRotation;

        public Worker Worker { get; private set; }
        
        void Awake()
        {
            m_animator = GetComponentInChildren<Animator>();
            Worker = GetComponent<Worker>();

            m_initialBlasterRotation = m_blaster.transform.localRotation;
        }

        // Use this for initialization
        void Start()
        {
            if(Worker.IsWeaponOut)
            {
                PutWeaponInHand();
            }
            else
            {
                PutWeaponAtHip();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PutWeaponAtHip()
        {
            m_blaster.transform.parent = m_hipHolder.transform;
            m_blaster.transform.localPosition = new Vector3(0, 0, 0);
            m_blaster.transform.localRotation = m_initialBlasterRotation;
        }

        public void PutWeaponInHand()
        {
            m_blaster.transform.parent = m_handHolder.transform;
            m_blaster.transform.localPosition = new Vector3(0, 0, 0);
            m_blaster.transform.localRotation = m_initialBlasterRotation;
        }

        public void UpdateAnimator()
        {
            m_animator.SetBool("Walk", Worker.IsWalking);
            m_animator.SetBool("WeaponOut", Worker.IsWeaponOut);
        }
    }
}
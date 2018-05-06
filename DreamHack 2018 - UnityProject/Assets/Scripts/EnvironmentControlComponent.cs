using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game{
    public abstract class EnvironmentControlComponent : MonoBehaviour {

        bool m_initialized = false;

        void Start(){
            WorldController.Instance.AddEnvironmentControlObject(this);
        }

        public void Initialize(){
            Debug.Log(this.GetType().ToString() + " initialized.");
            InitializeComponent();
            m_initialized = true;
        }

        public virtual void InitializeComponent(){

        }

        void Update(){
            if(m_initialized){
                UpdateComponent();
            }
        }

        public virtual void UpdateComponent(){

        }
    }
}

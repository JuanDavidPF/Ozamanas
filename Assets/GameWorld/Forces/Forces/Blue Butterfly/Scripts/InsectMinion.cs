using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using DG.Tweening;
using Ozamanas.Board;
using UnityEngine.Events;
using Ozamanas.Machines;

namespace Ozamanas.Forces
{   
     [RequireComponent(typeof(Rigidbody))]
     public class InsectMinion : MonoBehaviour
    {
       private GameObject objective;
       private List<MachineTrait> traits;
        private Rigidbody rg;
        [SerializeField] private float m_Speed = 0.01f;

        [SerializeField] private float lifeSpan = 20f;
        public GameObject Objective { get => objective; set => objective = value; }
        public List<MachineTrait> Traits { get => traits; set => traits = value; }

        [SerializeField] public UnityEvent OnMinionDestruction;
        void Start()
        {
            rg = GetComponent<Rigidbody>();

           Destroy(gameObject,lifeSpan);
        }

        void FixedUpdate()
        {
           if (objective==null) return;

            Vector3 heading = objective.transform.position - transform.position;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;   

             

        // the second argument, upwards, defaults to Vector3.up
            Quaternion rotation = Quaternion.LookRotation(heading, Vector3.up);
            transform.rotation = rotation;
            rg.MovePosition(transform.position+(direction*m_Speed*Time.deltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.tag != "Machine") return;

            if (other.TryGetComponentInParent(out Machines.HumanMachine machine))
            {
                foreach(MachineTrait trait in traits)
                {
                    machine.AddTraitToMachine(trait);
                }
                
                OnMinionDestruction?.Invoke();
                Destroy(gameObject);
            }
            else return;

          
        }//Closes OnTriggerEnter method

    }
}

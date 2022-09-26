using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using DG.Tweening;
using Ozamanas.Board;
using Ozamanas.Machines;

namespace Ozamanas.Forces
{
     [RequireComponent(typeof(Rigidbody))]
    public class TotemMinion : MonoBehaviour
    {


        private List<MachineTrait> traits;
        [SerializeField] private float m_Speed = 2f;
        [SerializeField] private float lifeSpan = 20f;
        private Rigidbody rg;

        // Start is called before the first frame update    void Start()
         void Start()
        {
            rg = GetComponent<Rigidbody>();
            Destroy(gameObject,lifeSpan);
        }

        void FixedUpdate()
        {
           if (traits==null) return;
           rg.MovePosition(transform.position+(Vector3.down*m_Speed*Time.deltaTime));
        }

         private void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
            if (other.tag != "Cell") return;

            if (other.TryGetComponentInParent(out Cell cell))
            {
               // cell.ActiveTraits.AddRange(traits);
            }
            else return;
        }//Closes OnTriggerEnter method

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ozamanas.Tags;
using DG.Tweening;

namespace Ozamanas.Board
{

    [RequireComponent(typeof(Collider))]
    public class JungleTree : MonoBehaviour
    {
            [SerializeField] private float lifetime = 5f;
            [SerializeField] private float growingTime = 2f;
            [SerializeField] private GameObject fragmentedModel;
            private bool alreadyTriggered = false;
            private Collider cd;
            [SerializeField] public UnityEvent OnDestruction;


        private void Awake()
            {
                cd = GetComponent<Collider>();
                
            }//Closes Awake method
            
            private void OnTriggerEnter(Collider other)
            {
                if ( other.transform.tag != "Machine") return;

                DestroyTree();
            }
            
            public void DestroyTree()
            {
                if(alreadyTriggered) return;
                alreadyTriggered = true;
                OnDestruction?.Invoke();
                gameObject.SetActive(false);
                GameObject dummy = Instantiate(fragmentedModel,transform.position,transform.rotation);
                if(dummy) Destroy(dummy,lifetime);

            }
           

    }
}

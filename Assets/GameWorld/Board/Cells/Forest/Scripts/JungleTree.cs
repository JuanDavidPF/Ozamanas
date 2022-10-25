using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ozamanas.Tags;
using DG.Tweening;

namespace Ozamanas.Forest
{

    [RequireComponent(typeof(Collider))]
    public class JungleTree : MonoBehaviour
    {
            [SerializeField] private float lifetime = 3f;
            [SerializeField] private float growingTime = 10f;
            [SerializeField] private GameObject fragmentedModel;
            private bool alreadyTriggered = false;
            private Collider cd;
            [SerializeField] public UnityEvent OnDestruction;

            private int forestIndex = 0;

            private ForestBehaviour forestBehaviour;

            public int ForestIndex { get => forestIndex; set => forestIndex = value; }

        private void Awake()
            {
                cd = GetComponent<Collider>();

                forestBehaviour = GetComponentInParent<ForestBehaviour>();
                
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
                forestBehaviour.SetTrunk(forestIndex);
                OnDestruction?.Invoke();
                gameObject.SetActive(false);
                GameObject dummy = Instantiate(fragmentedModel,transform.position,transform.rotation);
                if(dummy) Destroy(dummy,lifetime);

            }

            public void HideAndDestroy()
            {
                gameObject.transform.transform.DOScaleY(0f, growingTime).From(1);
                Destroy(gameObject,growingTime);
            }
           

    }
}

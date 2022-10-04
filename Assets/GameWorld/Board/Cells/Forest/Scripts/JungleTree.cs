using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ozamanas.Tags;
using DG.Tweening;

namespace Ozamanas.Board
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class JungleTree : MonoBehaviour
    {
            [SerializeField] private float lifetime = 5f;

            [SerializeField] private float growingTime = 2f;
            [SerializeField] private GameObject fragmentedModel;
            [SerializeField] private GameObject expansionTree;
            [SerializeField] private GameObject forestTree;
            [SerializeField] private TreeType tree_type = TreeType.Tree; 
            [SerializeField] private TreeState tree_status = TreeState.Expose; 

            private bool alreadyTriggered;
            private Rigidbody rb;
            private Collider cd;

            private MeshRenderer rd;

            [SerializeField] public UnityEvent OnDestruction;

        public TreeType Tree_type { get => tree_type; set => tree_type = value; }

        private void Awake()
            {
                rb = GetComponent<Rigidbody>();
                cd = GetComponent<Collider>();
                rd = GetComponent<MeshRenderer>();
            }//Closes Awake method
            private void OnCollisionEnter(Collision other)
            {

                if ( other.transform.tag != "Machine") return;

                DestroyTree();



  
            }//Closes OnColissionEnter method


            
            public void DestroyTree()
            {
                if(alreadyTriggered) return;
                alreadyTriggered = true;
                OnDestruction?.Invoke();
                gameObject.SetActive(false);
                GameObject dummy = Instantiate(fragmentedModel,transform.position,transform.rotation);
                if(dummy) Destroy(dummy,lifetime);

            }
            public void ChangeTreeToExpansion(bool expansion)
            {
                if(!expansionTree || !forestTree) return;
                
                if(expansion) {
                     forestTree.SetActive(false);
                    expansionTree.SetActive(true);
                   

                }
                else {
                    expansionTree.SetActive(false);
                    forestTree.SetActive(true);

                }

            }

            public void Hide()
            {
                if (tree_status != TreeState.Expose) return;

                transform.DOScaleY(1f,growingTime).From(0);
                tree_status = TreeState.Hidden;
             
            }

            public void Show()
            {
                if (tree_status != TreeState.Hidden) return;

                transform.DOScaleY(0f,growingTime).From(1);
                tree_status = TreeState.Expose;
             
            }

    }
}

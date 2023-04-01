using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ozamanas.Tags;
using Ozamanas.Board;
using Ozamanas.Extenders;
using System;

namespace Ozamanas.Forest
{
    public class Mountain : MonoBehaviour
    {
        [Serializable]
        public struct SwapRules
        {
            public CellData condition;
            public CellTopElement topElementToSwap;
            public CellData tokenToSwap;

        }
        [SerializeField] private List<SwapRules> ruleList = new List<SwapRules>();
        [SerializeField] private float lifetime = 1f;
        [SerializeField] private float fragmentedModelLifetime = 2f;
        [SerializeField] private GameObject fragmentedModel;

        private Cell currentCell;

        [SerializeField] public UnityEvent OnDestruction;

        private bool alreadyTriggered = false;

        private GameObject dummy;


        private void Start()
        {

            currentCell = GetComponentInParent<Cell>();

            if( lifetime > 0) Invoke("DestroyMountain",lifetime);

            
        }

        // Start is called before the first frame update

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag != "Machine") return;

  
            if(other.transform.TryGetComponentInParent(out Machines.HumanMachine machine)) 
            {
                        
                if(machine.GetMachineType() == MachineHierarchy.Destructor ) DestroyMountain();
            }
            
        }

        public void DestroyMountain()
        {
            if (alreadyTriggered) return;

            alreadyTriggered = true;
            
            OnDestruction?.Invoke();

            dummy = fragmentedModel ? Instantiate(fragmentedModel, transform.position, transform.rotation) : null;
            
            Invoke("RemoveAllMeshColliders", fragmentedModelLifetime - 0.5f);
            Destroy(dummy, fragmentedModelLifetime);
           
            

            currentCell.CurrentTopElement = GetTopElementToSwap(currentCell);
            currentCell.data = GetTokenToSwap(currentCell);
        }

        private void RemoveAllMeshColliders()
        {
            MeshCollider[] allMeshColliders = dummy.GetComponentsInChildren<MeshCollider>();
            foreach (MeshCollider meshCollider in allMeshColliders) 
            {
                meshCollider.enabled = false;
            }
        }

        private CellTopElement GetTopElementToSwap(Cell cell)
        {
            SwapRules expRule = ruleList.Find(rule => rule.condition == cell.data);

            if (!expRule.topElementToSwap) return null;

            return expRule.topElementToSwap;
        }

        private CellData GetTokenToSwap(Cell cell)
        {
            SwapRules expRule = ruleList.Find(rule => rule.condition == cell.data);

            if (!expRule.tokenToSwap) return null;

            return expRule.tokenToSwap;
        }

    }
}
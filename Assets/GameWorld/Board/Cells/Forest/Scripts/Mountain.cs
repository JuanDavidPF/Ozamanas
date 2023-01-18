using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ozamanas.Tags;
using Ozamanas.Board;
using Ozamanas.Extenders;

namespace Ozamanas.Forest
{
    public class Mountain : MonoBehaviour
    {
        
        [SerializeField] private float lifetime = 1f;
        [SerializeField] private float fragmentedModelLifetime = 2f;
        [SerializeField] private GameObject fragmentedModel;
        [SerializeField] private CellData forestId;

        private Cell currentCell;

        [SerializeField] public UnityEvent OnDestruction;

        private bool alreadyTriggered = false;

        public Cell CurrentCell { get => currentCell; set => currentCell = value; }


        private void Start()
        {
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
            currentCell.data = forestId;
            gameObject.SetActive(false);
            GameObject dummy = fragmentedModel ? Instantiate(fragmentedModel, transform.position, transform.rotation) : null;
            if (dummy) Destroy(dummy, fragmentedModelLifetime);
            return;
        }
    }
}

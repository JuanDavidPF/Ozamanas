using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using Ozamanas.Board;


namespace Ozamanas.Machines
{
    public class MachineBox : MonoBehaviour
    {   
        [SerializeField] private GameObject VFXBoxDestruction;
        private Rigidbody rb;
        private MachineSpawner machineSpawner;
       private void Awake()
        {
             rb = GetComponent<Rigidbody>();
             machineSpawner = GetComponent<MachineSpawner>();
        }

         private void Update()
        {
            //Fall to void
            if (transform.position.y <= -1) DestroyMachineBox();
           
        }


        private void DestroyMachineBox()
        {
            GameObject temp = Instantiate(VFXBoxDestruction,transform);
            temp.transform.parent = null;
            Destroy(gameObject);
        }
        private void FixedUpdate()
        {

            if (!rb || rb.isKinematic ) return;

            if (rb.IsSleeping()) SpawnMachine();
        }

        private void SpawnMachine()
        {
            machineSpawner.SpawnNextMachineOnQueue();
            DestroyMachineBox();
        }

         void OnCollisionEnter(Collision collision)
        {
             if (collision.gameObject.tag != "Cell") return;

             if (collision.gameObject.transform.TryGetComponentInParent(out Cell cell))
            {
                cell.SetOnMachineEnter(null);
            }


        }

          void OnCollisionExit(Collision collision)
        {
             if (collision.gameObject.tag != "Cell") return;

             if (collision.gameObject.transform.TryGetComponentInParent(out Cell cell))
            {
                cell.SetOnMachineExit(null);
            }


        }

    }
}

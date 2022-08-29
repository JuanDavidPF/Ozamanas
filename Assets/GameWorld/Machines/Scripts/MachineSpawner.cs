using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ozamanas.Machines
{

    public class MachineSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> spawnQueue = new List<GameObject>();


        [ContextMenu("Spawn next machine")]
        public void SpawnNextMachineOnQueue()
        {
            if (spawnQueue.Count == 0) return;

            GameObject nextMachine = spawnQueue[0];
            spawnQueue.RemoveAt(0);

            if (nextMachine) Instantiate(nextMachine, transform.position, transform.rotation);

        }//Closes SpawnNextMachineOnQueue method


    }//Closes MachineSpawner class

}//Closes Namespace declaration
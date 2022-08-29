using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ozamanas.Machines
{

    public class MachineSpawner : MonoBehaviour

    {
        [HideInInspector] public GameObject _go;
        [HideInInspector] public Transform _t;

        [SerializeField] private List<HumanMachine> spawnQueue = new List<HumanMachine>();

        private void Awake()
        {
            _go = gameObject;
            _t = _go.transform;
        }//Closes Awake method


        [ContextMenu("Spawn next machine")]
        public void SpawnNextMachineOnQueue()
        {
            if (spawnQueue.Count == 0) return;

            GameObject nextMachine = spawnQueue[0].gameObject;
            spawnQueue.RemoveAt(0);

            if (nextMachine) Instantiate(nextMachine, _t.position, _t.rotation);

        }//Closes SpawnNextMachineOnQueue method


    }//Closes MachineSpawner class

}//Closes Namespace declaration
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Levels;
using Ozamanas.Board;

namespace Ozamanas.Machines
{

    public class MachineSpawner : MonoBehaviour

    {
        [HideInInspector] public GameObject _go;
        [HideInInspector] public Transform _t;

        [SerializeField] private float spawnDelay = 0f;
        [SerializeField] private List<HumanMachineToken> spawnQueue = new List<HumanMachineToken>();
       


        private void Awake()
        {
            _go = gameObject;
            _t = _go.transform;


        }//Closes Awake method
        

        [ContextMenu("Spawn next machine")]
        public void SpawnNextMachineOnQueue()
        {
            if (spawnQueue.Count == 0 || !spawnQueue[0]) return;

            Invoke("SpawnMachineOnQueue",spawnDelay);
        }

        private void SpawnMachineOnQueue()
        {
            HumanMachine temp = Instantiate(spawnQueue[0].machinePrefab, _t.position, _t.rotation).GetComponent<HumanMachine>();
            temp.Machine_token = spawnQueue[0];
            spawnQueue.RemoveAt(0);
        }


    }//Closes MachineSpawner class

}//Closes Namespace declaration
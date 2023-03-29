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
         [SerializeField] private bool autoPopulateSpawnerList = true;

          [SerializeField] private float spawnDelay = 0f;
        [SerializeField] private List<HumanMachineToken> spawnQueue = new List<HumanMachineToken>();
       

        private CellData cellData;

        private void Awake()
        {
            _go = gameObject;
            _t = _go.transform;

            cellData = GetComponent<Cell>().data;

            if(autoPopulateSpawnerList) PopulateSpawnerList();
        }//Closes Awake method
        
        private void PopulateSpawnerList()
        {
            if (!cellData) return;
            
            LevelHolder temp = FindObjectOfType<LevelHolder>();

            if (!temp) return;

            LevelData levelData = temp.levelSelected.level;

            spawnQueue = new List<HumanMachineToken>();

            foreach (HumanMachineToken machine in levelData.machines)
            {
                if(machine.humanBase == cellData)
                spawnQueue.Add(machine);
            }
        }

        [ContextMenu("Spawn next machine")]
        public void SpawnNextMachineOnQueue()
        {
            if (spawnQueue.Count == 0 || !spawnQueue[0]) return;

            Invoke("SpawnMachineOnQueue",spawnDelay);
        }

        private void SpawnMachineOnQueue()
        {
            Instantiate(spawnQueue[0].machinePrefab, _t.position, _t.rotation);

            spawnQueue.RemoveAt(0);
        }


    }//Closes MachineSpawner class

}//Closes Namespace declaration
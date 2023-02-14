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
        [SerializeField] private List<HumanMachine> spawnQueue = new List<HumanMachine>();
       

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

            spawnQueue = new List<HumanMachine>();

            foreach (HumanMachine machine in levelData.machines)
            {
                if(machine.Machine_token.humanBase == cellData)
                spawnQueue.Add(machine);
            }
        }

        [ContextMenu("Spawn next machine")]
        public void SpawnNextMachineOnQueue()
        {
            if (spawnQueue.Count == 0 || !spawnQueue[0]) return;

            Instantiate(spawnQueue[0], _t.position, _t.rotation);

            spawnQueue.RemoveAt(0);
        }//Closes SpawnNextMachineOnQueue method


    }//Closes MachineSpawner class

}//Closes Namespace declaration
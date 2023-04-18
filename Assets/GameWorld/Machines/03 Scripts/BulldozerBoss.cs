using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Board;

namespace Ozamanas.Machines
{
    public class BulldozerBoss  : HumanMachine
    {
        private MachineSpawner machineSpawner;
        protected override void Awake()
        {
            base.Awake();
            machineSpawner = GetComponent<MachineSpawner>();
            transform.parent = null;
        }
       public void DestroyForest()
       {
            if(!CurrentCell) return;

            List<CellData> cellsData = new List<CellData>();

            foreach (SwapRules rule in machine_token.ruleList)
            {
                cellsData.Add(rule.condition);
            }

            CellData[] cellsArray = cellsData.ToArray();

            List<Cell> cells = new List<Cell>();

            cells = Board.Board.GetNearestsCellInRange(CurrentCell.transform.position,1,cellsArray);

            foreach( Cell cell in cells)
            {
                cell.data = machine_token.GetTokenToSwap(cell);
                cell.CurrentTopElement = machine_token.GetTopElementToSwap(cell);
            }
       }

       public void SpawnMachine()
       {
            if(!machineSpawner) return;

            machineSpawner.SpawnNextMachineOnQueue();
       }
    }
}

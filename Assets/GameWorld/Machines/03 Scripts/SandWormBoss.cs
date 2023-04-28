using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Board;

namespace Ozamanas.Machines
{
    public class SandWormBoss : HumanMachine
    {   
        [SerializeField] private CellDemolisher cellDemolisher;

        [SerializeField] private List<CellData> blacklist = new List<CellData>();

        [SerializeField] private int destroyCellsLimit = 3; 
        [SerializeField] private float cellLimit = 2; 

        private List<Cell> currentCells = new List<Cell>();
        private bool isCrawling = false;

        public bool IsCrawling { get => isCrawling; set => isCrawling = value; }

        private WormMovement wormMovement;

        protected override void Start()
        {
            base.Start();
            wormMovement = GetComponent<WormMovement>();
            transform.parent = null;
            
        }

        void Update()
        {
            if(wormMovement.Cart.m_Position >= cellLimit) 
            {
                IsCrawling = false;
                cellLimit += cellLimit;
            }
            
            if(IsCrawling) return;

            Crawling();
            
        }
        private void Crawling()
        {
            IsCrawling = true;

            SetRunningStatus();

            if(MachineMovement.CheckIfMachineIsBlocked())
            {
                SetBlockedStatus();
                return;
            }
        
            MachineMovement.SetCurrentDestination();

        } 

        public void AddCurrentCell(Cell cell)
        {
            if(blacklist.Contains(cell.data)) return;
            
            if(currentCells.Contains(cell)) return;

            currentCells.Add(cell);

            if(currentCells.Count > destroyCellsLimit ) 
            {
                SpawnCellDemolisher(currentCells[0]);
                currentCells.RemoveAt(0);
            }
        }
        public void SpawnCellDemolisher(Cell cell)
        {
            if(!cellDemolisher) return;

            GameObject temp = Instantiate(cellDemolisher.gameObject,cell.transform.position,Quaternion.identity);
            temp.GetComponent<CellDemolisher>().SpawnReplacement(cell);

        }
        
    }
}

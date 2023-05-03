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
        public WormMovement WormMovement { get => wormMovement; set => wormMovement = value; }
        public WormPhysics WormPhysics { get => wormPhysics; set => wormPhysics = value; }

        private WormMovement wormMovement;
        private WormPhysics wormPhysics;


        protected override void Start()
        {
            base.Start();
            WormMovement = GetComponent<WormMovement>();
            WormPhysics = GetComponent<WormPhysics>();
            transform.parent = null;
            PlayGoingUPAnim();
            
        }

        void Update()
        {
            if(WormMovement.Cart.m_Position >= cellLimit) 
            {
                CheckIfPathChanged();
                cellLimit += cellLimit;
            }
            
            if(!IsCrawling) Crawling();
        }

        private void CheckIfPathChanged()
        {
            if(!WormMovement.CheckIfPathChanged()) return;

            IsCrawling = false;
            
        }

        public void PlayGoingUPAnim()
        {
            animator.SetTrigger("GoingUP");
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

            WormMovement.RemoveCellFromPath(cell);
            GameObject temp = Instantiate(cellDemolisher.gameObject,cell.transform.position,Quaternion.identity);
            temp.GetComponent<CellDemolisher>().SpawnReplacement(cell);

        }

        protected override void OnDestroy()
        {
            foreach( Cell cell in currentCells) SpawnCellDemolisher(cell);
            base.OnDestroy();
            
        }
        
    }
}

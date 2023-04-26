using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Board;
using DG.Tweening;

namespace Ozamanas.Machines
{
    public class BulldozerBoss  : HumanMachine
    {
        private MachineSpawner machineSpawner;
         [SerializeField] private GameObject VFXAttack;
        [SerializeField] private GameObject machineBox;

        [SerializeField] private int tilesToReleaseAttack = 2;
        [SerializeField] private int attackRange = 1;
        private int counter =0;

        [SerializeField] private float power;
         [SerializeField] private float upwardsModifier;
        protected override void Awake()
        {
            base.Awake();
            machineSpawner = GetComponent<MachineSpawner>();
            transform.parent = null;

        }

        public bool IsTimeToAttack()
        {
            counter++;

            return counter >= tilesToReleaseAttack;
        }
       public void DestroyForest()
       {
            if(!CurrentCell) return;

            Instantiate(VFXAttack,transform);

            List<Cell> cells = new List<Cell>();

            cells = Board.BoardExtender.GetCellsOnRange(CurrentCell,attackRange,false);

            foreach( Cell cell in cells)
            {
                cell.data = machine_token.GetTokenToSwap(cell);
                cell.CurrentTopElement = machine_token.GetTopElementToSwap(cell);
            }

            ResetAttackCounter();
       }

       public void SpawnMachine()
       {
            GameObject temp = Instantiate(machineBox,transform);
            temp.transform.parent = null;
            Rigidbody rb = temp.GetComponent<Rigidbody>();
            rb.AddExplosionForce(power, transform.position, 10,upwardsModifier,ForceMode.Impulse);

            ResetAttackCounter();
       }

       private void ResetAttackCounter()
       {
            counter =0;
       }
    }
}

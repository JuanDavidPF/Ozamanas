using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using DG.Tweening;
using Ozamanas.Board;
using Ozamanas.Tags;
using Ozamanas.Machines;

namespace Ozamanas.Forces
{

    [RequireComponent(typeof(Animator))]
    public class MultipleOffensiveBird : AncientForce
    {


        [Space(15)]
        [Header("Visuals Setup")]

        [SerializeField] private Transform birdPosition;
        [SerializeField] private GameObject thunderVFX;
        private Animator animator;
        [SerializeField] private float waitingTime = 0.2f;

        [Space(15)]
        [Header("Attack Setup")]
        [Range(1, 7)]
        [SerializeField] private int totalThunders;
        [SerializeField] private int damageAmount;
        [SerializeField] private float posY = 0f;

        private List<Cell> shuffleList = new List<Cell>();

        protected override void Awake()
        {
            base.Awake();
            animator = gameObject.GetComponent<Animator>();
            

        }//Closes Awake method



        protected override void FinalPlacement()
        {

            base.FinalPlacement();

            if (!isPlaced) return;

            animator.SetTrigger("Release");

            birdPosition.position = new Vector3(birdPosition.position.x,posY,birdPosition.position.z);

            shuffleList.AddRange(Board.BoardExtender.GetCellsOnRange(cellOnAttack,data.attackRange.value-1,true));
            shuffleList.Shuffle(shuffleList.Count);
            StartCoroutine(InstantiateThunder());

        }//Closes FirstPlacement method

        IEnumerator InstantiateThunder()
        {
            for (int i = 0; i < totalThunders; i++)
            {
                yield return new WaitForSeconds(waitingTime);

                if(i>= shuffleList.Count) continue;

                Instantiate(thunderVFX, shuffleList[i].transform.position, Quaternion.identity);

                for (int x = shuffleList[i].CurrentHumanMachines.Count - 1; x > -1; x--)
                {
                    AttackMachine(shuffleList[i].CurrentHumanMachines[x].GetComponent<MachineArmor>());
                }

            }

            base.DestroyForce();
        }


        private void AttackMachine(Machines.MachineArmor machine)
        {
            if (!machine || machine.tag != "Machine") return;

             if (machine.TryGetComponent(out MachineMovement movement))
             {
                if(movement.CurrentAltitude != MachineAltitude.Terrestrial) return;
             }

              if (machine.TryGetComponent(out Machines.HumanMachine h_machine))
            {
                h_machine.SetMachineStatus(MachineState.Attacked);
            }

            if (machine.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                if(physics.state == PhysicMode.Kinematic) return;
                machine.TakeDamage(damageAmount);
                physics.AddForceToMachine(data.physicsForce,transform.position);
            }

           

        }//Closes AttemptMachineDamage method







    }
}

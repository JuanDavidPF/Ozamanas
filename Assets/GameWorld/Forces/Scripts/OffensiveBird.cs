using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using DG.Tweening;
using Ozamanas.Board;
using Ozamanas.Tags;

namespace Ozamanas.Forces
{

    [RequireComponent(typeof(Animator))]
    public class OffensiveBird : AncientForce
    {

        private enum OffensiveMode
        {
            Hitscan,
            Projectile
        }



        private Transform AOETransform;
        List<Machines.MachineArmor> machinesAffected = new List<Machines.MachineArmor>();
        private Tween birdTween;

        private Animator animator;
        [SerializeField] private OffensiveMode mode;
        [SerializeField] private float roosterSpeed = 2f;
        [SerializeField] private int damageAmount;
        [SerializeField] private MeshRenderer AOERenderer;
        [SerializeField] private Transform visuals;


        protected override void Awake()
        {
            base.Awake();
            if (AOERenderer) AOETransform = AOERenderer.transform;

            animator = gameObject.GetComponent<Animator>();

            AOETransform.position = AOETransform.position- data.draggedOffset;

        }//Closes Awake method



        protected override void FinalPlacement()
        {

            base.FinalPlacement();

            if (!isPlaced) return;
            
            visuals.gameObject.SetActive(false);
            AOERenderer.gameObject.SetActive(false);

            birdTween = transform.DOMoveY(0, roosterSpeed, false).SetSpeedBased();

            if (mode == OffensiveMode.Hitscan)
                birdTween.OnComplete(() =>
                {
                    visuals.gameObject.SetActive(true);
                    animator.SetTrigger("OnRelease");
                    ActivateTraits(Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid()));
                    foreach (var machine in machinesAffected.ToArray())
                    {
                        if (!machine) continue;
                        AttackMachine(machine);
                    }
                });

        }//Closes FirstPlacement method



        private void OnTriggerEnter(Collider other)
        {

            if (isPlaced || other.tag != "Machine") return;

            if (other.TryGetComponentInParent(out Machines.MachineArmor armor))
            {
                if (!machinesAffected.Contains(armor)) machinesAffected.Add(armor);

            }
            else return;

            UpdateAOEColor();
        }//Closes OnTriggerEnter method

        private void OnTriggerExit(Collider other)
        {

            if (isPlaced || other.tag != "Machine") return;

            if (other.TryGetComponentInParent(out Machines.MachineArmor armor))
                machinesAffected.Remove(armor);
            else return;
            UpdateAOEColor();

        }//Closes OnTriggerExit method

        private void UpdateAOEColor()
        {
            if (!AOERenderer) return;
            if (machinesAffected.Count == 0) AOERenderer.material.color = new Vector4(1f,0f,0f,.6f);
            else AOERenderer.material.color = new Vector4(0f,1f,0f,.6f);;
        }//Closes UpdateAOEColor method

         void OnDestroy()
        {
            if (birdTween != null) birdTween.Kill();
        }//Closes OnDestroy method


        private void OnCollisionEnter(Collision other)
        {
            if (mode != OffensiveMode.Projectile) return;

            if (!isPlaced || (other.transform.tag != "Machine" && other.transform.tag != "Cell")) return;

            if (other.transform.TryGetComponent(out Machines.MachineArmor machine))
            {
                AttackMachine(machine);
            }

        }//Closes OnCollisionEnter method

        private void ActivateTraits(Cell origin)
        {
            if (!origin) return;

            foreach (var cell in origin.GetCellsOnRange(data.traitRange))
            {
                if (!cell) continue;

                foreach (var trait in data.traits)
                {
                    if (!trait) continue;
                    cell.AddTraitToMachine(trait);
                }
            }


        }//Closes ActivateTraits method

        private void AttackMachine(Machines.MachineArmor machine)
        {
            if (!machine || machine.tag != "Machine") return;

            machine.TakeDamage(damageAmount);
            machinesAffected.Remove(machine);

            if (machine.TryGetComponent(out Machines.HumanMachine h_machine))
            {
                h_machine.SetMachineStatus(MachineState.Attacked);
            }

            if (machine.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                physics.AddForceToMachine(data.physicsForce,transform.position);
            }

            

        }//Closes AttemptMachineDamage method

        public override void DestroyForce()
        {
            base.DestroyForce();
        }




    }//Closes OffensiveBird class
}//closes Namespace declaration

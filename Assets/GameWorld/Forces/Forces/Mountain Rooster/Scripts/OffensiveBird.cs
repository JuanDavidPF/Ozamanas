using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using DG.Tweening;
using Ozamanas.Board;

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
        private Tween roosterTween;

        private Animator animator;
        [SerializeField] private OffensiveMode mode;
        [SerializeField] private float roosterSpeed = 2f;
        [SerializeField] private int damageAmount;
        [SerializeField] private Vector3 elevationForce;
        [SerializeField] private Vector3 torqueForce;
        [SerializeField] private MeshRenderer AOERenderer;

        protected override void Awake()
        {
            base.Awake();
            if (AOERenderer) AOETransform = AOERenderer.transform;
            animator=gameObject.GetComponent<Animator>();
        }//Closes Awake method


        public override void FirstPlacement()
        {
            animator.SetTrigger("Release");
            base.FirstPlacement();

            AOERenderer.gameObject.SetActive(false);
            roosterTween = transform.DOMoveY(0, roosterSpeed, false).SetSpeedBased();

            if (mode == OffensiveMode.Hitscan)
                roosterTween.OnComplete(() =>
                {
                    animator.SetTrigger("Attack");
                    ActivateTraits(Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid()));
                    foreach (var machine in machinesAffected.ToArray())
                    {
                        if (!machine) continue;
                        AttackMachine(machine);
                    }

                    Destroy(gameObject);
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
            if (machinesAffected.Count == 0) AOERenderer.material.color = Color.red;
            else AOERenderer.material.color = Color.green;
        }//Closes UpdateAOEColor method




        private void Update()
        {
            if (isPlaced || !AOETransform) return;
            Ray AOERay = new Ray(transform.position, -transform.up);


            foreach (var hit in Physics.RaycastAll(AOERay))
            {
                if (hit.transform.tag != "Cell") return;
                AOETransform.position = hit.point;
                break;
            }

        }//Closes Update Method

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (roosterTween != null) roosterTween.Kill();
        }//Closes OnDestroy method


        private void OnCollisionEnter(Collision other)
        {
            if (mode != OffensiveMode.Projectile) return;

            if (!isPlaced || (other.transform.tag != "Machine" && other.transform.tag != "Cell")) return;

            animator.SetTrigger("Attack");
            if (other.transform.TryGetComponent(out Machines.MachineArmor machine))
            {
                AttackMachine(machine);
            }

            Destroy(gameObject);
        }//Closes OnCollisionEnter method

        private void ActivateTraits(Cell origin)
        {
            if (!origin) return;

            foreach (var cell in origin.GetCellsOnRange(traitRange))
            {
                if (!cell) continue;

                foreach (var trait in traits)
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



            if (machine.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                physics.SetPhysical();
                if (physics.rb)
                {

                    physics.rb.AddForce(machine.transform.up + elevationForce, ForceMode.Impulse);
                    physics.rb.AddTorque(torqueForce, ForceMode.Impulse);
                }
            }

        }//Closes AttemptMachineDamage method






    }//Closes OffensiveBird class
}//closes Namespace declaration

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using DG.Tweening;
using Ozamanas.Board;

namespace Ozamanas.Forces
{
    public class AddForceMammal : AncientForce
    {
        private enum MammalForceMode
        {
            Push,
            Pull
        }

        private Transform AOETransform;
        [SerializeField] private MeshRenderer AOERenderer;
        [SerializeField] private MammalForceMode mode;
        List<Machines.MachineArmor> machinesAffected = new List<Machines.MachineArmor>();

        [SerializeField] private Vector3 elevationForce;
        [SerializeField] private Vector3 torqueForce;

        [SerializeField] private float mammalSpeed = 2f;
        [Range(100, 1000)]
        [SerializeField] private int mammalForce = 1000;
        private Tween mammalTween;


        [SerializeField] private GameObject pullArrows;
        [SerializeField] private GameObject pushArrows;
        protected override void Awake()
        {
            base.Awake();
            if (AOERenderer) AOETransform = AOERenderer.transform;

            if (mode == MammalForceMode.Push) pushArrows.SetActive(true);
            else pullArrows.SetActive(true);
        }//Closes Awake method

        public override void FirstPlacement()
        {

            base.FirstPlacement();

            AOERenderer.gameObject.SetActive(false);
            mammalTween = transform.DOMoveY(0, mammalSpeed, false).SetSpeedBased();


            mammalTween.OnComplete(() =>
            {
                ActivateTraits(Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid()));
                foreach (var machine in machinesAffected.ToArray())
                {
                    if (!machine) continue;
                    AddForceToMachine(machine);
                }

                Destroy(gameObject);
            });

        }//Closes FirstPlacement method

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

        private void AddForceToMachine(Machines.MachineArmor machine)
        {
            if (!machine || machine.tag != "Machine") return;

            machinesAffected.Remove(machine);

            if (machine.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                physics.SetPhysical();
                if (physics.rb)
                {
                    Vector3 force = machine.transform.position - gameObject.transform.position;
                    force = force.normalized * mammalForce;
                    if (mode == MammalForceMode.Pull) force = force * -1;
                    physics.rb.AddForce(force + elevationForce, ForceMode.Impulse);
                    physics.rb.AddTorque(torqueForce, ForceMode.Impulse);
                }
            }

        }//Closes AttemptMachineDamage method

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

        private void UpdateAOEColor()
        {
            if (!AOERenderer) return;
            if (machinesAffected.Count == 0) AOERenderer.material.color = Color.blue;
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
            if (mammalTween != null) mammalTween.Kill();
        }//Closes OnDestroy method


    }



}

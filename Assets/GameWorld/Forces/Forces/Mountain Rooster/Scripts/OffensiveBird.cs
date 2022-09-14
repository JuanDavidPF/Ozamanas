using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using DG.Tweening;
namespace Ozamanas.Forces
{


    public class OffensiveBird : AncientForce
    {


        private Transform AOETransform;

        List<Machines.MachineArmor> machinesAffected = new List<Machines.MachineArmor>();
        private Tween roosterTween;


        [SerializeField] private float roosterSpeed = 2f;
        [SerializeField] private int damageAmount;
        [SerializeField] private Vector3 elevationForce;
        [SerializeField] private Vector3 torqueForce;
        [SerializeField] private MeshRenderer AOERenderer;

        protected override void Awake()
        {
            base.Awake();
            if (AOERenderer) AOETransform = AOERenderer.transform;
        }//Closes Awake method


        public override void FirstPlacement()
        {

            base.FirstPlacement();

            AOERenderer.gameObject.SetActive(false);
            roosterTween = transform.DOMoveY(0, roosterSpeed, false).SetSpeedBased();
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

            if (!isPlaced || (other.transform.tag != "Machine" && other.transform.tag != "Cell")) return;

            if (other.transform.TryGetComponent(out Machines.MachineArmor armor))
            {
                armor.TakeDamage(damageAmount);
            }


            if (other.transform.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                physics.ActivatePhysics(true);
                if (physics.rb)
                {
                    physics.rb.AddForce(other.transform.up + elevationForce, ForceMode.Impulse);
                    physics.rb.AddTorque(torqueForce, ForceMode.Impulse);
                }
            }


            Destroy(gameObject);
        }//Closes OnCollisionEnter method



    }//Closes OffensiveBird class


}//closes Namespace declaration

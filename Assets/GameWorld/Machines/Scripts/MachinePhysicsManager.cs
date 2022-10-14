using System.Collections;
using System.Collections.Generic;
using NodeCanvas.StateMachines;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Ozamanas.Board;


namespace Ozamanas.Machines
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(FSMOwner))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(HumanMachine))]

    public class MachinePhysicsManager : MonoBehaviour
    {
        public FSMOwner fsm;
        public NavMeshAgent nma;
        public Rigidbody rb;
        public HumanMachine machine;
        public Transform collisionVFX;
        public Collider physicsCollider;
        public UnityEvent OnActivatePhysics;

        private bool guard = false;
        private void Awake()
        {
            if (!rb) rb = GetComponent<Rigidbody>();
            if (!fsm) fsm = GetComponent<FSMOwner>();
            if (!nma) nma = GetComponent<NavMeshAgent>();
            if (!machine) machine = GetComponent<HumanMachine>();
        }//Closes Awake Methods

        public void ActivatePhysics(bool activate)
        {
            if (!fsm || !nma || !rb || !machine || !physicsCollider) return;
            activate = !activate;
            fsm.enabled = activate;
            nma.enabled = activate;
            rb.isKinematic = activate;
            physicsCollider.enabled = !activate;

            machine.SetIdlingStatus();

        }//Closes ActivatePhysics method

        private void FixedUpdate()
        {
            if (!rb || rb.isKinematic) return;
            ActivatePhysics(!rb.IsSleeping());
        }

        void OnCollisionEnter(Collision collision)
        {
            if (!rb || rb.isKinematic) return;

            foreach (ContactPoint contact in collision.contacts)
            {
                collisionVFX.position = contact.point; //new Vector3(contact.point.x,collisionVFX.position.y,contact.point.z);
                collisionVFX.rotation = Quaternion.identity;
                OnActivatePhysics?.Invoke();
            }
        }

        private void CatReflexLanding()
        {
            if (!rb || rb.isKinematic) return;

            if (rb.velocity.y > 0) return;

            Quaternion quaternion = Quaternion.Euler(0, transform.rotation.y, 0);

            rb.MoveRotation(quaternion);
        }


        #region Trigger Manager

        private void OnTriggerEnter(Collider other)
        {


            if (other.tag != "Cell") return;

            CatReflexLanding();

            if (other.TryGetComponent(out Cell cell))
            {

                machine.CurrentCell = cell;
                machine.SetMachineTraitsfromCell(cell);
                cell.OnMachineEntered.Invoke(machine);
            }
        }//Closes OnTriggerEnter method


        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Cell") return;

            if (other.TryGetComponent(out Cell cell))
            {
                if (machine.CurrentCell == cell) machine.CurrentCell = null;
                machine.RemoveMachineTraitsFromCell(cell);
                cell.OnMachineExited.Invoke(machine);
            }

        }//Closes OnTriggerExit method


        private void OnTriggerStay(Collider other)
        {

        }//Closes OnTriggerStay method

        #endregion


    }//Closes MachinePhysicsManager method
}//Closes Namespace declaration

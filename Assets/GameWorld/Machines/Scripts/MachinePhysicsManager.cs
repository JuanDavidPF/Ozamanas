using System.Collections;
using System.Collections.Generic;
using NodeCanvas.StateMachines;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Ozamanas.Board;
using Ozamanas.Tags;


namespace Ozamanas.Machines
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(FSMOwner))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(HumanMachine))]

    public class MachinePhysicsManager : MonoBehaviour
    {

       
        public PhysicMode state = PhysicMode.Intelligent;
        public FSMOwner fsm;
        public NavMeshAgent nma;
        public Rigidbody rb;
        public HumanMachine machine;
        public Transform physicsVFX;
        public Collider physicsCollider;
         [Space(15)]
        [Header("Physical timeout")]
        [SerializeField] private float timeMaxInPhysical = 3f;
        private float timeInToPhysical = 0f;

         [Space(15)]
        [Header("Events")]
        public UnityEvent OnActivatePhysical;
        public UnityEvent OnActivateIntelligent;

        public UnityEvent OnActivateKinematic;

        private bool guard = false;


        private void Awake()
        {
            if (!rb) rb = GetComponent<Rigidbody>();
            if (!fsm) fsm = GetComponent<FSMOwner>();
            if (!nma) nma = GetComponent<NavMeshAgent>();
            if (!machine) machine = GetComponent<HumanMachine>();
        }//Closes Awake Methods


        public void SetKinematic()
        {

            state = PhysicMode.Kinematic;

            if (rb) rb.isKinematic = true;
            if (physicsCollider) physicsCollider.enabled = false;
            if (fsm) fsm.enabled = false;
            if (nma) nma.enabled = false;
            OnActivateKinematic?.Invoke();
            machine.SetIdlingStatus();

        }//Closes SetKinematic method


        public void SetIntelligent()
        {
            state = PhysicMode.Intelligent;

            if (rb) rb.isKinematic = true;
            if (physicsCollider) physicsCollider.enabled = false;
            if (nma) nma.enabled = true;
            if (fsm) fsm.enabled = true;
            OnActivateIntelligent?.Invoke();
            machine.SetRunningStatus();

        }//Closes SetKinematic method


        public void SetPhysical()
        {
            state = PhysicMode.Physical;

            if (rb) rb.isKinematic = false;
            if (fsm) fsm.enabled = false;
            if (nma) nma.enabled = false;
            if (physicsCollider) physicsCollider.enabled = true;
            OnActivatePhysical?.Invoke();
            machine.SetIdlingStatus();
            timeInToPhysical = Time.time;
        }//Closes ActivatePhysics method

        private void FixedUpdate()
        {
            if( state != PhysicMode.Physical ) return;

            if (!rb || rb.isKinematic ) return;

            if (rb.IsSleeping() || Time.time - timeInToPhysical >= timeMaxInPhysical) SetIntelligent();
        }

  
        void OnCollisionEnter(Collision collision)
        {
            if (!rb || rb.isKinematic) return;

            foreach (ContactPoint contact in collision.contacts)
            {

                if (!physicsVFX) continue;
                physicsVFX.position = contact.point; //new Vector3(contact.point.x,collisionVFX.position.y,contact.point.z);
                physicsVFX.rotation = Quaternion.identity;
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
                if(machine.CurrentCell == cell) return;   

                machine.CurrentCell = cell;
                machine.SetMachineTraitsfromCell(cell);
                cell.SetOnMachineEnter(machine);
            }
        }//Closes OnTriggerEnter method


        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Cell") return;

            if (other.TryGetComponent(out Cell cell))
            {
                if (machine.CurrentCell == cell) machine.CurrentCell = null;
                
                    
                    machine.RemoveMachineTraitsFromCell(cell);
                    cell.SetOnMachineExit(machine);
                
            }

        }//Closes OnTriggerExit method



        #endregion


    }//Closes MachinePhysicsManager method
}//Closes Namespace declaration

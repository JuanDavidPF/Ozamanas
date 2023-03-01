using System.Collections;
using System.Collections.Generic;
using NodeCanvas.StateMachines;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Ozamanas.Board;
using Ozamanas.Tags;
using Ozamanas.Forces;
using Ozamanas.Extenders;
using DG.Tweening;


namespace Ozamanas.Machines
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(FSMOwner))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(HumanMachine))]

    public class MachinePhysicsManager : MonoBehaviour
    {

        private Tween machineTween;
        public PhysicMode state = PhysicMode.Intelligent;
        public FSMOwner fsm;
        public NavMeshAgent nma;
        public Rigidbody rb;
        public HumanMachine machine;
         [Space(15)]
        [Header("Physical timeout")]
        private float timeMaxInPhysical = 1f;
        private float timeInToPhysical = 0f;

        [Space(15)]
        [Header("Events")]
        public UnityEvent OnActivatePhysical;
        public UnityEvent OnActivateIntelligent;
        public UnityEvent OnActivateKinematic;

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
            if (fsm) fsm.enabled = false;
            if (nma) nma.enabled = false;
            OnActivateKinematic?.Invoke();

        }//Closes SetKinematic method


        public void SetIntelligent()
        {
            state = PhysicMode.Intelligent;

            if (rb) rb.isKinematic = true;
            if (nma) nma.enabled = true;
            if (fsm) fsm.enabled = true;
            OnActivateIntelligent?.Invoke();

        }//Closes SetKinematic method


        public void SetPhysical()
        {
            state = PhysicMode.Physical;

            if (rb) rb.isKinematic = false;
            if (fsm) fsm.enabled = false;
            if (nma) nma.enabled = false;
            OnActivatePhysical?.Invoke();
            timeInToPhysical = Time.time;
        }//Closes ActivatePhysics method

        private void FixedUpdate()
        {
            if( state != PhysicMode.Physical ) return;

            if (!rb || rb.isKinematic ) return;

            if (rb.IsSleeping() || Time.time - timeInToPhysical >= timeMaxInPhysical) SetIntelligent();
        }

        

        public void AddForceToMachine(PhysicsForce force, Vector3 forceOrigin)
        {
            SetKinematic();

            switch(force.type)
            {
                case AddForceType.VerticalJump:
                PerformVerticalJump(force);
                break;
                case AddForceType.BackFlip:
                PerformBackFlip(force,forceOrigin);
                break;
                case AddForceType.FrontFlip:
                PerformFrontFlip(force,forceOrigin);
                break;
            }

        }

        
        private void PerformVerticalJump(PhysicsForce force)
        {
            Vector3 rot = new Vector3(0,0,force.flips*360);
                    
            MachineJump(transform.position,force.jumpPower,force.duration,rot);
        }

        private void PerformBackFlip(PhysicsForce force, Vector3 forceOrigin)
        {
            Vector3 rot = new Vector3(force.flips*360,0,0);
        
            Vector3 finalPosition = MathUtils.LerpByDistance(forceOrigin,transform.position,force.tiles);
            
            MachineJump(finalPosition,force.jumpPower,force.duration,rot);
        }

         private void PerformFrontFlip(PhysicsForce force, Vector3 forceOrigin)
        {
            Vector3 rot = new Vector3(-force.flips*360,0,0);
        
            Vector3 finalPosition = MathUtils.LerpByDistance(forceOrigin,transform.position,-force.tiles);
            
            MachineJump(finalPosition,force.jumpPower,force.duration,rot);
        }

        private void MachineJump(Vector3 finalPosition,float jumpPower,float duration, Vector3 rotation)
        {
            transform.DORotate(rotation,duration,RotateMode.FastBeyond360);
            
            machineTween = transform.DOJump(finalPosition,jumpPower,1,duration,false);

            machineTween.OnComplete(() =>
            {
                SetPhysical();
            });
        }

        #region Trigger Manager

        private void OnTriggerEnter(Collider other)
        {

            if (other.tag != "Cell") return;

            if (other.transform.TryGetComponentInParent(out Cell cell))
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

            if (other.TryGetComponentInParent(out Cell cell))
            {
                if (machine.CurrentCell == cell) machine.CurrentCell = null;
                    machine.RemoveMachineTraitsFromCell(cell);
                    cell.SetOnMachineExit(machine);
                
            }

        }//Closes OnTriggerExit method


        #endregion


    }//Closes MachinePhysicsManager method
}//Closes Namespace declaration

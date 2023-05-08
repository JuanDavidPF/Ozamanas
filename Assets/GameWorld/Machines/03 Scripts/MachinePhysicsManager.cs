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

    [RequireComponent(typeof(HumanMachine))]

    public class MachinePhysicsManager : MonoBehaviour
    {

        protected Tween machineTween;
        public PhysicMode state = PhysicMode.Intelligent;
        private FSMOwner fsm;
        private NavMeshAgent nma;
        protected Rigidbody rb;
        protected HumanMachine machine;
         [Space(15)]
        [Header("Physical timeout")]
        private float timeMaxInPhysical = 1f;
        private float timeInToPhysical = 0f;

        [Space(15)]
        [Header("Events")]
        public UnityEvent OnActivatePhysical;
        public UnityEvent OnActivateIntelligent;
        public UnityEvent OnActivateKinematic;

        public HumanMachine Machine { get => machine; set => machine = value; }

        protected virtual void Awake()
        {
            if (!rb) rb = GetComponent<Rigidbody>();
            if (!fsm) fsm = GetComponent<FSMOwner>();
            if (!nma) nma = GetComponent<NavMeshAgent>();
            if (!Machine) Machine = GetComponent<HumanMachine>();
        }//Closes Awake Methods


        public virtual void SetKinematic()
        {

            state = PhysicMode.Kinematic;

            if (rb) rb.isKinematic = true;
            if (fsm) fsm.enabled = false;
            if (nma) nma.enabled = false;
            OnActivateKinematic?.Invoke();

        }//Closes SetKinematic method


        public virtual void SetIntelligent()
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

        public virtual void ResetMachineNavAndAI()
        {
            if (fsm) fsm.enabled = false;
            if (nma) nma.enabled = false;
            if (nma) nma.enabled = true;
            if (fsm) fsm.enabled = true;
        }

        private void FixedUpdate()
        {
            if( state != PhysicMode.Physical ) return;

            if (!rb || rb.isKinematic ) return;

            if (rb.IsSleeping() || Time.time - timeInToPhysical >= timeMaxInPhysical) SetIntelligent();
        }

        

        public virtual void AddForceToMachine(PhysicsForce force, Vector3 forceOrigin)
        {
            if(!force) return;

            if(forceOrigin.Equals(null)) return;
            
            SetKinematic();

            if(Machine.Machine_token.machineHierarchy == MachineHierarchy.Boss)
            {
                AddForceToBoss( force, forceOrigin);
            }

            else AddForceToRegular( force, forceOrigin);



        }

         private void AddForceToRegular(PhysicsForce force, Vector3 forceOrigin)
        {
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

        private void AddForceToBoss(PhysicsForce force, Vector3 forceOrigin)
        {
             switch(force.type)
            {
                case AddForceType.VerticalJump:
                PerformVerticalPush(force);
                break;
                case AddForceType.BackFlip:
                PerformBackPush(force,forceOrigin);
                break;
                case AddForceType.FrontFlip:
                PerformFrontPush(force,forceOrigin);
                break;
            }
        }

        private void PerformVerticalPush(PhysicsForce force)
        {
            machineTween = transform.DOMoveY(-0.5f,force.pushDuration,false).SetLoops(1,LoopType.Yoyo);

            machineTween.OnComplete(() =>
            {
                SetIntelligent();
            });
        }

         private void PerformBackPush(PhysicsForce force, Vector3 forceOrigin)
        {
        
            Vector3 finalPosition = MathUtils.LerpByDistance(forceOrigin,transform.position,force.pushPower);

            finalPosition.y = transform.position.y;

            machineTween = transform.DOMove(finalPosition,force.pushDuration,false);

            machineTween.OnComplete(() =>
            {
                SetIntelligent();
            });
            
            
        }

           private void PerformFrontPush(PhysicsForce force, Vector3 forceOrigin)
        {
        
            Vector3 finalPosition = MathUtils.LerpByDistance(forceOrigin,transform.position,-force.pushPower);

            finalPosition.y = transform.position.y;

            machineTween = transform.DOMove(finalPosition,force.pushDuration,false);

            machineTween.OnComplete(() =>
            {
                SetIntelligent();
            });
            
            
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

        protected virtual void OnTriggerEnter(Collider other)
        {

            if (other.tag != "Cell") return;

            if (other.transform.TryGetComponentInParent(out Cell cell))
            {
                if(Machine.CurrentCell == cell) return;   
                Machine.CurrentCell = cell;
                cell.CurrentTopElement = Machine.Machine_token.GetTopElementToSwap(cell);
                cell.data = Machine.Machine_token.GetTokenToSwap(cell);
                cell.SetOnMachineEnter(Machine);
                
            }
        }//Closes OnTriggerEnter method

        
        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.tag != "Cell") return;

            if (other.TryGetComponentInParent(out Cell cell))
            {
                if (Machine.CurrentCell == cell) Machine.CurrentCell = null;
                    cell.SetOnMachineExit(Machine);
                
            }

        }//Closes OnTriggerExit method

        private void OnDestroy()
        {
            if(!Machine.CurrentCell) return;
            Machine.CurrentCell.SetOnMachineExit(Machine);
            Machine.CurrentCell = null;
        }

        #endregion


    }//Closes MachinePhysicsManager method
}//Closes Namespace declaration

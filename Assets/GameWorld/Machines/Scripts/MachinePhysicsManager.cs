using System.Collections;
using System.Collections.Generic;
using NodeCanvas.StateMachines;
using UnityEngine;
using UnityEngine.AI;


namespace Ozamanas.Machines
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(FSMOwner))]
    [RequireComponent(typeof(NavMeshAgent))]


    public class MachinePhysicsManager : MonoBehaviour
    {
        public FSMOwner fsm;
        public NavMeshAgent nma;
        public Rigidbody rb;

        private void Awake()
        {
            if (!rb) rb = GetComponent<Rigidbody>();
            if (!fsm) fsm = GetComponent<FSMOwner>();
            if (!nma) nma = GetComponent<NavMeshAgent>();
        }//Closes Awake Methods

        public void ActivatePhysics(bool activate)
        {
            if (!fsm || !nma || !rb) return;
            activate = !activate;
            fsm.enabled = activate;
            nma.enabled = activate;
            rb.isKinematic = activate;
        }//Closes ActivatePhysics method

        private void FixedUpdate()
        {
            if (!rb || rb.isKinematic) return;
            ActivatePhysics(!rb.IsSleeping());
        }


    }//Closes MachinePhysicsManager method
}//Closes Namespace declaration

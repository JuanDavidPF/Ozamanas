using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

namespace Ozamanas.Board
{
    [SelectionBase]
    [RequireComponent(typeof(Animator))]
    public class Cell : MonoBehaviour
    {

        private Animator m_animator;
        public CellData data;


        public float3 worldPosition;
        public int3 gridPosition;

        public bool isOccupied = false;



        private void Awake()
        {
            m_animator = m_animator ? m_animator : GetComponent<Animator>();
        }//Closes Awake method


        public void SetAnimatorTrigger(string triggerName)
        {
            if (!m_animator) return;
            m_animator.SetTrigger(triggerName);

        }//Closes SetAnimatorTrigger method

        private void OnDisable()
        {
            Board.RemoveCellFromBoard(this);
        }//Closes OnDisable event


        private void OnTriggerEnter(Collider other)
        {
            
        }//Closes OnTriggerEnter method

        private void OnTriggerExit(Collider other)
        {
           

        }//Closes OnTriggerExit method

    }//Closes Cell class
}//Closes Namespace declaration

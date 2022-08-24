using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

namespace Ozamanas.Board
{

    [RequireComponent(typeof(Animator))]
    public class Cell : MonoBehaviour
    {


        private Animator m_animator;
        public CellData data;

        public int3 gridPosition;


        private void Awake()
        {
            m_animator = m_animator ? m_animator : GetComponent<Animator>();
        }//Closes Awake method

        private void OnEnable()
        {
            Board.AddCellToBoard(this);

            m_animator.SetTrigger("Spawn");
        }//Closes OnEnable event

        private void OnDisable()
        {
            Board.RemoveCellFromBoard(this);
        }//Closes OnDisable event

    }//Closes Cell class
}//Closes Namespace declaration

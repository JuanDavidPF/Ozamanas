using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ozamanas.Board
{

    [RequireComponent(typeof(Collision))]
    public class CellChildElementSelectionHandler : MonoBehaviour
    {

        [SerializeField] private CellSelectionHandler m_cellSelectionReference;

        private void Start()
        {
            m_cellSelectionReference = m_cellSelectionReference ? m_cellSelectionReference : GetComponentInParent<CellSelectionHandler>();
            this.enabled = m_cellSelectionReference;
        }//Closes Start method

        private void OnMouseEnter()
        {
            if (!m_cellSelectionReference) return;
            m_cellSelectionReference.OnMouseEnter();
        }//Closes OnMouseEnter method

        private void OnMouseExit()
        {
            if (!m_cellSelectionReference) return;
            m_cellSelectionReference.OnMouseExit();
        }//Closes OnMouseExit method

        private void OnMouseDown()
        {
            if (!m_cellSelectionReference) return;
            m_cellSelectionReference.OnMouseDown();
        }//Closes OnMouseDown method

    }//Closes CellChildElementSelectionHandler class
}//Closes Namespace declaration
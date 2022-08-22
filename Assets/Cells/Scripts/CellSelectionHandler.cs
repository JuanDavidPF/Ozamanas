using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Board
{

    [RequireComponent(typeof(Cell))]
    [RequireComponent(typeof(Collider))]
    public class CellSelectionHandler : MonoBehaviour
    {
        private Cell m_cellReference;

        public static Cell currentCellSelected;
        public static Cell currentCellHovered;

        private void Awake()
        {
            m_cellReference = GetComponent<Cell>();
        }//Closes Awake method


        public string OnSelectionMessage;

        private void OnMouseEnter()
        {
            Debug.Log(OnSelectionMessage);
        }//Closes OnMouseEnter method

    }//Closes CellSelectionHandler class

}//Closes Namespace declaration
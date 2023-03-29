using System.Collections;
using System.Collections.Generic;
using Ozamanas.Extenders;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Ozamanas.Board
{

    public class CellSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {   
        [HideInInspector] public Cell cellReference;

        private static CellSelectionHandler m_currentCellSelected;
        private static CellSelectionHandler m_currentCellHovered;

        public static CellSelectionHandler currentCellSelected
        {
            get { return m_currentCellSelected; }
            set
            {
               /* if (m_currentCellSelected)
                {
                    if (m_currentCellHovered != m_currentCellSelected) m_currentCellSelected.EraseOutline(2);
                    else m_currentCellHovered.DrawHoveredOutline();
                }*/

                m_currentCellSelected = value;

               // if (m_currentCellSelected) m_currentCellSelected.DrawSelectedOutline();

            }
        }

        public static CellSelectionHandler currentCellHovered
        {
            get { return m_currentCellHovered; }
            set
            {

                if (m_currentCellHovered) m_currentCellHovered.cellReference.CellOverLay.DeActivatePointer(Tags.CellPointerType.MouseOverPointer);

                m_currentCellHovered = value;

                if (m_currentCellHovered && m_currentCellHovered != m_currentCellSelected)
                {
                    m_currentCellHovered.cellReference.CellOverLay.ActivatePointer(Tags.CellPointerType.MouseOverPointer);
                }

            }
        }

    

        [SerializeField] private int hoverLayer;
        [SerializeField] private int selectedLayer;

        private void Awake()
        {
            cellReference = cellReference ? cellReference : GetComponentInParent<Cell>();

        }//Closes Awake method

        

        public void OnPointerEnter(PointerEventData eventData)
        {
            currentCellHovered = this;

        }//Closes OnPointerEnter method

        public void OnPointerClick(PointerEventData eventData)
        {
            currentCellSelected = this;

            Debug.Log("OnPointerClick");

        }//Closes OnPointerClick method

       
        public void OnPointerExit(PointerEventData eventData)
        {
            if (currentCellHovered == this) currentCellHovered = null;

           cellReference.CellOverLay.DeActivatePointer(Tags.CellPointerType.Pointer);

        }//Closes OnPointerExit method
    }//Closes CellSelectionHandler class

}//Closes Namespace declaration


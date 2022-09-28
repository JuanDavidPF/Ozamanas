using System.Collections;
using System.Collections.Generic;
using Ozamanas.Extenders;
using Ozamanas.Outlines;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ozamanas.Board
{

    [RequireComponent(typeof(Cell))]


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
                if (m_currentCellSelected)
                {
                    if (m_currentCellHovered != m_currentCellSelected) m_currentCellSelected.EraseOutline();
                    else m_currentCellHovered.DrawHoveredOutline();
                }

                m_currentCellSelected = value;

                if (m_currentCellSelected) m_currentCellSelected.DrawSelectedOutline();

            }
        }

        public static CellSelectionHandler currentCellHovered
        {
            get { return m_currentCellHovered; }
            set
            {
                if (m_currentCellHovered && m_currentCellHovered != m_currentCellSelected)
                    m_currentCellHovered.EraseOutline();

                m_currentCellHovered = value;

                if (m_currentCellHovered && m_currentCellHovered != m_currentCellSelected)
                {
                    m_currentCellHovered.DrawHoveredOutline();

                }

            }
        }


        [SerializeField] private int hoverLayer;
        [SerializeField] private int selectedLayer;
        private void Awake()
        {
            cellReference = cellReference ? cellReference : GetComponent<Cell>();

        }//Closes Awake method



        public void DrawOutline(int outlineLayer)
        {
            gameObject.SetLayer(outlineLayer);
        }//Closes DrawOutline method
        public void DrawHoveredOutline() => DrawOutline(hoverLayer);
        public void DrawSelectedOutline() => DrawOutline(selectedLayer);
        public void EraseOutline() => DrawOutline(20);



        public void OnPointerEnter(PointerEventData eventData)
        {


            currentCellHovered = this;

        }//Closes OnPointerEnter method

        public void OnPointerClick(PointerEventData eventData)
        {
            currentCellSelected = this;

        }//Closes OnPointerClick method

        public void OnPointerExit(PointerEventData eventData)
        {
            if (currentCellHovered == this) currentCellHovered = null;
        }//Closes OnPointerExit method
    }//Closes CellSelectionHandler class

}//Closes Namespace declaration


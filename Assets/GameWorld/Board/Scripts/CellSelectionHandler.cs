using System.Collections;
using System.Collections.Generic;
using Ozamanas.Extenders;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityFx.Outline;

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
                    if (m_currentCellHovered != m_currentCellSelected) m_currentCellSelected.EraseOutline(2);
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

                if (m_currentCellHovered) m_currentCellHovered.EraseOutline(1);

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




        public void DrawHoveredOutline() => OutlineBuilder.AddToLayer(hoverLayer, gameObject);
        public void DrawSelectedOutline() => OutlineBuilder.AddToLayer(selectedLayer, gameObject);
        public void EraseOutline(int layerIndex) => OutlineBuilder.Remove(layerIndex, gameObject);



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


using System.Collections;
using System.Collections.Generic;
using Ozamanas.Board;
using Ozamanas.Outline;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ozamanas.Outline
{

    [RequireComponent(typeof(Cell))]

    [RequireComponent(typeof(Outline))]
    public class CellSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [HideInInspector] public Cell cellReference;
        private Outline outline;


        private static CellSelectionHandler m_currentCellSelected;
        private static CellSelectionHandler m_currentCellHovered;

        public static CellSelectionHandler currentCellSelected
        {
            get { return m_currentCellSelected; }
            set
            {
                if (m_currentCellSelected)
                {
                    if (m_currentCellHovered != m_currentCellSelected) m_currentCellSelected.ToggleOutline(false);
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
                    m_currentCellHovered.ToggleOutline(false);

                m_currentCellHovered = value;

                if (m_currentCellHovered && m_currentCellHovered != m_currentCellSelected)
                {
                    m_currentCellHovered.DrawHoveredOutline();
                    m_currentCellHovered.ToggleOutline(true);
                }

            }
        }

        [SerializeField] private OutlineConfig m_hoveredOutline;
        [SerializeField] private OutlineConfig m_selectedOutline;

        private void Awake()
        {
            cellReference = cellReference ? cellReference : GetComponent<Cell>();
            outline = outline ? outline : GetComponent<Outline>();
            outline.enabled = false;
        }//Closes Awake method


        public void ToggleOutline(bool turnOn)
        {
            if (!outline) return;
            outline.enabled = turnOn;
        }//Closes ToggleOutline method

        public void DrawOutline(OutlineConfig outlineConfig)
        {
            outline.OutlineMode = outlineConfig.mode;
            outline.OutlineColor = outlineConfig.outlineColor;
            outline.OutlineWidth = outlineConfig.width;
        }//Closes DrawOutline method
        public void DrawHoveredOutline() => DrawOutline(m_hoveredOutline);
        public void DrawSelectedOutline() => DrawOutline(m_selectedOutline);



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


using System.Collections;
using System.Collections.Generic;
using Ozamanas.Outline;
using UnityEngine;

namespace Ozamanas.Board
{

    [RequireComponent(typeof(Cell))]

    [RequireComponent(typeof(Outline.Outline))]
    public class CellSelectionHandler : MonoBehaviour
    {
        [HideInInspector] public Cell cellReference;
        private Outline.Outline outline;


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
            outline = outline ? outline : GetComponent<Outline.Outline>();
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


        public void OnMouseDown()
        {
            currentCellSelected = this;
        }//Closes OnMouseDown method

        public void OnMouseEnter()
        {
            currentCellHovered = this;
        }//Closes OnMouseEnter method

        public void OnMouseExit()
        {
            if (currentCellHovered == this) currentCellHovered = null;
        }//Closes OnMouseExit method


    }//Closes CellSelectionHandler class

}//Closes Namespace declaration


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Board;
using Ozamanas.Tags;
using Ozamanas.Machines;
using Ozamanas.Extenders;
using Ozamanas.World;
using UnityEngine.Events;
using Sirenix.OdinInspector;


namespace Ozamanas.Forest
{
    public class ForestBehaviour : Cell
    {
        
        [Title("Cell Tokens:")]
        [SerializeField] private CellData gaiaCellData;
        [SerializeField] private CellData expansionCellData;
        [SerializeField] private CellData barrierCellData;

         [SerializeField] private CellTopElement gaiaTopElement;

         [SerializeField] private CellTopElement expansionTopElement;

         [SerializeField] private CellTopElement barrierTopElement;

        


       
    }
}

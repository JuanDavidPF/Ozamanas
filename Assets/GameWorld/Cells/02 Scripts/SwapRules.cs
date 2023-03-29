using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Board
{
    [Serializable]
    public class SwapRules 
    {
         public CellData condition;
        public CellTopElement topElementToSwap;
        public CellData tokenToSwap;
    }
}

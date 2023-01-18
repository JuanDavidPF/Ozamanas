using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Tags;

namespace Ozamanas.Board
{
    public class Pointer : MonoBehaviour
    {
       [SerializeField] private CellPointerType pointerType;

        public CellPointerType PointerType { get => pointerType; set => pointerType = value; }
    }
}

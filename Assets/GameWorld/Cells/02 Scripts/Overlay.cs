using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Tags;

namespace Ozamanas.Board
{
    public class Overlay : MonoBehaviour
    {
        [SerializeField] private List<Pointer> pointers = new List<Pointer>();

        private void Start()
        {
            DeActivateAllPointers();
        }

        public void ActivatePointer(CellPointerType type)
        {
            foreach(Pointer pointer in pointers)
            {
                if(pointer.PointerType == type)
                    pointer.gameObject.SetActive(true);
            }
        }

        public void DeActivatePointer(CellPointerType type)
        {
            foreach(Pointer pointer in pointers)
            {
                if(pointer.PointerType == type)
                    pointer.gameObject.SetActive(false);
            }
        }

        public void DeActivateAllPointers()
        {
            foreach(Pointer pointer in pointers)
            {
                    pointer.gameObject.SetActive(false);
            }
        }


    }
}

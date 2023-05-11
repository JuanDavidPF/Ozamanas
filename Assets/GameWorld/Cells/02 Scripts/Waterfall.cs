using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using Ozamanas.Tags;
using Sirenix.OdinInspector;

namespace Ozamanas.Board
{
    public class Waterfall : MonoBehaviour
    {
        
        
        [SerializeField] private CellData emptyCell;
        [SerializeField] private GameObject waterFall;


        private void Start()
        {
           waterFall.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Cell") return;

            if (other.transform.TryGetComponentInParent(out Cell cell))
            {
                if(cell.data != emptyCell)
                {
                     waterFall.SetActive(false);
                }
            }
        }

       
    }
}

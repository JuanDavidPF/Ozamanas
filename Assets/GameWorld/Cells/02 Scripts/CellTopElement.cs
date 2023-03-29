using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Board
{
     [CreateAssetMenu(menuName = "Cells/Cell Top Element Token", fileName = "new Cell TopElement")]
    public class CellTopElement : ScriptableObject
    {
       public List<GameObject> topElements;

       public GameObject GetTopElement()
       {
            if(topElements.Count == 0) return null;

            return topElements[Random.Range(0,topElements.Count)];
       }
    }
}

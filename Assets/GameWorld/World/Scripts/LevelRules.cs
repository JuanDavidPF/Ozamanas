using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Board;

namespace Ozamanas.World
{
    public class LevelRules : MonoBehaviour
    {
        private void VerifyTiles()
        {
            List<Cell> allCells = new List<Cell>();
            allCells.AddRange(Board.Board.GetAllCells());

            foreach( Cell cell in allCells)
            {
                cell.CleanMachineList();
            }

        }

        IEnumerator CleanMachinesFromTiles()
        {
             yield return null;
            VerifyTiles();
        }

        public void OnMachineDestroyed()
        {
            StartCoroutine(CleanMachinesFromTiles());
        }


    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Board;
using Ozamanas.Tags;



namespace Ozamanas.Forest
{
    [RequireComponent(typeof(Cell))]
    public class ForestBehaviour : MonoBehaviour
    {
        [Serializable]
        private class TreeContainer
        {
            public Transform treeTransform;
            public GameObject forestTree;
            public GameObject expansionTree;
            public GameObject currentTree;
        }
        [SerializeField] private List<TreeContainer> trees = new List<TreeContainer>();
        private Cell cellReference;
        [Space(15)]
        [Header("Cell identificators")]
        [SerializeField] private CellData expansionID;
        [SerializeField] private CellData forestID;
        [SerializeField] private CellData barrierID;
        // Start is called before the first frame update

        void Awake()
        {
            DummyTree[] temp = GetComponentsInChildren<DummyTree>();
            for (int i = 0; i < temp.Length; i++)
            {
                TreeContainer container = new TreeContainer();
                container.treeTransform = temp[i].transform;
                container.forestTree = temp[i].ForestTree;
                container.expansionTree = temp[i].ExpansionTree;
                container.currentTree = null;
                trees.Add(container);
                temp[i].gameObject.SetActive(false);
            }
            cellReference = GetComponent<Cell>();
        }

        void Start()
        {
             if (!cellReference) return;
            if (cellReference.data == expansionID) ChangeToExpansion();
            if (cellReference.data == forestID) ChangeToForest();
            if (cellReference.data == barrierID) ChangeToBarrier();
        }

        public void OnCellDataChange()
        {

            if (!cellReference) return;
            if (cellReference.data == expansionID) ChangeToExpansion();
            if (cellReference.data == forestID) ChangeToForest();
            if (cellReference.data == barrierID) ChangeToBarrier();
        }

   

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Machine") return;

            if (!cellReference) return;

            if (cellReference.data == forestID) return;


            ChangeToForest();
        }

        private void ChangeToForest()
        {
            Debug.Log("ChangeToForest");
            for (int i = 0; i < trees.Count; i++)
            {
                if (trees[i].currentTree) Destroy(trees[i].currentTree);
                GameObject temp = Instantiate(trees[i].forestTree, transform.position, transform.rotation);
                temp.transform.parent = gameObject.transform;
                temp.gameObject.GetComponent<Rigidbody>().position = trees[i].treeTransform.position;
                temp.gameObject.GetComponent<Rigidbody>().rotation = trees[i].treeTransform.rotation;
                trees[i].currentTree = temp;
            }

        }

        private void ChangeToExpansion()
        {
             Debug.Log("ChangeToExpansion");
            for (int i = 0; i < trees.Count; i++)
            {
                if (trees[i].currentTree) Destroy(trees[i].currentTree);
                GameObject temp = Instantiate(trees[i].forestTree, transform.position, transform.rotation);
                temp.transform.parent = gameObject.transform;
                temp.transform.position = trees[i].treeTransform.position;
                temp.transform.rotation = trees[i].treeTransform.rotation;
                trees[i].currentTree = temp;
            }
        }

   
        private void ChangeToBarrier()
        {


        }
    }
}

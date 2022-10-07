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
        public Transform container;

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


            foreach (var dummy in GetComponentsInChildren<DummyTree>())
            {
                TreeContainer container = new TreeContainer();
                container.treeTransform = dummy.transform;
                container.forestTree = dummy.ForestTree;
                container.expansionTree = dummy.ExpansionTree;
                container.currentTree = null;
                trees.Add(container);
                dummy.gameObject.SetActive(false);
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

            foreach (var tree in trees)
            {
                if (tree.currentTree) Destroy(tree.currentTree);
                GameObject temp = Instantiate(tree.forestTree, container);
                tree.currentTree = temp;

            }


        }

        private void ChangeToExpansion()
        {

            foreach (var tree in trees)
            {
                if (tree.currentTree) Destroy(tree.currentTree);
                GameObject temp = Instantiate(tree.expansionTree, container);
                tree.currentTree = temp;
            }
        }


        private void ChangeToBarrier()
        {


        }
    }
}

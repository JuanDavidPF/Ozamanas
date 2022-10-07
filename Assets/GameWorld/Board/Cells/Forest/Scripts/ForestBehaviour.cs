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
            public TreeType tree_type; 
            public GameObject forestTree;
            public GameObject expansionTree;
            public GameObject currentTree;
        }
        [SerializeField] private List<TreeContainer> trees = new List<TreeContainer>();
        private Cell cellReference;

        [SerializeField] private Transform visuals;
        [Space(15)]
        [Header("Cell identificators")]
        [SerializeField] private CellData expansionID;
        [SerializeField] private CellData forestID;
        [SerializeField] private CellData barrierID;
        // Start is called before the first frame update

        void Awake()
        {
            SelectPack();
            PopulateTrees();
            cellReference = GetComponent<Cell>();
        }

        private void SelectPack()
        {
            TreePack[] temp = GetComponentsInChildren<TreePack>();
            
            int random = UnityEngine.Random.Range(0, temp.Length);
            for (int i = 0; i < temp.Length; i++)
            {
                if(i!=random) temp[i].gameObject.SetActive(false);
            }

        }
        private void PopulateTrees()
        {
            DummyTree[] temp = GetComponentsInChildren<DummyTree>();
            for (int i = 0; i < temp.Length; i++)
            {
                TreeContainer container = new TreeContainer();
                container.treeTransform = temp[i].transform;
                container.forestTree = temp[i].ForestTree;
                container.expansionTree = temp[i].ExpansionTree;
                container.currentTree = null;
                container.tree_type = temp[i].Tree_type;
                trees.Add(container);
                temp[i].gameObject.SetActive(false);
            }
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
            
            
            for (int i = 0; i < trees.Count; i++)
            {
                if (trees[i].currentTree) Destroy(trees[i].currentTree);
                if(trees[i].tree_type != TreeType.Flower) 
                {
                    GameObject temp = Instantiate(trees[i].forestTree, visuals);
                temp.transform.position = trees[i].treeTransform.position;
                temp.transform.rotation = trees[i].treeTransform.rotation;
                trees[i].currentTree = temp;
                }
                 
            }

        }

        private void ChangeToExpansion()
        {
            for (int i = 0; i < trees.Count; i++)
            {
               if (trees[i].currentTree) Destroy(trees[i].currentTree);
                GameObject temp = Instantiate(trees[i].expansionTree, visuals);
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

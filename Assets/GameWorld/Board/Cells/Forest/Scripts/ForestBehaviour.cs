using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Board;
using Ozamanas.Tags;
using Ozamanas.Machines;
using Ozamanas.Extenders;

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

            public GameObject trunk;
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
        [SerializeField] private List<GameObject> bushes = new List<GameObject>();
        [SerializeField] private List<Transform> bushesPositions = new List<Transform>();

        void Awake()
        {
            PopulateBushes();
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
                container.trunk = temp[i].Trunk;
                container.currentTree = null;
                container.tree_type = temp[i].Tree_type;
                trees.Add(container);
                temp[i].gameObject.SetActive(false);
            }
        } 

        private void PopulateBushes()
        {
            if(bushes.Count == 0 || bushesPositions.Count == 0 ) return;

            foreach(Transform pos in bushesPositions)
            {
                Instantiate(bushes[UnityEngine.Random.Range(0, bushes.Count)],pos);
            }

        }

        void Start()
        {
            StartForestByToken();
        }

        private void StartForestByToken()
        {
           
            if (!cellReference) return;
            if (cellReference.data == expansionID) ChangeToExpansion();
            if (cellReference.data == forestID) ChangeToForest();
            if (cellReference.data == barrierID) ChangeToBarrier();
        }

        public void OnCellDataChange(CellData data)
        {
            
            if (!data) return;
            if (data == expansionID) ChangeToExpansion();
            if (data == forestID) ChangeToForest();
            if (data == barrierID) ChangeToBarrier();
        }

        public void OnMachineEnter(HumanMachine machine)
        {
            if (cellReference.data == forestID) return;

            ChangeToForest();
        }

        public void OnMachineExit(HumanMachine machine)
        {
            
        }
       
        private void ChangeToForest()
        {
            
            for (int i = 0; i < trees.Count; i++)
            {
                
                if(trees[i].tree_type != TreeType.Flower) 
                {
                    if (trees[i].currentTree) Destroy(trees[i].currentTree);
                    GameObject temp = Instantiate(trees[i].forestTree, visuals);
                    temp.transform.position = trees[i].treeTransform.position;
                    temp.transform.rotation = trees[i].treeTransform.rotation;
                if (temp.transform.TryGetComponentInChildren(out Forest.JungleTree jungleTree))
                {
                    jungleTree.ForestIndex = i;
                }
                    trees[i].currentTree = temp;
                }
                else
                {
                   DestroyCurrentFlower(i);
                }
                 
            }

        }

         private void DestroyCurrentFlower(int i)
        {
            if (!trees[i].currentTree) return;

            if (trees[i].currentTree.transform.TryGetComponentInChildren(out Forest.JungleTree jungleTree))
            {
                jungleTree.HideAndDestroy();
            }
        }

        public void SetTrunk(int i)
        {
            if(i<0 || i>= trees.Count) return;

            if (trees[i].currentTree) Destroy(trees[i].currentTree);
            GameObject temp = Instantiate(trees[i].trunk, visuals);
            temp.transform.position = trees[i].treeTransform.position;
            temp.transform.rotation = trees[i].treeTransform.rotation;
            trees[i].currentTree = temp;
        }

        private void ChangeToExpansion()
        {
            for (int i = 0; i < trees.Count; i++)
            {
               if (trees[i].currentTree) Destroy(trees[i].currentTree);
                GameObject temp = Instantiate(trees[i].expansionTree, visuals);
                temp.transform.position = trees[i].treeTransform.position;
                temp.transform.rotation = trees[i].treeTransform.rotation;
                if (temp.transform.TryGetComponentInChildren(out Forest.JungleTree jungleTree))
                {
                    jungleTree.ForestIndex = i;
                }
                
                trees[i].currentTree = temp;
            }
        }

   
        private void ChangeToBarrier()
        {
             for (int i = 0; i < trees.Count; i++)
            {
               if (trees[i].currentTree) Destroy(trees[i].currentTree);
            }
        }
    }
}

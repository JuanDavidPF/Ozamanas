using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Board;
using Ozamanas.Tags;
using Ozamanas.Machines;
using Ozamanas.Extenders;
using Ozamanas.World;

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

        [SerializeField] private GameplayState winState;

         [SerializeField] private GameplayState loseState;


        // Start is called before the first frame update
                [Space(15)]
        [Header("Forest Lists")]
        [SerializeField] private List<GameObject> bushes = new List<GameObject>();
        [SerializeField] private List<Transform> bushesPositions = new List<Transform>();
        [SerializeField] private List<TreePack> treePacks = new List<TreePack>();
        [SerializeField] private List<DummyTree> dummyTrees = new List<DummyTree>();
        [SerializeField] private GameObject mountain;

        [Space(15)]
        [Header("Textures")]

  

        [SerializeField] private MeshRenderer tileMeshRenderer;

        [SerializeField] private Texture GAIATexture;

        [SerializeField] private Texture EXPTexture;

        private List<GameObject> activeBushes = new List<GameObject>();

        private void Awake()
        {
            cellReference = GetComponent<Cell>();
        }

        void Start()
        {
            PopulateBushes();
            SelectPack();
            PopulateTrees();
            StartForestByToken();
        }

        private void SelectPack()
        {
            int random = UnityEngine.Random.Range(0, treePacks.Count);

            foreach (var tree in treePacks)
            {
                tree.gameObject.SetActive(false);
            }

            treePacks[random].gameObject.SetActive(true);
            dummyTrees = treePacks[random].DummyTreeList;
        }   


        private void PopulateTrees()
        {
            foreach (var temp in dummyTrees)
            {
                TreeContainer container = new TreeContainer();
                container.treeTransform = temp.transform;
                container.forestTree = temp.ForestTree;
                container.expansionTree = temp.ExpansionTree;
                container.trunk = temp.Trunk;
                container.currentTree = null;
                container.tree_type = temp.Tree_type;
                trees.Add(container);
                temp.gameObject.SetActive(false);
            }
        }

        private void PopulateBushes()
        {
            if (bushes.Count == 0 || bushesPositions.Count == 0) return;

            foreach (Transform pos in bushesPositions)
            {
               GameObject temp = Instantiate(bushes[UnityEngine.Random.Range(0, bushes.Count)], pos);
               activeBushes.Add(temp);
            }

        }

        private void StartForestByToken()
        {

            if (!cellReference) return;
            if (cellReference.data == expansionID) ChangeToExpansion();
            if (cellReference.data == forestID) ChangeToForest();
            if (cellReference.data == barrierID) 
            {
                ChangeToBarrier();
                InstantiateBarrier();
            }

        }

        public void OnCellDataChange(CellData data)
        {

            if (!data) return;
            if (data == expansionID) ChangeToExpansion();
            if (data == forestID) ChangeToForest();
            if (data == barrierID) ChangeToBarrier();
        }

        public void OnCellGameStateChange(GameplayState state)
        {
            if (!state) return;
            if (state == winState) ChangeToExpansion();
            if (state == loseState && cellReference.data != forestID) ChangeToForest();
        }

        public void OnMachineEnter(HumanMachine machine)
        {
            if (cellReference.data == forestID || cellReference.data == barrierID) return;

            cellReference.data = forestID;

        }

        public void OnMachineExit(HumanMachine machine)
        {

        }

        private void ChangeToForest()
        {

            for (int i = 0; i < trees.Count; i++)
            {

                if (trees[i].tree_type != TreeType.Flower)
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

            tileMeshRenderer.material.mainTexture = GAIATexture;

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
            if (i < 0 || i >= trees.Count) return;

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

            tileMeshRenderer.material.mainTexture = EXPTexture;
        }


        private void ChangeToBarrier()
        {
            
            for (int i = 0; i < trees.Count; i++)
            {
                if (trees[i].currentTree) Destroy(trees[i].currentTree);
            }

            foreach( GameObject bush in activeBushes)
            {
                Destroy(bush);
            }

            tileMeshRenderer.material.mainTexture = EXPTexture;

        }

        public void InstantiateBarrier()
        {
            GameObject temp = Instantiate(mountain, visuals);

            if(temp.TryGetComponent<Mountain>(out Mountain _mountain))
            {
                _mountain.CurrentCell = cellReference;
            }
        }
    }
}

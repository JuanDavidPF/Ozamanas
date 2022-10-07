using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Tags;

namespace Ozamanas.Forest
{
    public class DummyTree : MonoBehaviour
    {
            [SerializeField] private GameObject expansionTree;
            [SerializeField] private GameObject forestTree;
            [SerializeField] private TreeType tree_type = TreeType.Tree; 

        public GameObject ForestTree { get => forestTree; set => forestTree = value; }
        public GameObject ExpansionTree { get => expansionTree; set => expansionTree = value; }
        public TreeType Tree_type { get => tree_type; set => tree_type = value; }
    }
}

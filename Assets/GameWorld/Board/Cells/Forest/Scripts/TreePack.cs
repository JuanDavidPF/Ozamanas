using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Forest
{   
    [ExecuteInEditMode]
    public class TreePack : MonoBehaviour
    {
        [SerializeField] private List<DummyTree> dummyTreeList = new List<DummyTree>();

        public List<DummyTree> DummyTreeList { get => dummyTreeList; set => dummyTreeList = value; }

        void Update()
    {
         if (!Application.isEditor) return;

         DummyTree[] dummys = gameObject.GetComponentsInChildren<DummyTree>();
        DummyTreeList = new List<DummyTree>(dummys);
    }
    }

}

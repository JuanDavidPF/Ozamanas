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

   [SerializeField] private List<JungleTree> trees = new List<JungleTree>();
    private Cell cellReference;
    [Space(15)]
        [Header("Cell identificators")]
        [SerializeField] private CellData expansionID;
        [SerializeField] private CellData forestID;
        [SerializeField] private CellData barrierID;
    // Start is called before the first frame update

    void Awake()
    {
        JungleTree[] temp = GetComponentsInChildren<JungleTree>();
        trees = new List<JungleTree>(temp);
        cellReference = GetComponent<Cell>();
    }
    public void OnCellDataChange()
    {

        
        if(cellReference.data == expansionID) ChangeToExpansion();
         if(cellReference.data == forestID) ChangeToForest();
         if(cellReference.data == barrierID) ChangeToBarrier();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Machine") return;

        ChangeToForest();
    }

    private void ChangeToForest()
    {
        
        foreach(JungleTree tree in trees)
        {
            tree.ChangeTreeToExpansion(false);
            if (tree.Tree_type == TreeType.Flower) tree.Hide();
        }

    }

    private void ChangeToExpansion()
    {
        foreach(JungleTree tree in trees)
        {
            tree.Show();
            tree.ChangeTreeToExpansion(true);
        }
    }

    private void ChangeToBarrier()
    {

        foreach(JungleTree tree in trees)
        {
            tree.Hide();
            tree.ChangeTreeToExpansion(false);
        }
    }
}
}

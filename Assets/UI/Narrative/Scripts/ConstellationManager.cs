using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.SaveSystem;
using Ozamanas.Forces;


namespace Ozamanas.UI
{
public class ConstellationManager : MonoBehaviour
{
     [SerializeField] private int forceSelectedLimit = 4;
    [SerializeField] private GameObject constellationBtnPrefab;
    [SerializeField] private Transform constellationContainer;
    [SerializeField] private PlayerDataHolder playerDataHolder;
    private List<ForceData> forcesRoster;
    [HideInInspector] public Dictionary<ForceData, ConstellationButton> constellationButtons = new Dictionary<ForceData, ConstellationButton>();

  
    private void Start()
    {
        if (!constellationBtnPrefab || !constellationContainer || !playerDataHolder) return;

        forcesRoster = playerDataHolder.data.unlockedForces;

        foreach (ForceData data in forcesRoster)
        {

            if (!data) continue;
            ConstellationButton cb = Instantiate(constellationBtnPrefab, constellationContainer).GetComponent<ConstellationButton>();

            if (!cb) continue;

            constellationButtons.Add(data, cb);
            cb.GetComponent<ConstellationButton>().SetData(data,this);

        }

       forcesRoster = playerDataHolder.data.selectedForces;

        foreach (ForceData data in forcesRoster)
        {
           constellationButtons[data].IsCurrentlySelected = true;
        }


    }

    public bool RemoveSelectedForce(ForceData force)
    {

        if(!playerDataHolder.data.selectedForces.Contains(force)) return false;

        playerDataHolder.data.selectedForces.Remove(force);

        return true;
    }

    public bool AddSelectedForce(ForceData force)
    {
        if(playerDataHolder.data.selectedForces.Contains(force)) return false;

        if(playerDataHolder.data.selectedForces.Count >= forceSelectedLimit) return false;

        playerDataHolder.data.selectedForces.Add(force);

        return true;

    }

}
}

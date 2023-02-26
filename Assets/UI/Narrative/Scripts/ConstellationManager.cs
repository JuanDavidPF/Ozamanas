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
    [SerializeField] private PlayerDataHolder playerDataHolder;
    private List<ConstellationButton> constellations = new List<ConstellationButton>();

    private void Start()
    {
        if (!playerDataHolder) return;

        List<ForceData> forcesRoster  = playerDataHolder.data.unlockedForces;

        List<ForceData> selectedRoster  = playerDataHolder.data.selectedForces;

        constellations.AddRange(FindObjectsOfType<ConstellationButton>());

        foreach(ConstellationButton constellation in constellations)
        {

            if (forcesRoster.Contains(constellation.Data)) constellation.gameObject.SetActive(true);
            else constellation.gameObject.SetActive(false);

            if (selectedRoster.Contains(constellation.Data)) constellation.IsCurrentlySelected = true;
            else constellation.IsCurrentlySelected = false;

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

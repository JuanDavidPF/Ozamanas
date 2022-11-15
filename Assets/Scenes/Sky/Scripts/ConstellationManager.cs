using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.SaveSystem;
using Ozamanas.Forces;


public class ConstellationManager : MonoBehaviour
{
    [SerializeField] private GameObject constellationBtnPrefab;
    [SerializeField] private Transform constellationContainer;
    [SerializeField] private SelectedForcesUIPanel forcesManager;
    [SerializeField] private PlayerDataHolder playerDataHolder;
     private List<ForceData> forcesRoster;

    [HideInInspector] public Dictionary<ForceData, ConstellationButton> constellationButtons = new Dictionary<ForceData, ConstellationButton>();

  
    private void Start()
    {
        if (!constellationBtnPrefab || !constellationContainer || !forcesManager || !playerDataHolder) return;

        

        forcesRoster = playerDataHolder.data.unlockedForces;

        foreach (ForceData data in forcesRoster)
        {

            if (!data) continue;
            ConstellationButton cb = Instantiate(constellationBtnPrefab, constellationContainer).GetComponent<ConstellationButton>();

            if (!cb) continue;

            constellationButtons.Add(data, cb);
            cb.GetComponent<ConstellationButton>().SetData(data, forcesManager);

        }

    }//Closes Start method
}

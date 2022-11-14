using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Tags;

public class CharcoalVisuals : MonoBehaviour
{
    [SerializeField] private List<GameObject> rocks;

    [SerializeField] private GameObject blocked;
    [SerializeField] private GameObject playable;
    void Start()
    {
        int i =  UnityEngine.Random.Range(0, rocks.Count);
        rocks[i].SetActive(true);
    }

    public void SetVisuals(LevelState state)
    {
        switch(state)
        {
            case LevelState.Blocked:
            blocked.SetActive(true);
            playable.SetActive(false);
            break;
            case LevelState.Playable:
            playable.SetActive(true);
            blocked.SetActive(false);
            break;
            default:
             playable.SetActive(false);
            blocked.SetActive(false);
            break;
        }
    }

}

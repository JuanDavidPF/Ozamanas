using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.DialogueTrees;
using Ozamanas.SaveSystem;
using JuanPayan.Helpers;
using  UnityEngine.SceneManagement;
using Ozamanas.Forces;

public class NarratorController : MonoBehaviour
{
    
     public enum SceneName
    {
            Camp = 100,
            Fire = 200,
            Sky = 300
    }

    [SerializeField] private DialogueTreeController dialogueController;


    [SerializeField] private PlayerData playerData;

     [Header("Switch Scene Setup")]
    [SerializeField] private SceneSwitcher sceneSwitcher;

    [SerializeField] private string sceneAnimatorTag;
    [SerializeField] private string fireSceneName;

    [SerializeField] private string campSceneName;

    [SerializeField] private string skySceneName;

    void Awake()
    {
        if(!sceneSwitcher) sceneSwitcher = GetComponent<SceneSwitcher>();
    }
    void Start()
    {
        dialogueController.StartDialogue();
    }

    // Update is called once per frame
   public string GetCurrentDialogueCode()
   {
        if(!playerData) return null;

        if(!string.IsNullOrEmpty(playerData.currentDialogueCode) ) return playerData.currentDialogueCode;

        return null;
   }

   public void SetNextDialogue(string code)
   {
        playerData.currentDialogueCode= code;
   }

   public void SetAnimatorTrigger(string trigger)
   {
        if(GameObject.FindGameObjectWithTag(sceneAnimatorTag).TryGetComponent<Animator>(out Animator anim))
        {
            anim.SetTrigger(trigger);
        }
   }

   public void SwitchScene(SceneName scene)
   {
        switch(scene)
        {
            case SceneName.Camp:
            sceneSwitcher.SceneToLoad = campSceneName;
            sceneSwitcher.Mode = LoadSceneMode.Single;
            break;
            case SceneName.Fire:
            sceneSwitcher.SceneToLoad = fireSceneName;
            sceneSwitcher.Mode = LoadSceneMode.Single;
            break;
            case SceneName.Sky:
            sceneSwitcher.SceneToLoad = skySceneName;
            sceneSwitcher.Mode = LoadSceneMode.Additive;
            break;
        }

        sceneSwitcher.Behaviour();
   }

   public void UnlockForce(ForceData data)
   {
       if(!data) return;

       if(playerData.unlockedForces.Contains(data)) return;

       playerData.unlockedForces.Add(data);
   }

    public void SelectForce(ForceData data)
   {
       if(!data) return;

       if(playerData.selectedForces.Contains(data)) return;

       playerData.selectedForces.Add(data);
   }
}

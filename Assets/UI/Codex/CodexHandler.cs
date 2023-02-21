using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Machines;
using Ozamanas.Forces;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace Ozamanas.UI
{

public class CodexHandler : MonoBehaviour
{
    [Title("Forces UI Components:")]
     [SerializeField] private Image forceIconGroup;

     [SerializeField] private Image forceCard;
    [SerializeField] private TextMeshProUGUI forceNameText;

    [SerializeField] private PhraseContainer forcePhraseContainer;

    [SerializeField] private RectTransform forcesContainer;


  [Title("Machines UI Components:")]
      [SerializeField] private Image machineIconGroup;
    [SerializeField] private Image machineCard;
    [SerializeField] private TextMeshProUGUI machineNameText;

      [SerializeField] private PhraseContainer machinesPhraseContainer;

          [SerializeField] private RectTransform machinesContainer;


      [Title("Narrative Components:")]

    [SerializeField] private PhraseSequence phraseSequence;

    private List<MachineCard> machines = new List<MachineCard>();

    private List<ForceCard> forces = new List<ForceCard>();


    void Awake()
    {
        MachineCard[] temp = FindObjectsOfType<MachineCard>();
        machines.AddRange(temp);


        ForceCard[] temp2 = FindObjectsOfType<ForceCard>();
        forces.AddRange(temp2);
    }

    void Start()
    {
        foreach(ForceCard force in forces)
        {
            force.forceData = force.forceData;
        }

        foreach(MachineCard machine in machines)
        {
           machine.MachineData = machine.MachineData;
        }

        if(forces.Count >0 )
        forces[0].OnPointerClick();
    }

    public void OnObjectClicked(HumanMachineToken machine)
    {
        if(!machine) return;

        if(!CheckInputFields()) return;
        
        machinesContainer.gameObject.SetActive(true);
        forcesContainer.gameObject.SetActive(false);

        machineCard.sprite = machine.machineCard;
        machineNameText.text = machine.machineName.GetLocalizedString();
        machineIconGroup.sprite = machine.machineFamily.familyIcon;
        
        
        phraseSequence.ResetSequence();
        phraseSequence.phrases.Add(machine.machineDescription);
        machinesPhraseContainer.StartDialogue(phraseSequence);

    }
    public void OnObjectClicked(ForceData force)
    {
        if(!force) return;

        if(!CheckInputFields()) return;

         machinesContainer.gameObject.SetActive(false);

        forcesContainer.gameObject.SetActive(true);

        forceCard.sprite = force.forceCard;
        forceNameText.text = force.forceName.GetLocalizedString(); 
        forceIconGroup.sprite = force.forceFamily.familyIcon;

        phraseSequence.ResetSequence();
        phraseSequence.phrases.Add(force.forceCodexDescription);
        forcePhraseContainer.StartDialogue(phraseSequence);

    }

    private bool CheckInputFields()
    {
        if(!machineCard) return false;

        if(!machineNameText) return false;

        if(!forceCard) return false;

        if(!forceNameText) return false;

        if(!machinesPhraseContainer) return false;

        if(!forcePhraseContainer) return false;

        if(!phraseSequence) return false;

        if(!machineIconGroup) return false;

        if(!forceIconGroup) return false;

        if(!machinesContainer) return false;

        if(!forcesContainer) return false;

        return true;
    }

    public void UnSelectCards()
    {
        foreach(ForceCard force in forces)
        {
            force.SelectForceCard(false);
        }

        foreach(MachineCard machine in machines)
        {
            machine.SelectMachineCard(false);
        }
    }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Machines;
using Ozamanas.Forces;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Ozamanas.UI
{

public class CodexHandler : MonoBehaviour
{
    [SerializeField] private Image machineCard;
    [SerializeField] private TextMeshProUGUI machineNameText;
    [SerializeField] private Image forceCard;
    [SerializeField] private TextMeshProUGUI forceNameText;
    [SerializeField] private PhraseContainer phraseContainer;

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

    public void OnObjectClicked(HumanMachineToken machine)
    {
        if(!machine) return;

        if(!CheckInputFields()) return;

        UnSelectCards();
        
        forceCard.gameObject.SetActive(false);
        forceNameText.gameObject.SetActive(false);
        machineCard.gameObject.SetActive(true);
        machineNameText.gameObject.SetActive(true);
        machineCard.sprite = machine.machineCard;
        machineNameText.text = machine.machineName.GetLocalizedString();
        phraseSequence.ResetSequence();
        phraseSequence.phrases.Add(machine.machineDescription);
        phraseContainer.StartDialogue(phraseSequence);

    }
    public void OnObjectClicked(ForceData force)
    {
        if(!force) return;

        if(!CheckInputFields()) return;

         UnSelectCards();

        machineCard.gameObject.SetActive(false);
        machineNameText.gameObject.SetActive(false);

        forceCard.gameObject.SetActive(true);
        forceNameText.gameObject.SetActive(true);

        forceCard.sprite = force.forceCard;
        forceNameText.text = force.forceName.GetLocalizedString(); 

        phraseSequence.ResetSequence();
        phraseSequence.phrases.Add(force.forceDescription);
        phraseContainer.StartDialogue(phraseSequence);

    }

    private bool CheckInputFields()
    {
        if(!machineCard) return false;

        if(!machineNameText) return false;

        if(!forceCard) return false;

        if(!forceNameText) return false;

        if(!phraseContainer) return false;

        if(!phraseSequence) return false;

        return true;
    }

    private void UnSelectCards()
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
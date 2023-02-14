using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ozamanas.Forces;
using UnityEngine.Localization;

namespace Ozamanas.UI
{
    public class StatsContainer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stat_1;
       [SerializeField] private TextMeshProUGUI stat_2;
        [SerializeField] private TextMeshProUGUI stat_3;
        [SerializeField] private PhraseSequence phraseSequence;
       [SerializeField] private LocalizedString phrase;

        [SerializeField] private PhraseContainer phraseContainer;

        private ForceData currentData;

       public void UpdatePanel(ForceData data)
       {    
            if(!data) return;

            if(currentData ==  data) return;

            stat_1.text = data.price.value.ToString();
            stat_2.text = data.range.value.ToString();
            stat_3.text = data.cooldown.value.ToString();
            
            phrase = data.forceDescription;
            phraseSequence.ResetSequence();
            phraseSequence.phrases.Add(phrase);

            if(!phraseContainer) return;

            phraseContainer.StartDialogue(phraseSequence);
       }
    }
}

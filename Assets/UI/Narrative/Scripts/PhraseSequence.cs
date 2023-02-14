using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Tags;
using UnityEngine.Localization;


namespace Ozamanas.UI
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Narrative/PhraseSequence", fileName = "new Phrase Sequence")]
    public class PhraseSequence : ScriptableObject
    {
        public PhraseSequenceState state = PhraseSequenceState.Active;
        public List<PreRequisite> preRequisites;
        public List<LocalizedString> phrases;
        public List<ActionButton> actionButtons;

        public void ResetSequence()
        {
            state = PhraseSequenceState.Active;
            preRequisites = new List<PreRequisite>();
            actionButtons = new List<ActionButton>();
            phrases = new List<LocalizedString>();
        }

    }
}

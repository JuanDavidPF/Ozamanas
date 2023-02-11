using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Tags;

namespace Ozamanas.UI
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Narrative/PhraseSequence", fileName = "new Phrase Sequence")]
    public class PhraseSequence : ScriptableObject
    {
        public PhraseSequenceState state = PhraseSequenceState.Active;
        public List<PreRequisite> preRequisites;
        public List<Phrase> phrases;
        public List<ActionButton> actionButtons;

    }
}

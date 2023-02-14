using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Ozamanas.UI
{   
    [CreateAssetMenu(menuName = "ScriptableObjects/Narrative/PhraseHolder", fileName = "new Phrase Holder")]
    public class PhraseHolder : ScriptableObject
    {
                       public List<PhraseSequence> data;

                        [Button("Set All Sequences to Active")]
                       private void ResetAllPhrases()
                       {
                            foreach(PhraseSequence sequence in data)
                            {
                                if( sequence.state != Tags.PhraseSequenceState.Default)
                                sequence.state = Tags.PhraseSequenceState.Active;
                            }
                            
                       }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.UI
{   
    [CreateAssetMenu(menuName = "ScriptableObjects/Narrative/PhraseHolder", fileName = "new Phrase Holder")]
    public class PhraseHolder : ScriptableObject
    {
        public List<PhraseSequence> data;
    }
}

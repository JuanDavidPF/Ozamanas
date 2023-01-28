using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;


namespace Ozamanas.UI
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Narrative/Phrase", fileName = "new Phrase")]
    public class Phrase : ScriptableObject
    {
        public LocalizedString phrase;
        public LocalizedString phraseTitle;
        public LocalizedString author;
    }
}

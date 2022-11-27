using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Ozamanas.Tags;

namespace Ozamanas.Levels
{
     [CreateAssetMenu(menuName = "References/Level/TutorialActions", fileName = "new TutorialAction")]
public class TutorialAction : ScriptableObject
{
    public TutorialType tutorialType = TutorialType.Explicative;
    public LocalizedString title = new LocalizedString();
    public LocalizedString description = new LocalizedString();
    [Range(1f,60f)]
    public float initDelay = 0f;
    public string focus;
}
}

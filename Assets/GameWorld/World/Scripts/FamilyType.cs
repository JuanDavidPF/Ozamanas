using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Localization;


namespace Ozamanas.World
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Human Machines/FamilyType", fileName = "new FamilyType")]
    public class FamilyType : ScriptableObject
    {
      [PreviewField(Alignment =ObjectFieldAlignment.Center)]
      public Sprite familyIcon;


     public LocalizedString familyName = new LocalizedString();


    }
}

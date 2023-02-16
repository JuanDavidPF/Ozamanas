using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Tags;
using Ozamanas.Forces;
using Ozamanas.Levels;
using Sirenix.OdinInspector;


namespace Ozamanas.UI
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Narrative/PreRequisite", fileName = "new PreRequisite")]
    public class PreRequisite : ScriptableObject
    {
        [HorizontalGroup("PreRequisite Type")]
        [HideLabel]
        public PreRequisiteType type;

        [HorizontalGroup("PreRequisite Type")]
        [HideLabel]
        [ShowIf("type",PreRequisiteType.UnlockedForce)]
        public ForceData force;

        [HorizontalGroup("PreRequisite Type")]
        [HideLabel]
        [ShowIf("type",PreRequisiteType.LevelComplete)]
        public LevelData level;
        
        [HorizontalGroup("PreRequisite Type")]
        [HideLabel]
        [ShowIf("type",PreRequisiteType.OnScene)]
        public Scenes sceneName;  
    }
}

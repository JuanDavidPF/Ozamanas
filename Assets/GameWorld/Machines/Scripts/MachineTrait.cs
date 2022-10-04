using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Ozamanas.Tags;


namespace Ozamanas.Machines
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Human Machines/Traits", fileName = "new HumanMachine Trait")]
    public class MachineTrait : ScriptableObject
    {
        public Sprite traitIcon;
        public LocalizedString traitName = new LocalizedString();
        public LocalizedString traitDescription = new LocalizedString();
        public List<MachineTraits> types = new List<MachineTraits>();
        public GameObject auxiliar;
        public bool isPermanentOnMachine = true;
        public float machineTimer = 0f;
        public bool isPermanetOnHolder = false;
        public float holderTimer = 0f;

    }
}

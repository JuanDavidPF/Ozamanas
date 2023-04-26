using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Tags;

namespace Ozamanas.Machines
{
    public class TraitVFX : MonoBehaviour
    {
        [SerializeField] private MachineTrait trait;

        public MachineTrait Trait { get => trait; set => trait = value; }
    }
}

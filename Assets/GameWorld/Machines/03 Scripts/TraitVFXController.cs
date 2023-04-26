using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Tags;

namespace Ozamanas.Machines
{
    public class TraitVFXController : MonoBehaviour
    {
        [SerializeField] private List<TraitVFX> vfxs = new List<TraitVFX>();

        private void Start()
        {
            DeActivateAllTraitVFX();
        }

        public void ActivateTraitVFX(MachineTrait type)
        {
            foreach(TraitVFX vfx in vfxs)
            {
                if(vfx.Trait == type)
                    vfx.gameObject.SetActive(true);
            }
        }

        public void DeActivateTraitVFX(MachineTrait type)
        {
            foreach(TraitVFX vfx in vfxs)
            {
                if(vfx.Trait == type)
                    vfx.gameObject.SetActive(false);
            }
        }

        public void DeActivateAllTraitVFX()
        {
            foreach(TraitVFX vfx in vfxs)
            {
                    vfx.gameObject.SetActive(false);
            }
        }

    }
}

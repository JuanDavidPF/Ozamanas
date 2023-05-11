using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ozamanas.Machines
{
    [RequireComponent(typeof(HumanMachine))]
    public class BulldozerBossArmor : MachineArmor
    {
        protected override void SpawnDestroyedMachine()
        {
            if(!replacement) return;
            GameObject temp = Instantiate(replacement, transform.position, transform.rotation);
            temp.SetActive(true);
            temp.GetComponent<MachineOnDestroy>().DestructableSetup(lifeSpan);
        }
    }
}

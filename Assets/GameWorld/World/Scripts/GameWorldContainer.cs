using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Machines;
using Ozamanas.Forces;
using Sirenix.OdinInspector;

namespace Ozamanas.World
{
    public class GameWorldContainer : MonoBehaviour
    {   [TableList]
        [Title("Machines Information")]
        [SerializeField] private List<HumanMachineToken> machines;
        [TableList]
        [Title("Ancient Forces Information")]
        [SerializeField] private List<ForceData> forces;
    }
}

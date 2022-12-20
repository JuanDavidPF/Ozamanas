using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Machines;
using Sirenix.OdinInspector;

namespace Ozamanas.World
{
    public class GameWorldContainer : MonoBehaviour
    {   [TableList]
    [Title("Machines Information")]
        [SerializeField] private List<HumanMachineToken> machines;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Machines;
using Ozamanas.Forces;
using Ozamanas.Levels;
using Sirenix.OdinInspector;

namespace Ozamanas.World
{
    public class GameWorldContainer : MonoBehaviour
    {   [TableList(ShowIndexLabels = true)]
        [Title("Machines Information")]
        [SerializeField] private List<HumanMachineToken> machines;
        [TableList(ShowIndexLabels = true)]
        [Title("Ancient Forces Information")]
        [SerializeField] private List<ForceData> forces;

        [TableList(ShowIndexLabels = true)]
        [Title("Levels Information")]
        [SerializeField] private List<LevelData> levels;
    }
}

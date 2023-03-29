using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuanPayan.Helpers
{
    public class DestroyIfDisabled : MonoBehaviour
    {
    public bool SelfDestructionEnabled { get; set; } = false;

    private void OnDisable()
    {
        if (SelfDestructionEnabled)
        {
            Destroy(gameObject);
        }
    }
    }
}

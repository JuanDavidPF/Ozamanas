using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TimeScaleModifier : MonoBehaviour
{
    public float currentScale = 0f;
    public float constantScale = 1f;

    private void Update()
    {
        currentScale = Time.timeScale;
    }
    public void SetDynamicTimeScale(float dynamicScale)
    {

        Time.timeScale = dynamicScale;
    }

    public void SetConstantTimeScale()
    {

        Time.timeScale = constantScale;
    }
}

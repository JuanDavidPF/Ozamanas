using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : MonoBehaviour
{
   [SerializeField] LineRenderer lr;
    [SerializeField] float dotSize = 1f;
    // Update is called once per frame

    public void SetPositions(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        
    }
    void Update()
    {
        lr.startWidth = dotSize;
        lr.material.mainTextureScale = new Vector2(1f / dotSize, 1f);
    }
}

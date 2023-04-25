using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzeBox : MonoBehaviour
{
    private void OnEnable()
    {
        transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y - 1, transform.parent.position.z);
    }
}

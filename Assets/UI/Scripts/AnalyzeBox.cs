using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalyzeBox : MonoBehaviour
{
    private Image _bar;
    private void Awake()
    {
        _bar = transform.GetChild(0).GetChild(2).GetComponent<Image>();
    }
    private void OnEnable()
    {
        transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y - 1, transform.parent.position.z);
        _bar.fillAmount = 0;
    }
}

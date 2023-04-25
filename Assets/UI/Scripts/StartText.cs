using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartText : MonoBehaviour
{
    private Text _start;
    private void Awake()
    {
        _start = GetComponent<Text>();
        StartCoroutine(BreathControl());
    }

    IEnumerator BreathControl()
    {
        float _breathTime = 0.15f;
        float elapsedTime = 0;
        float repeatTime = 10;
        
        for(int i = 0; i < repeatTime; ++i)
        {
            _start.color = new Color(_start.color.r, _start.color.g, _start.color.b, 0);
            while (_start.color.a < 1)
            {
                elapsedTime += Time.deltaTime;
                _start.color = new Color(_start.color.r, _start.color.g, _start.color.b, Mathf.Min(1, elapsedTime / _breathTime));
                yield return null;
            }

            elapsedTime = 0;
            while (_start.color.a > 0)
            {
                elapsedTime += Time.deltaTime;
                _start.color = new Color(_start.color.r, _start.color.g, _start.color.b, Mathf.Max(0, 1 - (elapsedTime / _breathTime)));
                yield return null;
            }
        }
        gameObject.SetActive(false);
    }
}

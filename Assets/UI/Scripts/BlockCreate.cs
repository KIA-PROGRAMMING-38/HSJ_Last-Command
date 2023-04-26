using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockCreate : MonoBehaviour
{
    private SpriteRenderer _warning;
    private void Awake()
    {
        _warning = transform.GetChild(0).GetComponent<SpriteRenderer>();
        StartCoroutine(BreathControl());
    }

    IEnumerator BreathControl()
    {
        float _breathTime = 0.2f;
        float elapsedTime = 0;
        float repeatTime = 5;

        for (int i = 0; i < repeatTime; ++i)
        {
            _warning.color = new Color(_warning.color.r, _warning.color.g, _warning.color.b, 0);
            while (_warning.color.a < 0.8)
            {
                elapsedTime += Time.deltaTime;
                _warning.color = new Color(_warning.color.r, _warning.color.g, _warning.color.b, Mathf.Min(1, elapsedTime / _breathTime));
                yield return null;
            }

            elapsedTime = 0;
            while (_warning.color.a > 0.2)
            {
                elapsedTime += Time.deltaTime;
                _warning.color = new Color(_warning.color.r, _warning.color.g, _warning.color.b, Mathf.Max(0, 1 - (elapsedTime / _breathTime)));
                yield return null;
            }
        }
        gameObject.SetActive(false);
        transform.parent.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}

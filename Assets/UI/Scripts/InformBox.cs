using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformBox : MonoBehaviour
{
    private float _shakeTime = 1f;
    private float _shakeTense = 0.01f;
    private float _disappearTime = 0.1f;
    private float _elapsedTime;
    private float _elapsedDisappearTime;
    private RectTransform _rectTransform;
    private Vector3 _originalPosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        transform.localPosition = Vector2.zero;
        _originalPosition = transform.position + (Vector3)Random.insideUnitCircle;
        _elapsedTime = 0;
        _elapsedDisappearTime = 0;
        _rectTransform.localScale = new Vector3(1, 1, 1);
    }
    private void Update()
    {
        if(_elapsedTime < _shakeTime)
        {
            float x = Random.Range(-1f, 1f) * _shakeTense;
            float y = Random.Range(-1f, 1f) * _shakeTense;
            _rectTransform.position = _originalPosition + new Vector3(x, y, 0f);
            _elapsedTime += Time.deltaTime;
        }
        else
        {
            _elapsedDisappearTime += Time.deltaTime;
            float changedY = 1 - (_elapsedDisappearTime / _disappearTime);
            _rectTransform.localScale = new Vector3(_rectTransform.localScale.x, changedY, _rectTransform.localScale.z);
            if(changedY <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

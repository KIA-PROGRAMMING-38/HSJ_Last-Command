using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private Image[] _lines;
    [SerializeField] private GameObject _bossName;
    [SerializeField] private GameObject _bossTitle;
    private Text _name;
    private Text _title;
    private Image _nameMask;
    private Image _titleMask;
    private RectTransform _nameRect;
    private RectTransform _titleRect;
    private float _targetTime = 2;
    private float _nameFadeInSpeed = 80;
    private float _titleFadeInSpeed = 20;
    private float _fadeOutSpeed = 2;
    public event Action OnGameStart;

    void Awake()
    {
        _nameRect = _bossName.GetComponent<RectTransform>();
        _titleRect = _bossTitle.GetComponent<RectTransform>();
        _nameRect.localPosition = Vector2.zero;
        _titleRect.localPosition = Vector2.zero;
        _name = _bossName.transform.GetComponentInChildren<Text>();
        _title = _bossTitle.transform.GetComponentInChildren<Text>();
        _nameMask = _bossName.GetComponent<Image>();
        _titleMask = _bossTitle.GetComponent<Image>();
        _bossName.gameObject.SetActive(false);
        _bossTitle.gameObject.SetActive(false);
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        bool isLineDrawed = false;
        float elapsedTime = 0;
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while(!isLineDrawed)
        {
            for (int i = 0; i < _lines.Length; ++i)
            {
                elapsedTime += Time.deltaTime;
                _lines[i].fillAmount = elapsedTime / _targetTime;
            }
            yield return null;
            if (_lines[0].fillAmount >= 1)
            {
                isLineDrawed = true;
            }
        }

        yield return wait;
        elapsedTime = 0;
        _bossName.SetActive(true);
        _bossTitle.SetActive(true);
        bool isFadeInFinished = false;

        while (!isFadeInFinished)
        {
            elapsedTime += Time.deltaTime;
            _name.color = new Color(_name.color.r, _name.color.g, _name.color.b, elapsedTime);
            _title.color = new Color(_title.color.r, _title.color.g, _title.color.b, elapsedTime);
            _nameRect.localPosition = new Vector2(0, elapsedTime * _nameFadeInSpeed);
            _titleRect.localPosition = new Vector2(0, -elapsedTime * _titleFadeInSpeed);
            yield return null;
            if(_nameRect.localPosition.y >= 80)
            {
                isFadeInFinished = true;
            }
        }

        yield return wait;
        elapsedTime = 0;
        float fadeFinishedTime = 0;
        bool isFadeOutFinished = false;
        bool isRightLineFaded = false;
        _lines[0].rectTransform.rotation = Quaternion.Euler(0, 0, 180);
        while (!isFadeOutFinished)
        {
            elapsedTime += Time.deltaTime;
            _nameMask.fillAmount = 1 - elapsedTime;
            _titleMask.fillAmount = 1 - elapsedTime;
            if(!isRightLineFaded)
            {
                _lines[1].fillAmount = 1 - (elapsedTime * _fadeOutSpeed);
                if (_lines[1].fillAmount <= 0)
                {
                    isRightLineFaded = true;
                    fadeFinishedTime = elapsedTime;
                }
            }
            else
            {
                _lines[0].fillAmount = 1 - (elapsedTime - fadeFinishedTime) * _fadeOutSpeed;
            }
            yield return null;
            if (_lines[0].fillAmount <= 0)
            {
                isFadeOutFinished = true;
            }
        }
        OnGameStart?.Invoke();
    }
}

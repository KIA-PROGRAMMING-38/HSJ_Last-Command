using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util.Enum;

public class GameClearUI : MonoBehaviour
{
    [SerializeField] private Text[] _infoTexts;
    [SerializeField] private Text[] _infos;
    [SerializeField] private Text[] _exTexts;
    [SerializeField] private Image _line;
    private float _playTime;
    private Rank _rank;
    private int _hitCount;
    [SerializeField] private float _fadeInTime;
    private IEnumerator _waitInput;

    public void CalculateScore()
    {
        _infos[0].text = string.Format($"{(int)_playTime / 60:D2} : {(int)_playTime % 60:D2}");
        _infos[1].text = _hitCount.ToString();
        _infos[2].text = _rank.ToString();
    }
    public void SetUI(float playTime, Rank rank, int hitCount)
    {
        _playTime = playTime;
        _rank = rank;
        _hitCount = hitCount;
    }
    private void OnEnable()
    {
        for (int i = 0; i < _infoTexts.Length; ++i)
        {
            ResetColor(_infoTexts[i]);
            ResetColor(_infos[i]);
        }
        for (int i = 0; i < _exTexts.Length; ++i)
        {
            ResetColor(_exTexts[i]);
        }
        ResetColor(_line);
        StartCoroutine(ShowUI());
    }
    private IEnumerator ShowUI()
    {
        WaitForSeconds wait = new WaitForSeconds(_fadeInTime);

        StartCoroutine(FadeIn(_exTexts[0]));
        StartCoroutine(FadeIn(_line));

        yield return wait;

        for (int i = 0; i < _infoTexts.Length; ++i)
        {
            StartCoroutine(FadeIn(_infos[i]));
            StartCoroutine(FadeIn(_infoTexts[i]));
            yield return wait;
        }

        StartCoroutine(FadeIn(_exTexts[1]));
        _waitInput = WaitControl();
        StartCoroutine(_waitInput);
    }

    private IEnumerator FadeIn(Graphic graphic)
    {
        float t = 0;
        Color originalColor = graphic.color;

        while (t < 1)
        {
            t += Time.deltaTime / _fadeInTime;
            originalColor.a = Mathf.Lerp(0, 1, t);
            graphic.color = originalColor;
            yield return null;
        }
        graphic.color = originalColor;
    }
    private IEnumerator WaitControl()
    {
        while(true)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                Application.Quit();
            }
            yield return null;
        }
    }

    private void ResetColor(Graphic graphic)
    {
        Color color = graphic.color;
        color.a = 0;
        graphic.color = color;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}

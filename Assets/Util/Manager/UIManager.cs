using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;
using Util.Manager;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UIManager : MonoBehaviour
{
    public GameManager _gameManager { get; private set; }
    [SerializeField] private GameObject _inGameUIPrefabs;
    private GameObject _inGameUI;
    private GameObject _playerHPUI;
    private GameObject _bossHPUI;
    private GameOverUI _gameOverUI;
    private Image[] _playerHurtEffect;
    public GameClearUI _gameClearUI { get; private set; }

    private GameObject[] _playerHPImages;
    private GameObject[] _bossHPImages;

    private Image[] _bossHeartImages;
    private Text _bossHeartText;
    private int _bossHPId;
    private int _playerHPId;

    private float _shakeTense = 8;
    private float _shakeTime = 1;
    private RectTransform[] _rectTransform;
    private Vector3[] _originalPosition;

    private float _breath;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    public void BossHpDecrease()
    {
        _bossHPImages[_bossHPId].SetActive(false);
        ++_bossHPId;
    }
    public void PlayerHpDecrease()
    {
        _playerHPImages[_playerHPId].SetActive(false);
        ++_playerHPId;
    }

    public void ChangeTemp(int tempDamage, int confDamage, float treshold)
    {
        _bossHeartImages[1].fillAmount = Mathf.Min(1, (tempDamage + confDamage) / treshold);
        Debug.Log(_bossHeartImages[1].fillAmount);
        _bossHeartText.text = $"{(int)(_bossHeartImages[1].fillAmount * 100)}%";

    }
    public void ChangeConf(int totalDamage, float treshold)
    {
        _bossHeartImages[2].fillAmount = Mathf.Min(1, totalDamage / treshold);
    }
    public void EnterGroggy()
    {
        _bossHeartImages[2].fillAmount = 1;
    }
    public void ResetFill()
    {
        _bossHeartImages[1].fillAmount = 0;
        _bossHeartImages[2].fillAmount = 0;
        _bossHeartText.text = "0%";
    }
    public void SetUI(int playerHP, Boss boss)
    {
        _playerHPImages = new GameObject[playerHP];
        _bossHPImages = new GameObject[boss.HP()];
        _bossHeartImages = new Image[3];
        _rectTransform = new RectTransform[2];
        _originalPosition = new Vector3[2];
        if(_inGameUI == null)
        {
            _inGameUI = Instantiate(_inGameUIPrefabs);
            _playerHPUI = _inGameUI.transform.GetChild(0).gameObject;
            _rectTransform[0] = _playerHPUI.GetComponent<RectTransform>();
            _bossHPUI = _inGameUI.transform.GetChild(1).gameObject;
            _rectTransform[1] = _bossHPUI.GetComponent<RectTransform>();
            _gameClearUI = _inGameUI.transform.GetChild(2).GetComponent<GameClearUI>();
            _gameOverUI = _inGameUI.transform.GetChild(3).GetComponent<GameOverUI>();
            for(int i = 0; i < 2; ++ i)
            {
                _originalPosition[i] = _rectTransform[i].position;
            }
        }
        for(int i = 0; i < playerHP; ++i)
        {
            _playerHPImages[i] = _playerHPUI.transform.GetChild(i + 1).GetChild(0).gameObject;
        }
        for (int i = 0; i < boss.HP(); ++i)
        {
            _bossHPImages[i] = _bossHPUI.transform.GetChild(i + 1).GetChild(0).gameObject;
        }
        for(int i = 0; i < 3; ++i)
        {
            _bossHeartImages[i] = boss.transform.GetChild(i + 1).GetChild(0).GetComponent<Image>();
        }
        _bossHeartText = boss.transform.GetChild(3).GetChild(1).GetComponent<Text>();
        _playerHurtEffect = _inGameUI.transform.GetChild(4).GetComponentsInChildren<Image>();
    }
    public void RemoveUI()
    {
        _playerHPUI.SetActive(false);
        _bossHPUI.SetActive(false);
    }

    public void ShowGameOverUI()
    {
        _gameOverUI.gameObject.SetActive(true);
    }
    public void ShowGameClearUI()
    {
        _gameClearUI.gameObject.SetActive(true);
    }
    
    public void ShakeUI()
    {
        StartCoroutine(Shake());
    }
    public void BreathUI()
    {
        foreach(Image image in _playerHurtEffect)
        {
            StartCoroutine(EffectBreath(image));
        }
    }
    IEnumerator Shake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _shakeTime)
        {
            float x = Random.Range(-1f, 1f) * _shakeTense;
            float y = Random.Range(-1f, 1f) * _shakeTense;
                
            for (int i = 0; i < 2; ++i)
            {
                _rectTransform[i].position = _originalPosition[i] + new Vector3(x, y, 0f);
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        for (int i = 0; i < 2; ++i)
        {
            _rectTransform[i].position = _originalPosition[i];
        }
    }

    IEnumerator EffectBreath(Image image)
    {
        image.gameObject.SetActive(true);
        WaitForSeconds wait = new WaitForSeconds(1f);

        image.color = new Color(
            image.color.r,
            image.color.g,
            image.color.b,
            255f);

        while (image.color.a > 10)
        {
            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                image.color.a - (_breath * Time.deltaTime));

            yield return null;
        }

        image.color = new Color(
            image.color.r,
            image.color.g,
            image.color.b,
            10f);

        yield return wait;

        while (image.color.a < 255)
        {
            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                image.color.a + (_breath * Time.deltaTime));

            yield return null;
        }

        image.color = new Color(
            image.color.r,
            image.color.g,
            image.color.b,
            255f);

        yield return wait;

        image.gameObject.SetActive(false);
    }
}

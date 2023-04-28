using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;
using Util.Manager;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _startUIPrefab;
    private GameObject _startUI;
    public Test _startEffect { get; private set; }
    private Image _startFadeIn;

    public StageManager _stageManager { get; private set; }
    [SerializeField] private GameObject _inGameUIPrefab;
    private GameObject _inGameUI;

    private GameObject _playerHPUI;
    private GameObject[] _playerHPImages;
    private int _playerHPId;
    private int _playerHp;

    private GameObject _bossHPUI;
    private GameObject[] _bossHPImages;
    private int _bossHPId;

    private GameOverUI _gameOverUI;
    public GameClearUI _gameClearUI { get; private set; }

    private GameObject _playerHurt;
    private Image[] _playerHurtEffect;
    private float _breathTime = 0.4f;

    private Image[] _bossHeartImages;
    private Text _bossHeartText;

    private float _shakeTense = 8;
    private float _shakeTime = 1;
    private RectTransform[] _rectTransform;
    private Vector3[] _originalPosition;

    private Text _earnEnergyBox;
    private char[] _earnedEnergy;
    private int _maxLength;

    private Image _analyzeBox;

    public void Awake()
    {
        _startUI = Instantiate(_startUIPrefab);
        _startEffect = _startUI.GetComponent<Test>();
    }
    public void Init(StageManager gameManager)
    {
        _stageManager = gameManager;
    }
    public void SetUI(Player player, Boss boss)
    {
        if (_inGameUI == null)
        {
            _playerHPImages = new GameObject[player.HP()];
            _bossHPImages = new GameObject[boss.HP()];
            _bossHeartImages = new Image[3];
            _rectTransform = new RectTransform[2];
            _originalPosition = new Vector3[2];
            _playerHurtEffect = new Image[2];
            _inGameUI = Instantiate(_inGameUIPrefab);
            _playerHPUI = _inGameUI.transform.GetChild(0).gameObject;
            _playerHp = _playerHPUI.transform.childCount - 1 - player.HP();
            _rectTransform[0] = _playerHPUI.GetComponent<RectTransform>();
            _bossHPUI = _inGameUI.transform.GetChild(1).gameObject;
            _rectTransform[1] = _bossHPUI.GetComponent<RectTransform>();
            _gameClearUI = _inGameUI.transform.GetChild(2).GetComponent<GameClearUI>();
            _gameOverUI = _inGameUI.transform.GetChild(3).GetComponent<GameOverUI>();
            _playerHurt = _inGameUI.transform.GetChild(4).gameObject;
            _startFadeIn = _inGameUI.transform.GetChild(5).GetComponent<Image>();
            for (int i = 0; i < 2; ++i)
            {
                _originalPosition[i] = _rectTransform[i].position;
            }
            _earnEnergyBox = player.transform.Find("Earn Energy Box").GetChild(0).GetComponentInChildren<Text>();
            _earnedEnergy = new char[player.maxLength - 3];
            _maxLength = player.maxLength - 3;

            _analyzeBox = player.transform.Find("Analyzing Box").GetChild(0).GetChild(2).GetComponent<Image>();
        }
        for (int i = 0; i < player.HP(); ++i)
        {
            _playerHPImages[i] = _playerHPUI.transform.GetChild(i + _playerHp + 1).GetChild(0).gameObject;
            _playerHPImages[i].transform.parent.gameObject.SetActive(true);
        }
        for (int i = 0; i < boss.HP(); ++i)
        {
            _bossHPImages[i] = _bossHPUI.transform.GetChild(i + 1).GetChild(0).gameObject;
            _bossHPImages[i].transform.parent.gameObject.SetActive(true);
        }
        for (int i = 0; i < 3; ++i)
        {
            _bossHeartImages[i] = boss.transform.GetChild(i + 1).GetChild(0).GetComponent<Image>();
        }
        _bossHeartText = boss.transform.GetChild(3).GetChild(1).GetComponent<Text>();
        for (int i = 0; i < 2; ++i)
        {
            _playerHurtEffect[i] = _playerHurt.transform.GetChild(i).GetComponent<Image>();
        }
        StartCoroutine(FadeIn());
    }
    public void RemoveUI()
    {
        _playerHPUI.SetActive(false);
        _bossHPUI.SetActive(false);
    }

    public void PlayerHpDecrease()
    {
        _playerHPImages[_playerHPId].SetActive(false);
        ++_playerHPId;
    }
    public void PlayerHpIncrease()
    {
        --_playerHPId;
        _playerHPImages[_playerHPId].SetActive(true);
    }

    public void BossHpDecrease()
    {
        _bossHPImages[_bossHPId].SetActive(false);
        ++_bossHPId;
    }
    public void ChangeTemp(int tempDamage, int confDamage, float treshold)
    {
        _bossHeartImages[1].fillAmount = Mathf.Min(1, (tempDamage + confDamage) / treshold);
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
        _playerHurt.SetActive(true);
        StartCoroutine(EffectBreath());
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

    IEnumerator EffectBreath()
    {
        float elapsedTime = 0;
        foreach (Image image in _playerHurtEffect)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
        while (_playerHurtEffect[0].color.a < 1)
        {
            elapsedTime += Time.deltaTime;
            foreach (Image image in _playerHurtEffect)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Min(1, elapsedTime / _breathTime));
            }
            yield return null;
        }

        elapsedTime = 0;
        while (_playerHurtEffect[0].color.a > 0)
        {
            elapsedTime += Time.deltaTime;
            foreach (Image image in _playerHurtEffect)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Max(0, 1 - (elapsedTime / _breathTime)));
            }
            yield return null;
        }
        _playerHurt.SetActive(false);
    }

    public void ChangeText(int currentLength)
    {
        for (int i = 0; i < currentLength; ++i)
        {
            _earnedEnergy[i] = 'I';
        }
        for (int i = currentLength; i < _maxLength; ++i)
        {
            _earnedEnergy[i] = '.';
        }
        _earnEnergyBox.text = $"loading {new string(_earnedEnergy)}";
    }

    public void ChangeFill(float offsetTime)
    {
        _analyzeBox.fillAmount = offsetTime;
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        float targetTime = 0.5f;
        while(elapsedTime <= targetTime)
        {
            elapsedTime += Time.deltaTime;
            _startFadeIn.color = new Color(1, 1, 1, 1 - (elapsedTime / targetTime));
            yield return null;
        }
    }
}

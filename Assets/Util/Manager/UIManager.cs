using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util.Manager;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UIManager : MonoBehaviour
{
    public GameManager _gameManager { get; private set; }
    [SerializeField] private GameObject _inGameUIPrefabs;
    private GameObject _inGameUI;
    private GameObject[] _playerHPImages;
    private GameObject[] _bossHPImages;
    private Image[] _bossHeartImages;
    private int _bossHPId;
    private int _playerHPId;

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
    }
    public void SetUI(int playerHP, Boss boss)
    {
        _playerHPImages = new GameObject[playerHP];
        _bossHPImages = new GameObject[boss.HP()];
        _bossHeartImages = new Image[3];
        if(_inGameUI == null)
        {
            _inGameUI = Instantiate(_inGameUIPrefabs);
        }
        for(int i = 0; i < playerHP; ++i)
        {
            _playerHPImages[i] = _inGameUI.transform.GetChild(0).GetChild(i + 1).GetChild(0).gameObject;
        }
        for (int i = 0; i < boss.HP(); ++i)
        {
            _bossHPImages[i] = _inGameUI.transform.GetChild(1).GetChild(i + 1).GetChild(0).gameObject;
        }
        for(int i = 0; i < 3; ++i)
        {
            _bossHeartImages[i] = boss.transform.GetChild(i + 1).GetChild(0).GetComponent<Image>();
        }
    }
}

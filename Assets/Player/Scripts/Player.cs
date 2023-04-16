using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class Player : MonoBehaviour
{
    private GameObject _boss;
    [SerializeField] private GameObject _snakeSprite;
    private int _defaultLength;
    [SerializeField] private int _maxLength;
    public int _currentLength;
    [SerializeField] private int _frameDelay;

    private GameObject[] _energies;
    private Stack<bool> _energy;

    public event Action OnOverclock;
    public event Action OnOverclockEnd;
    private bool _isOverclocking;

    [SerializeField] private float _invincibleTime;
    private int _Hp;
    public event Action OnHpDecrease;

    private void Awake()
    {
        GameManager._instance._player = gameObject;
        if(GameManager._instance._boss == null)
        {
            _boss = GameObject.FindWithTag("Boss");
        }
        else
        {
            _boss = GameManager._instance._boss;
        }

        if(_boss != null)
        {
            _boss.GetComponent<Boss>().OnConfChange -= Invincible;
            _boss.GetComponent<Boss>().OnConfChange += Invincible;
        }
        _defaultLength = 3;
        _currentLength = _defaultLength;
        _energies = new GameObject[_maxLength];
        _energies[0] = gameObject;
        _isOverclocking = false;
        _energy = new Stack<bool>();

        for (int i = 1; i < _maxLength; ++i)
        {
            GameObject newHead = Instantiate(_snakeSprite, transform);
            newHead.name = $"Head{i}";
            _energies[i] = newHead;
            Head headComponent = newHead.AddComponent<Head>();
            headComponent._frameDelay = _frameDelay;
            headComponent._frontHead = _energies[i - 1].transform;
        }
        for (int i = _maxLength; i > _defaultLength; --i)
        {
            transform.GetChild(i - 2).gameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        if (_boss != null)
        {
            _boss.GetComponent<Boss>().OnConfChange -= Invincible;
        }
    }
    public void EarnEnergy()
    {
        if (_currentLength < _maxLength)
        {
            _currentLength++;
            transform.GetChild(_currentLength - 2).gameObject.SetActive(true);
            _energy.Push(true);
        }

        if (_currentLength == _maxLength)
        {
            Overclock();
        }
    }

    public bool IsEnergyCharged()
    {
        if (_energy.Count > 0)
        {
            return true;
        }
        return false;

    }

    public void UseEnergy()
    {
        _energy.Pop();
        _currentLength--;
    }

    private void Overclock()
    {
        if (!_isOverclocking)
        {
            OnOverclock?.Invoke();
            _isOverclocking = true;
            Debug.Log("오버클럭!");
        }
    }
    public void EndOverclock()
    {
        if (_isOverclocking)
        {
            OnOverclockEnd?.Invoke();
            _isOverclocking = false;
            Debug.Log("오버클럭 해제");
        }
    }

    public void Invincible()
    {
        StartCoroutine("OnInvincible");
    }

    IEnumerator OnInvincible()
    {
        Debug.Log("무적 시작");
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        yield return new WaitForSeconds(_invincibleTime);
        Debug.Log("무적 끝");
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void Damaged()
    {
        OnHpDecrease?.Invoke();
        _Hp--;
        Invincible();
    }
}

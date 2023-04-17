using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public ObjectManager _objectManager { get; private set; }
    public int _temporaryDamageGain { get; private set; }
    public int _confirmedDamageGain { get; private set; }
    public int _totalDamageGain { get; private set; }

    [SerializeField] private float _damageTreshold;

    [SerializeField] private float _diminishingTime;
    private float _elapsedTime;

    [SerializeField] private int _Hp;

    public event Action OnAttackSuccess;

    public event Action<int, int, float> OnTempChange;
    public event Action<int, float> OnConfChange;
    public event Action OnDamageChange;
    public event Action OnGroggy;
    public event Action OnDie;

    void Awake()
    {
        _temporaryDamageGain = 0;
        _confirmedDamageGain = 0;
        _totalDamageGain = 0;
        _elapsedTime = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(_temporaryDamageGain > 0)
            {
                ChangeDamageType();
            }
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            GetTempDamage();
        }

        Debug.Log($"현재 임시 피해 : {_temporaryDamageGain}");
        Debug.Log($"현재 확정 피해 : {_confirmedDamageGain}");
        Debug.Log($"총 피해 : {_totalDamageGain}");

        if(_totalDamageGain >= _damageTreshold)
        {
            EnterGroggyState();
        }
    }

    private void Update()
    {
        if(_temporaryDamageGain > 0)
        {
            if(_elapsedTime >= _diminishingTime)
            {
                DecreaseTempDamage();
            }
            else
            {
                _elapsedTime += Time.deltaTime;
            }
        }
    }
    public void Damaged()
    {
        _Hp--;
        if(_Hp <= 0)
        {
            OnDie?.Invoke();
            return;
        }
        OnAttackSuccess?.Invoke();
    }

    private void ChangeDamageType()
    {
        Debug.Log("임시를 확정으로");
        _confirmedDamageGain += _temporaryDamageGain;
        _temporaryDamageGain = 0;
        _totalDamageGain = _temporaryDamageGain + _confirmedDamageGain;
        OnConfChange?.Invoke(_totalDamageGain, _damageTreshold);
        OnDamageChange?.Invoke();
    }

    private void GetTempDamage()
    {
        Debug.Log("임시피해 5!");
        _temporaryDamageGain += 5;
        _totalDamageGain = _temporaryDamageGain + _confirmedDamageGain;
        OnTempChange?.Invoke(_temporaryDamageGain, _confirmedDamageGain, _damageTreshold);
    }

    private void DecreaseTempDamage()
    {
        _temporaryDamageGain--;
        _elapsedTime = 0;
        Debug.Log($"현재 임시 피해 : {_temporaryDamageGain}");
        OnTempChange?.Invoke(_temporaryDamageGain, _confirmedDamageGain, _damageTreshold);
    }

    private void EnterGroggyState()
    {
        GetComponent<Animator>().SetBool("isGroggy", true);
        transform.Find("Circle").gameObject.SetActive(true);
        _totalDamageGain = 0;
        _temporaryDamageGain = 0;
        _confirmedDamageGain = 0;
        OnGroggy?.Invoke();
    }
    public int HP()
    {
        return _Hp;
    }

    public void Init(ObjectManager objectManager)
    {
        _objectManager = objectManager;
    }
}

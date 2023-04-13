using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private int _temporaryDamageGain;
    private int _confirmedDamageGain;
    private int _totalDamageGain;

    [SerializeField] private float _diminishingTime;
    private float _elapsedTime;

    void Awake()
    {
        gameObject.tag = "Boss";
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
                Debug.Log("�ӽø� Ȯ������");
                _confirmedDamageGain += _temporaryDamageGain;
                _temporaryDamageGain = 0;
                _totalDamageGain = _temporaryDamageGain + _confirmedDamageGain;
                collision.GetComponent<Player>().Invincible();
            }
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Debug.Log("�ӽ����� 5!");
            _temporaryDamageGain += 5;
            collision.gameObject.SetActive(false);
            _totalDamageGain = _temporaryDamageGain + _confirmedDamageGain;
        }

        Debug.Log($"���� �ӽ� ���� : {_temporaryDamageGain}");
        Debug.Log($"���� Ȯ�� ���� : {_confirmedDamageGain}");
        Debug.Log($"�� ���� : {_totalDamageGain}");
    }

    private void Update()
    {
        if(_temporaryDamageGain > 0)
        {
            if(_elapsedTime >= _diminishingTime)
            {
                _temporaryDamageGain--;
                _elapsedTime = 0;
                Debug.Log($"���� �ӽ� ���� : {_temporaryDamageGain}");
            }
            else
            {
                _elapsedTime += Time.deltaTime;
            }
        }
    }
}

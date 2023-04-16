using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class HealthPresenter : MonoBehaviour
{
    private GameObject _boss;
    private GameObject _player;
    [SerializeField] private GameObject[] _images1;
    [SerializeField] private GameObject[] _images2;
    private int _index1;
    private int _index2;

    private void Awake()
    {
        if (GameManager._instance?._boss == null)
        {
            _boss = GameObject.FindWithTag("Boss");
            _player = GameObject.FindWithTag("Player");
        }
        else
        {
            _boss = GameManager._instance._boss;
            _player = GameManager._instance._player;
        }
        _boss.GetComponent<Boss>().OnAttackSuccess -= HpDecrease;
        _boss.GetComponent<Boss>().OnAttackSuccess += HpDecrease;
        _player.GetComponent<Player>().OnHpDecrease -= HpDecrease2;
        _player.GetComponent<Player>().OnHpDecrease += HpDecrease2;
    }

    private void OnDestroy()
    {
        if (_boss != null)
        {
            _boss.GetComponent<Boss>().OnAttackSuccess -= HpDecrease;
        }
        if(_player != null)
        {
            _player.GetComponent<Player>().OnHpDecrease -= HpDecrease2;
        }
    }

    private void HpDecrease()
    {
        _images1[_index1].SetActive(false);
        ++_index1;
    }
    private void HpDecrease2()
    {
        _images2[_index2].SetActive(false);
        ++_index2;
    }
}

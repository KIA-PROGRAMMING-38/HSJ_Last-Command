using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class HealthPresenter : MonoBehaviour
{
    private GameObject _boss;
    [SerializeField] private GameObject[] _images;
    private int _index;

    private void Awake()
    {
        if (GameManager._instance?._boss == null)
        {
            _boss = GameObject.FindWithTag("Boss");
        }
        else
        {
            _boss = GameManager._instance._boss;
        }
        _boss.GetComponent<Boss>().OnAttackSuccess -= HpDecrease;
        _boss.GetComponent<Boss>().OnAttackSuccess += HpDecrease;
        _index = 0;
    }

    private void OnDestroy()
    {
        if (_boss != null)
        {
            _boss.GetComponent<Boss>().OnAttackSuccess -= HpDecrease;
        }
    }

    private void HpDecrease()
    {
        _images[_index].SetActive(false);
        ++_index;
    }
}

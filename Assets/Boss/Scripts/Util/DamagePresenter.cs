using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePresenter : MonoBehaviour
{
    private Boss _boss;
    private BossGroggy _groggyState;
    [SerializeField] private Image[] _images;

    private void Awake()
    {
        if (GameManager._instance?._boss == null)
        {
            _boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();
        }
        else
        {
            _boss = GameManager._instance._boss.GetComponent<Boss>(); ;
        }

        _groggyState = _boss.GetComponent<Animator>().GetBehaviour<BossGroggy>();

        _groggyState.OnGroggyEnd -= ResetFill;
        _groggyState.OnGroggyEnd += ResetFill;
        _boss.OnTempChange -= ChangeTemp;
        _boss.OnTempChange += ChangeTemp;
        _boss.OnConfChange -= ChangeConf;
        _boss.OnConfChange += ChangeConf;
        _boss.OnGroggy -= EnterGroggy;
        _boss.OnGroggy += EnterGroggy;
    }

    private void OnDestroy()
    {
        if (_boss != null)
        {
            _boss.OnTempChange -= ChangeTemp;
            _boss.OnConfChange -= ChangeConf;
            _boss.OnGroggy -= EnterGroggy;
        }
        if(_groggyState != null)
        {
            _groggyState.OnGroggyEnd -= ResetFill;
        }
    }


    private void ChangeTemp()
    {
        _images[1].fillAmount = Mathf.Min(1, (_boss._temporaryDamageGain + _boss._confirmedDamageGain) / _boss.GetTreshold());
        Debug.Log(_images[1].fillAmount);
    }
    private void ChangeConf()
    {
        _images[2].fillAmount = Mathf.Min(1, _boss._totalDamageGain / _boss.GetTreshold());
    }
    private void EnterGroggy()
    {
        _images[2].fillAmount = 1;
    }
    private void ResetFill()
    {
        _images[1].fillAmount = 0;
        _images[2].fillAmount = 0;
    }
}

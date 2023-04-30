using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroggyTimer : MonoBehaviour
{
    private Text _timer;
    private BossGroggy _bossGroggy;

    private void Awake()
    {
        _timer = transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>();
        _bossGroggy = transform.GetComponentInParent<Boss>()._objectManager._bossGroggy;
        _bossGroggy.OnGroggyTime -= SetTime;
        _bossGroggy.OnGroggyTime += SetTime;
    }

    private void OnDestroy()
    {
        _bossGroggy.OnGroggyTime -= SetTime;
    }
    private void SetTime(float time)
    {
        int sec = (int)time / 1;
        int milSec = (int)((time - sec) * 100);
        _timer.text = $"{sec} : {milSec}";
    }
}

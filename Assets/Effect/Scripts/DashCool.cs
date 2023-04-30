using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCool : MonoBehaviour
{
    private PlayerInput _input;
    [SerializeField] private GameObject[] _coolUI;
    private Image _coolDown;

    private void Awake()
    {
        _coolDown = _coolUI[0].GetComponent<Image>();
        _input = GetComponentInParent<PlayerInput>();
        _input.OnDirectionChanged -= ChangeRotation;
        _input.OnDirectionChanged += ChangeRotation;
        _input.OnDashReady -= ChargeDash;
        _input.OnDashReady += ChargeDash;
        _input.OnDashUsed -= UseDash;
        _input.OnDashUsed += UseDash;
        _input.OnWaitDash -= InformCool;
        _input.OnWaitDash += InformCool;
    }

    private void OnDestroy()
    {
        _input.OnDirectionChanged -= ChangeRotation;
        _input.OnDashReady -= ChargeDash;
        _input.OnDashUsed -= UseDash;
        _input.OnWaitDash -= InformCool;
    }

    public void ChangeRotation() => transform.rotation = _input._playerDirection._rotation;
    public void ChargeDash()
    {
        _coolDown.fillAmount = 0;
        _coolUI[1].SetActive(true);
        _coolUI[0].SetActive(false);
    }
    public void UseDash ()
    {
        _coolUI[1].SetActive(false);
        _coolUI[0].SetActive(true);
    }
    public void InformCool(float elapsedTime, float dashWaitTime) => _coolDown.fillAmount = elapsedTime / (2 * dashWaitTime);
 }

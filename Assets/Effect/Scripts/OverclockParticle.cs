using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverclockParticle : MonoBehaviour
{
    PlayerInput _input;
    private void Awake()
    {
        _input = GetComponentInParent<PlayerInput>();
    }
    private void Update()
    {
        transform.rotation = _input._playerDirection._rotation;
    }
}

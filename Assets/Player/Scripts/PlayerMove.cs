using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Vector2[] _headPosition;
    [SerializeField] private float _moveSpeed;

    private void Awake()
    {
        _playerInput = gameObject.GetComponent<PlayerInput>();
    }
    private void FixedUpdate()
    {

    }
}

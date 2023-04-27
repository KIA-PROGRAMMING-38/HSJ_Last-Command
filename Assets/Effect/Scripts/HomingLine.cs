using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingLine : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private float _elapsedTime;
    private float _speed = 0.3f;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        _elapsedTime = 0;
        _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 0);
    }
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, _elapsedTime/ _speed);
        if(_elapsedTime > 0.7f)
        {
            gameObject.SetActive(false);
        }
    }
}

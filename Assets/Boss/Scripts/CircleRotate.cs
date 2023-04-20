using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotate : MonoBehaviour
{
    [SerializeField] private GameObject[] _circles;
    private int _num;
    [SerializeField] private float _rotateSpeed;
    private void Awake()
    {
        _num = _circles.Length;
    }
    private void Update()
    {
        for(int i = 0; i < _num; ++i)
        {
            float rotation = _rotateSpeed * Time.deltaTime;
            if(i % 2 == 0)
            {
                _circles[i].transform.Rotate(0,0, rotation);
            }
            else
            {
                _circles[i].transform.Rotate(0, 0, -rotation);
            }
        }
    }
}

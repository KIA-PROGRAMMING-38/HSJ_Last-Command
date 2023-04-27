using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    private float _rotateSpeed = 30;
    [SerializeField] private GameObject _warningPrefab;
    private WheelWarning _warning;
    private void Awake()
    {
        gameObject.SetActive(false);
        _warning = Instantiate(_warningPrefab, transform.position, transform.rotation).GetComponent<WheelWarning>();
        _warning.SetWheel(gameObject);
    }
    private void FixedUpdate()
    {
        float rotation = _rotateSpeed * Time.fixedDeltaTime;
        transform.Rotate(0, 0, rotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnergySpawner : MonoBehaviour
{
    [SerializeField] private CreatedEnergy _createdEnergyPrefab;
    private CreatedEnergy _createdEnergy;
    private int _previousPoint;
    private int _currentPoint;
    private Transform _targetPoint;

    private void Awake()
    {
        _currentPoint = Random.Range(0, transform.childCount);
        _previousPoint = -1;

        if (_createdEnergy != null)
        {
            _createdEnergy.OnRespawn -= SetRandomPosition;
            _createdEnergy.OnRespawn += SetRandomPosition;
        }

        SpawnEnergy();
    }

    private void OnDestroy()
    {
        if (_createdEnergy != null)
        {
            _createdEnergy.OnRespawn -= SetRandomPosition;
        }
    }

    private Transform SetRandomPosition()
    {
        while (_currentPoint == _previousPoint)
        {
            _currentPoint = Random.Range(0, transform.childCount);
        }
        _previousPoint = _currentPoint;
        return transform.GetChild(_currentPoint);
    }

    private void SpawnEnergy()
    {
        _targetPoint = SetRandomPosition();
        _createdEnergy = Instantiate(_createdEnergyPrefab, _targetPoint.position, _targetPoint.rotation);
        _createdEnergy.OnRespawn += SetRandomPosition;
    }
    public void ClearEnergy()
    {
        _createdEnergy.gameObject.SetActive(false);
    }
}
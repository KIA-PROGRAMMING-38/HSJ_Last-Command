using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _snakeSprite;
    private int _defaultLength;
    [SerializeField] private int _maxLength;
    private int _currentLength;
    [SerializeField] private int _frameDelay;

    private GameObject[] _heads;
    private bool[] _energy;
    private int _chargedEnergy;

    private void Awake()
    {
        _defaultLength = 3;
        _currentLength = _defaultLength;
        _heads = new GameObject[_maxLength];
        _heads[0] = gameObject;

        _energy = new bool[_maxLength - _defaultLength];
        _chargedEnergy = 0;

        for(int i = 1; i < _maxLength; ++i)
        {
            GameObject newHead = Instantiate(_snakeSprite, transform);
            _heads[i] = newHead;
            Head headComponent = newHead.AddComponent<Head>();
            headComponent._frameDelay = _frameDelay;
            headComponent._frontHead = _heads[i - 1].transform;
        }
        for(int i = _maxLength; i > _defaultLength; --i)
        {
            transform.GetChild(i - 2).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EarnEnergy()
    {
        _currentLength++;
        if(_currentLength > _maxLength)
        {
            _currentLength--;
            return;
        }
        else
        {
            transform.GetChild(_currentLength - 2).gameObject.SetActive(true);
            _energy[_chargedEnergy] = true;
            ++_chargedEnergy;
        }
    }
}

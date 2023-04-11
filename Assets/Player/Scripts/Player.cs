using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _snakeSprite;
    private int _defaultLength; 
    private void Awake()
    {
        _defaultLength = 3;
        for(int i = 0; i < _defaultLength; ++i)
        {
            Instantiate(_snakeSprite, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

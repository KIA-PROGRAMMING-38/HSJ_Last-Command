using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public GameObject _player;
    public GameObject _boss;
    private void Awake()
    {
        _instance = this;
    }
}

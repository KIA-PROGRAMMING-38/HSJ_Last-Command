using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Util.Manager;

public class ObjectManager : MonoBehaviour
{
    public GameManager _gameManager { get; private set; }
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Boss _bossPrefab;
    [SerializeField] private EnergySpawner _energySpawnerPrefab;
    [SerializeField] private Block _blockPrefab;

    public Player _player { get; private set; }
    public Boss _boss { get; private set; }
    public BossGroggy _bossGroggy { get; private set; }
    public BossDie _bossDie { get; private set; }
    public EnergySpawner _energySpawner { get; private set; }
    public Block _block { get; private set; }
    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    private void Awake()
    {
        InstantiateObjects();
        _player.Init(this);
        _boss.Init(this);
        BindEvents();
    }
    private void OnDestroy()
    {
        UnbindEvents();
    }
    public void InstantiateObjects()
    {
        _player = Instantiate(_playerPrefab);
        _boss = Instantiate(_bossPrefab);
        _bossGroggy = _boss.GetComponent<Animator>().GetBehaviour<BossGroggy>();
        _bossDie = _boss.transform.GetChild(5).GetComponent<BossDie>();
        _energySpawner = Instantiate(_energySpawnerPrefab);
        _block = Instantiate(_blockPrefab);
    }

    private void BindEvents()
    {
        _boss.OnDamageChange -= _player.Invincible;
        _boss.OnDamageChange += _player.Invincible;
        _boss.OnDie -= ClearObject;
        _boss.OnDie += ClearObject;
        _boss.OnDie -= ClearPlayer;
        _boss.OnDie += ClearPlayer;
        _player.OnDie -= ClearObject;
        _player.OnDie += ClearObject;
        _player.OnDie -= ClearBoss;
        _player.OnDie += ClearBoss;
    }
    private void UnbindEvents()
    {
        _boss.OnDamageChange -= _player.Invincible;
        _boss.OnDie -= ClearObject;
        _boss.OnDie -= ClearPlayer;
        _player.OnDie -= ClearObject;
        _player.OnDie -= ClearBoss;
    }
    private void ClearObject()
    {
        _energySpawner.gameObject.SetActive(false);
        _energySpawner.ClearEnergy();
        _block.gameObject.SetActive(false);
    }
    public void ClearBoss() => _boss.gameObject.SetActive(false);
    public void ClearPlayer() => _player.gameObject.SetActive(false);
}

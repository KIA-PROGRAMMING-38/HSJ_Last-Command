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
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _wheelPrefab;
    public Player _player { get; private set; }
    public PlayerAnalyze _playerAnalyze { get; private set; }
    public PlayerIdleMove _playerIdle { get; private set; }
    public Boss _boss { get; private set; }
    public BossGroggy _bossGroggy { get; private set; }
    public BossDie _bossDie { get; private set; }
    public EnergySpawner _energySpawner { get; private set; }
    public GameObject _block { get; private set; }
    public GameObject _wall { get; private set; }
    public GameObject _wheel { get; private set; }
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
        _playerAnalyze = _player.GetComponent<Animator>().GetBehaviour<PlayerAnalyze>();
        _playerIdle = _player.GetComponent<Animator>().GetBehaviour<PlayerIdleMove>();
        _boss = Instantiate(_bossPrefab);
        _bossGroggy = _boss.GetComponent<Animator>().GetBehaviour<BossGroggy>();
        _bossDie = _boss.transform.GetChild(5).GetComponent<BossDie>();
        _energySpawner = Instantiate(_energySpawnerPrefab);
        _wall = Instantiate(_wallPrefab);
    }

    private void BindEvents()
    {
        _boss.OnDamageChange -= _player.Invincible;
        _boss.OnDamageChange += _player.Invincible;
        _bossGroggy.OnGroggyEnd -= _boss.EndGroggy;
        _bossGroggy.OnGroggyEnd += _boss.EndGroggy;
        _bossGroggy.OnAttackSuccess -= _player.Invincible;
        _bossGroggy.OnAttackSuccess += _player.Invincible;
        _bossGroggy.OnAttackSuccess -= _player.HealHP;
        _bossGroggy.OnAttackSuccess += _player.HealHP;
        _boss.OnDie -= ClearObject;
        _boss.OnDie += ClearObject;
        _boss.OnDie -= ClearPlayer;
        _boss.OnDie += ClearPlayer;
        _player.OnDie -= ClearObject;
        _player.OnDie += ClearObject;
        _player.OnDie -= ClearBoss;
        _player.OnDie += ClearBoss;
        _player.OnOverclock -= _playerAnalyze.OnOverclock;
        _player.OnOverclock += _playerAnalyze.OnOverclock;
        _player.OnOverclockEnd -= _playerAnalyze.OnOverclockEnd;
        _player.OnOverclockEnd += _playerAnalyze.OnOverclockEnd;
        _player.OnHpDecrease -= _playerAnalyze.Damaged;
        _player.OnHpDecrease += _playerAnalyze.Damaged;
        _playerAnalyze.OnAnalyze -= _player.SetAnalyzeBoxTrue;
        _playerAnalyze.OnAnalyze += _player.SetAnalyzeBoxTrue;
        _playerAnalyze.OffAnalyzing -= _player.SetAnalyzeBoxFalse;
        _playerAnalyze.OffAnalyzing += _player.SetAnalyzeBoxFalse;
        _player.OnOverclock -= _playerIdle.OnOverclock;
        _player.OnOverclock += _playerIdle.OnOverclock;
        _player.OnOverclockEnd -= _playerIdle.OnOverclockEnd;
        _player.OnOverclockEnd += _playerIdle.OnOverclockEnd;

    }
    private void UnbindEvents()
    {
        _boss.OnDamageChange -= _player.Invincible;
        _bossGroggy.OnGroggyEnd -= _boss.EndGroggy;
        _bossGroggy.OnAttackSuccess -= _player.Invincible;
        _bossGroggy.OnAttackSuccess -= _player.HealHP;
        _boss.OnDie -= ClearObject;
        _boss.OnDie -= ClearPlayer;
        _player.OnDie -= ClearObject;
        _player.OnDie -= ClearBoss;
        _player.OnOverclock -= _playerAnalyze.OnOverclock;
        _player.OnOverclockEnd -= _playerAnalyze.OnOverclockEnd;
        _player.OnHpDecrease -= _playerAnalyze.Damaged;
        _player.OnOverclock -= _playerIdle.OnOverclock;
        _player.OnOverclockEnd -= _playerIdle.OnOverclockEnd;

        _playerAnalyze.OnAnalyze -= _player.SetAnalyzeBoxTrue;
        _playerAnalyze.OffAnalyzing -= _player.SetAnalyzeBoxFalse;
    }
    private void ClearObject()
    {
        _energySpawner.gameObject.SetActive(false);
        _energySpawner.ClearEnergy();
        if(_block != null)
        {
            ClearBlock();
        }
        if(_wheel != null)
        {
            ClearWheel();
        }
    }
    public void ClearBoss() => _boss.gameObject.SetActive(false);
    public void ClearPlayer() => _player.gameObject.SetActive(false);
    public void CreateBlock() => _block = Instantiate(_blockPrefab);
    public void ClearBlock() => _block.SetActive(false);
    public void CreateWheel() => _wheel = Instantiate(_wheelPrefab);
    public void ClearWheel() => _wheel.SetActive(false);
}

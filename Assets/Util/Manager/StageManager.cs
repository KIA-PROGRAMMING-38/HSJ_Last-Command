using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Util.Manager;

public class StageManager : MonoBehaviour
{
    [SerializeField] private ObjectManager _objectManagerPrefab;
    [SerializeField] private UIManager _uiManagerPrefab;
    [SerializeField] private Pattern _patternManagerPrefab;

    private ObjectManager _objectManager;
    private UIManager _uiManager;
    private Pattern _patternManager;
    private RankManager _rankManager;

    private void Awake()
    {
        StartGame();
    }
    private void OnDestroy()
    {
        UnbindEvents();
    }
    private void StartGame()
    {
        _uiManager = Instantiate(_uiManagerPrefab);
        _uiManager.Init(this);
        _uiManager._startEffect.OnGameStart -= InitSettings;
        _uiManager._startEffect.OnGameStart += InitSettings;
    }
    private void InitSettings()
    {
        _objectManager = Instantiate(_objectManagerPrefab);
        _objectManager.Init(this);
        _patternManager = Instantiate(_patternManagerPrefab);
        _patternManager.Init(this);
        _rankManager = gameObject.AddComponent<RankManager>();
        _uiManager.SetUI(_objectManager._player, _objectManager._boss);
        _patternManager.SetTransform(_objectManager._boss.transform, _objectManager._player.transform);
        BindEvents();
    }

    private void BindEvents()
    {
        _objectManager._player.OnGameStart -= _patternManager.StartGame;
        _objectManager._player.OnGameStart += _patternManager.StartGame;
        _objectManager._bossDie.OnAnimationFinished -= _rankManager.GameClear;
        _objectManager._bossDie.OnAnimationFinished += _rankManager.GameClear;
        _objectManager._player.OnDieEnd -= _rankManager.GameOver;
        _objectManager._player.OnDieEnd += _rankManager.GameOver;
        _objectManager._boss.OnDie -= _uiManager.RemoveUI;
        _objectManager._boss.OnDie += _uiManager.RemoveUI;
        _objectManager._player.OnDie -= _uiManager.RemoveUI;
        _objectManager._player.OnDie += _uiManager.RemoveUI;
        _objectManager._boss.OnDie -= _patternManager.DestroyMissiles;
        _objectManager._boss.OnDie += _patternManager.DestroyMissiles;
        _objectManager._player.OnDie -= _patternManager.DestroyMissiles;
        _objectManager._player.OnDie += _patternManager.DestroyMissiles;
        _objectManager._boss.OnDie += _uiManager.RemoveUI;
        _objectManager._boss.OnAttackSuccess -= _uiManager.BossHpDecrease;
        _objectManager._boss.OnAttackSuccess += _uiManager.BossHpDecrease;
        _objectManager._player.OnHpDecrease -= _uiManager.PlayerHpDecrease;
        _objectManager._player.OnHpDecrease += _uiManager.PlayerHpDecrease;
        _objectManager._player.OnHpDecrease -= _uiManager.ShakeUI;
        _objectManager._player.OnHpDecrease += _uiManager.ShakeUI;
        _objectManager._player.OnHpDecrease -= _uiManager.BreathUI;
        _objectManager._player.OnHpDecrease += _uiManager.BreathUI;
        _objectManager._player.OnHpDecrease -= _rankManager.AddHitCount;
        _objectManager._player.OnHpDecrease += _rankManager.AddHitCount;
        _objectManager._player.OnHpDecrease -= _rankManager.StopTime;
        _objectManager._player.OnHpDecrease += _rankManager.StopTime;
        _objectManager._player.OnHpIncrease -= _uiManager.PlayerHpIncrease;
        _objectManager._player.OnHpIncrease += _uiManager.PlayerHpIncrease;
        _objectManager._player.OnEarnEnergy -= _uiManager.ChangeText;
        _objectManager._player.OnEarnEnergy += _uiManager.ChangeText;
        _objectManager._playerAnalyze.OnAnalyzing -= _uiManager.ChangeFill;
        _objectManager._playerAnalyze.OnAnalyzing += _uiManager.ChangeFill;
        _objectManager._bossGroggy.OnGroggyEnd -= _uiManager.ResetFill;
        _objectManager._bossGroggy.OnGroggyEnd += _uiManager.ResetFill;
        _objectManager._boss.OnTempChange -= _uiManager.ChangeTemp;
        _objectManager._boss.OnTempChange += _uiManager.ChangeTemp;
        _objectManager._boss.OnConfChange -= _uiManager.ChangeConf;
        _objectManager._boss.OnConfChange += _uiManager.ChangeConf;
        _objectManager._boss.OnGroggy -= _uiManager.EnterGroggy;
        _objectManager._boss.OnGroggy += _uiManager.EnterGroggy;
        _objectManager._boss.OnAttackSuccess -= _rankManager.StopTime;
        _objectManager._boss.OnAttackSuccess += _rankManager.StopTime;
        _objectManager._boss.OnAttackSuccess -= _patternManager.ChangePattern;
        _objectManager._boss.OnAttackSuccess += _patternManager.ChangePattern;

        _rankManager.OnGameClear -= _objectManager.ClearBoss;
        _rankManager.OnGameClear += _objectManager.ClearBoss;
        _rankManager.OnGameClear -= _uiManager._gameClearUI.CalculateScore;
        _rankManager.OnGameClear += _uiManager._gameClearUI.CalculateScore;
        _rankManager.OnGameClear -= _uiManager.ShowGameClearUI;
        _rankManager.OnGameClear += _uiManager.ShowGameClearUI;
        _rankManager.OnGameClearUI -= _uiManager._gameClearUI.SetUI;
        _rankManager.OnGameClearUI += _uiManager._gameClearUI.SetUI;
        _rankManager.OnGameOver -= _uiManager.ShowGameOverUI;
        _rankManager.OnGameOver += _uiManager.ShowGameOverUI;

        _patternManager.OnBossMove -= _objectManager._boss._bossPattern.PrepareAttack;
        _patternManager.OnBossMove += _objectManager._boss._bossPattern.PrepareAttack;
        _patternManager.OnBossAttack -= _objectManager._boss._bossPattern.SetPosition;
        _patternManager.OnBossAttack += _objectManager._boss._bossPattern.SetPosition;
        _patternManager.OnBlockPatternStart -= _objectManager.CreateBlock;
        _patternManager.OnBlockPatternStart += _objectManager.CreateBlock;
        _patternManager.OnWheelPatternStart -= _objectManager.CreateWheel;
        _patternManager.OnWheelPatternStart += _objectManager.CreateWheel;
    }

    private void UnbindEvents()
    {
        _objectManager._player.OnGameStart -= _patternManager.StartGame;
        _objectManager._bossDie.OnAnimationFinished -= _rankManager.GameClear;
        _objectManager._boss.OnDie -= _uiManager.RemoveUI;
        _objectManager._boss.OnDie -= _patternManager.DestroyMissiles;
        _objectManager._player.OnDie -= _uiManager.RemoveUI;
        _objectManager._player.OnDie -= _patternManager.DestroyMissiles;
        _objectManager._player.OnDieEnd -= _rankManager.GameOver;
        _objectManager._boss.OnAttackSuccess -= _uiManager.BossHpDecrease;
        _objectManager._player.OnHpDecrease -= _uiManager.PlayerHpDecrease;
        _objectManager._player.OnHpDecrease -= _uiManager.ShakeUI;
        _objectManager._player.OnHpDecrease -= _rankManager.AddHitCount;
        _objectManager._player.OnHpDecrease -= _rankManager.StopTime;
        _objectManager._player.OnHpDecrease -= _uiManager.BreathUI;
        _objectManager._player.OnHpIncrease -= _uiManager.PlayerHpIncrease;
        _objectManager._player.OnEarnEnergy -= _uiManager.ChangeText;
        _objectManager._playerAnalyze.OnAnalyzing -= _uiManager.ChangeFill;
        _objectManager._bossGroggy.OnGroggyEnd -= _uiManager.ResetFill;
        _objectManager._boss.OnTempChange -= _uiManager.ChangeTemp;
        _objectManager._boss.OnConfChange -= _uiManager.ChangeConf;
        _objectManager._boss.OnGroggy -= _uiManager.EnterGroggy;
        _objectManager._boss.OnAttackSuccess -= _patternManager.ChangePattern;
        _objectManager._boss.OnAttackSuccess -= _rankManager.StopTime;

        _rankManager.OnGameClear -= _objectManager.ClearBoss;
        _rankManager.OnGameClear -= _uiManager.ShowGameClearUI;
        _rankManager.OnGameClear -= _uiManager._gameClearUI.CalculateScore;
        _rankManager.OnGameClearUI -= _uiManager._gameClearUI.SetUI;
        _rankManager.OnGameOver -= _uiManager.ShowGameOverUI;

        _patternManager.OnBossMove -= _objectManager._boss._bossPattern.PrepareAttack;
        _patternManager.OnBossAttack -= _objectManager._boss._bossPattern.SetPosition;
        _patternManager.OnBlockPatternStart -= _objectManager.CreateBlock;
        _patternManager.OnWheelPatternStart -= _objectManager.CreateWheel;
        _uiManager._startEffect.OnGameStart -= InitSettings;
    }
}

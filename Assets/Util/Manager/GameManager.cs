using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Util.Manager;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ObjectManager _objectManagerPrefab;
    [SerializeField] private UIManager _uiManagerPrefab;
    [SerializeField] private PatternManager _patternManagerPrefab;
    [SerializeField] private StageManager _stageManagerPrefab;

    private ObjectManager _objectManager;
    private UIManager _uiManager;
    private PatternManager _patternManager;
    private StageManager _stageManager;

    private void Awake()
    {
        InitSettings();
        _uiManager.SetUI(_objectManager._player, _objectManager._boss);
        _patternManager.SetTransform(_objectManager._boss.transform, _objectManager._player.transform);
        BindEvents();
    }
    private void OnDestroy()
    {
        UnbindEvents();
    }
    private void InitSettings()
    {
        _objectManager = Instantiate(_objectManagerPrefab);
        _objectManager.Init(this);
        _uiManager = Instantiate(_uiManagerPrefab);
        _uiManager.Init(this);
        _patternManager = Instantiate(_patternManagerPrefab);
        _patternManager.Init(this);
        _stageManager = Instantiate(_stageManagerPrefab);
        _stageManager.Init(this);
    }

    private void BindEvents()
    {
        _objectManager._player.OnGameStart -= _patternManager.StartGame;
        _objectManager._player.OnGameStart += _patternManager.StartGame;
        _objectManager._bossDie.OnAnimationFinished -= _stageManager.GameClear;
        _objectManager._bossDie.OnAnimationFinished += _stageManager.GameClear;
        _objectManager._player.OnDieEnd -= _stageManager.GameOver;
        _objectManager._player.OnDieEnd += _stageManager.GameOver;
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
        _objectManager._player.OnHpDecrease -= _stageManager.AddHitCount;
        _objectManager._player.OnHpDecrease += _stageManager.AddHitCount;
        _objectManager._player.OnHpDecrease -= _stageManager.StopTime;
        _objectManager._player.OnHpDecrease += _stageManager.StopTime;
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
        _objectManager._boss.OnAttackSuccess -= _stageManager.StopTime;
        _objectManager._boss.OnAttackSuccess += _stageManager.StopTime;
        _objectManager._boss.OnAttackSuccess -= _patternManager.ChangePattern;
        _objectManager._boss.OnAttackSuccess += _patternManager.ChangePattern;

        _stageManager.OnGameClear -= _objectManager.ClearBoss;
        _stageManager.OnGameClear += _objectManager.ClearBoss;
        _stageManager.OnGameClear -= _uiManager._gameClearUI.CalculateScore;
        _stageManager.OnGameClear += _uiManager._gameClearUI.CalculateScore;
        _stageManager.OnGameClear -= _uiManager.ShowGameClearUI;
        _stageManager.OnGameClear += _uiManager.ShowGameClearUI;
        _stageManager.OnGameClearUI -= _uiManager._gameClearUI.SetUI;
        _stageManager.OnGameClearUI += _uiManager._gameClearUI.SetUI;
        _stageManager.OnGameOver -= _uiManager.ShowGameOverUI;
        _stageManager.OnGameOver += _uiManager.ShowGameOverUI;

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
        _objectManager._bossDie.OnAnimationFinished -= _stageManager.GameClear;
        _objectManager._boss.OnDie -= _uiManager.RemoveUI;
        _objectManager._boss.OnDie -= _patternManager.DestroyMissiles;
        _objectManager._player.OnDie -= _uiManager.RemoveUI;
        _objectManager._player.OnDie -= _patternManager.DestroyMissiles;
        _objectManager._player.OnDieEnd -= _stageManager.GameOver;
        _objectManager._boss.OnAttackSuccess -= _uiManager.BossHpDecrease;
        _objectManager._player.OnHpDecrease -= _uiManager.PlayerHpDecrease;
        _objectManager._player.OnHpDecrease -= _uiManager.ShakeUI;
        _objectManager._player.OnHpDecrease -= _stageManager.AddHitCount;
        _objectManager._player.OnHpDecrease -= _stageManager.StopTime;
        _objectManager._player.OnHpDecrease -= _uiManager.BreathUI;
        _objectManager._player.OnHpIncrease -= _uiManager.PlayerHpIncrease;
        _objectManager._player.OnEarnEnergy -= _uiManager.ChangeText;
        _objectManager._playerAnalyze.OnAnalyzing -= _uiManager.ChangeFill;
        _objectManager._bossGroggy.OnGroggyEnd -= _uiManager.ResetFill;
        _objectManager._boss.OnTempChange -= _uiManager.ChangeTemp;
        _objectManager._boss.OnConfChange -= _uiManager.ChangeConf;
        _objectManager._boss.OnGroggy -= _uiManager.EnterGroggy;
        _objectManager._boss.OnAttackSuccess -= _patternManager.ChangePattern;
        _objectManager._boss.OnAttackSuccess -= _stageManager.StopTime;

        _stageManager.OnGameClear -= _objectManager.ClearBoss;
        _stageManager.OnGameClear -= _uiManager.ShowGameClearUI;
        _stageManager.OnGameClear -= _uiManager._gameClearUI.CalculateScore;
        _stageManager.OnGameClearUI -= _uiManager._gameClearUI.SetUI;
        _stageManager.OnGameOver -= _uiManager.ShowGameOverUI;

        _patternManager.OnBossMove -= _objectManager._boss._bossPattern.PrepareAttack;
        _patternManager.OnBossAttack -= _objectManager._boss._bossPattern.SetPosition;
        _patternManager.OnBlockPatternStart -= _objectManager.CreateBlock;
        _patternManager.OnWheelPatternStart -= _objectManager.CreateWheel;
    }

}

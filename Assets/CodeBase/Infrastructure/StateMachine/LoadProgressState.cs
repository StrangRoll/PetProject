using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;

namespace CodeBase.Infrastructure.StateMachine
{
    public class LoadProgressState : IState
    {
        private const int StandartMaxHp = 50;
        private const float StandartDamage = 1;
        private const float StandartDamageRadius = .5f;
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly SaveLoadService saveLoadService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
        }

        public void Exit()
        {
            
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();
        }

        private PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress(SceneNames.Level);

            progress.HeroState.MaxHP = StandartMaxHp;
            progress.HeroState.ResetHP();
            progress.HeroStats.Damage = StandartDamage;
            progress.HeroStats.DamageRadius = StandartDamageRadius;
            
            return progress;
        }
    }
}
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId MonsterTypeId;
        
        [SerializeField] private bool _slain;
        
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        public string Id { get; set; }

        public void Init(IGameFactory factory) => 
            _factory = factory;

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(Id))
                _slain = true;
            else
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            var monster = _factory.CreateMonster(MonsterTypeId, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>(); 
            _enemyDeath.EnemyDied += OnEnemyDied;
        }

        private void OnEnemyDied()
        {
            if (_enemyDeath == null)
                return;
            
            _enemyDeath.EnemyDied -= OnEnemyDied;
            _slain = true;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain)
                progress.KillData.ClearedSpawners.Add(Id);
        }
    }
}
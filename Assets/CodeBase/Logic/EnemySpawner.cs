using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(UniqueId))]
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        [FormerlySerializedAs("_monsterTypeId")] public MonsterTypeId MonsterTypeId;
        [FormerlySerializedAs("_id")] public string Id;
        
        [SerializeField] private bool _slain;
        
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        private void Awake()
        {
            Id = GetComponent<UniqueId>().Id;
            _factory = AllServices.Container.Single<IGameFactory>();
        }
        
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
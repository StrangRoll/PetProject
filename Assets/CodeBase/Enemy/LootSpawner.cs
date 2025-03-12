using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyDeath _enemyDeath;
        
        private IGameFactory _factory;
        private int _lootMin;
        private int _lootMax;

        private void OnEnable()
        {
            _enemyDeath.EnemyDied += OnEnemyDied;;
        }
        
        private void OnDisable()
        {
            _enemyDeath.EnemyDied -= OnEnemyDied;;
        }
        
        public void Init(IGameFactory factory) 
            => _factory = factory;

        public void SetLoot(int min, int max)
        {
            _lootMin = min;
            _lootMax = max;
        }
        
        private void OnEnemyDied()
        {
            SpawnLoot();
            _enemyDeath.EnemyDied -= OnEnemyDied;
        }

        private void SpawnLoot()
        {
            var loot = _factory.CreateLoot();
            loot.transform.position = transform.position;

            var lootItem = new Loot();
            lootItem.Init(Random.Range(_lootMin, _lootMax));
            
            loot.Init(lootItem);
        }
    }
}
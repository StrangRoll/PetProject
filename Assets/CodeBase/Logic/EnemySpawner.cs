using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(UniqueId))]
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private MonsterTypeId _monsterTypeId;
        
        private string _id;
        [SerializeField] private bool _slain;
        
        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
        }
        
        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(_id))
                _slain = true;
            else
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain)
                progress.KillData.ClearedSpawners.Add(_id);
        }
    }
}
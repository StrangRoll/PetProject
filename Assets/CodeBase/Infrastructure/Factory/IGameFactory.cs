using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject CreateHero(Vector3 at);
        GameObject HeroGameObject { get; }
        GameObject CreateHud();
        void CreateSpawner(Vector3 position, string spawnerId, MonsterTypeId spawnerMonsterTypeId);
        void Register(ISavedProgressReader progressReader);
        void CleanUp();
        Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent);
        LootPiece CreateLoot();
    }
}
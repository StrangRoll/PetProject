using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject CreateHero(InitialPoint at);
        GameObject HeroGameObject { get; }
        GameObject CreateHud();
        void Register(ISavedProgressReader progressReader);
        void CleanUp();
        GameObject CreateMonster(MonsterTypeId typeId, Transform parent);
    }
}
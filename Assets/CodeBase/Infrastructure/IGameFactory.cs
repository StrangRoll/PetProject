using UnityEngine;

namespace CodeBase.Infrastructure
{
    public interface IGameFactory
    {
        GameObject CreateHero(InitialPoint at);
        void CreateHud();
    }
}
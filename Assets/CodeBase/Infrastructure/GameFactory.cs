using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameFactory : IGameFactory
    {
        private const string HeroPath = "Hero/hero";
        public const string HudPath = "Hud/Hud";

        public GameObject CreateHero(InitialPoint at)
        {
            return InstantiatePrefab(HeroPath, at.transform.position);
        }
        
        public void CreateHud()
        {
            InstantiatePrefab(HudPath);
        }

        private static GameObject InstantiatePrefab(string path, Vector3 at = default)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity); 
        }
    }
}
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Logic
{
    public class UncollectedLootChecker : IUncollectedLootChecker
    {
        private Dictionary<Loot, PositionOnLevel> _lootPosition = new Dictionary<Loot, PositionOnLevel>();
        private IGameFactory _factory;

        public void Init(IGameFactory factory)
        {
            _factory = factory;
        }
        
        public void AddNewLoot(Loot loot, LootPiece lootPiece)
        {
            _lootPosition.Add(loot, GetPositionOnLevel(lootPiece.transform));
            lootPiece.Picked += OnLootPicked;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            foreach (var lootData in progress.WorldData.LootData.UncollectedLoot)
            {
                if (lootData.PositionOnLevel.Level == SceneManager.GetActiveScene().name)
                {
                    var lootPiece = _factory.CreateLoot();
                    lootPiece.Init(lootData.Loot);
                    lootPiece.transform.position = lootData.PositionOnLevel.Position.AsUnityVector3();
                    AddNewLoot(lootData.Loot, lootPiece);
                }
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.LootData.UncollectedLoot.Clear();

            foreach (var loot in _lootPosition) 
                progress.WorldData.LootData.AddUncollectedLoot(loot.Key, loot.Value);
        }

        private PositionOnLevel GetPositionOnLevel(Transform transform)
        {
            var position = transform.position;
            var level = SceneManager.GetActiveScene().name; 
            var positionOnLevel = new PositionOnLevel(level, position.AsVector3Data());
            
            return positionOnLevel;
        }

        private void OnLootPicked(Loot loot, LootPiece lootPiece)
        {
            _lootPosition.Remove(loot);
            lootPiece.Picked -= OnLootPicked;
        }
    }
}
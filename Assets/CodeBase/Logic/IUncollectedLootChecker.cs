using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;

namespace CodeBase.Logic
{
    public interface IUncollectedLootChecker : IService, ISavedProgress
    {
        public void Init(IGameFactory factory);
        void AddNewLoot(Loot loot, LootPiece lootPiece);
    }
}
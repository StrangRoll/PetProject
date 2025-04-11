using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class ShopWindow : WindowBase
    {
        [SerializeField] private TMP_Text _skullText;

        protected override void Init() =>
            RefreshSkullText();
        protected override void SubscribeUpdates() => 
            PlayerProgress.WorldData.LootData.Changed += OnLootChanged;

        protected override void Cleanup() => 
            PlayerProgress.WorldData.LootData.Changed -= OnLootChanged;
        
        private void OnLootChanged() => 
            RefreshSkullText();

        private void RefreshSkullText() => 
            _skullText.text = $"{PlayerProgress.WorldData.LootData.Collected}";
    }
}
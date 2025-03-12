using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class LootCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counter;
        
        private WorldData _worldData;

        private void Start()
        {
            UpdateCounter();
        }

        public void Init(WorldData worldData)
        {
            _worldData = worldData;
            _worldData.LootData.Changed += OnLootChanged;
        }

        private void OnLootChanged() => 
            UpdateCounter();

        private void UpdateCounter()
        {
            _counter.text = $"{_worldData.LootData.Collected}";
        }
    }
}
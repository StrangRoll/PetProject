using System.Collections;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Hero
{
    public class LootPiece : MonoBehaviour
    {
        [SerializeField] private GameObject _lootVisual;
        [SerializeField] private ParticleSystem _pickupFxPrefab;
        
        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;
        private float _destoryTimer = 1.5f;

        public void Construct(WorldData worldData) => 
            _worldData = worldData;

        public void Init(Loot loot)
        {
            _loot = loot;
        }

        private void OnTriggerEnter(Collider other) => Pickup();

        private void Pickup()
        {
            if (_picked) 
                return;
            
            _picked = true;
            UpdateWorldData();
            
            _lootVisual.SetActive(false);
            
            PlayPickupFx();
            StartCoroutine(StartDestroyTimer());
        }

        private void UpdateWorldData()
        {
            _worldData.LootData.Collect(_loot);
        }

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(_destoryTimer);
        }

        private void PlayPickupFx()
        {
            Instantiate(_pickupFxPrefab, transform.position, Quaternion.identity);
        }
    }
}
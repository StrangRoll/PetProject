using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId MonsterTypeId;

        [Range(1, 10)] public int MinLoot;
        [Range(1, 100)] public int MaxLoot;
        
        [Range(1, 10)] public int Hp;
        [Range(1f, 30f)] public float Damage;

        [Range(0.5f, 1f)] public float Cleavage;
        [Range(1f, 10f)] public float MoveSpeed;
        
        [FormerlySerializedAs("Prefab")] public AssetReferenceGameObject PrefabReference;
    }
}
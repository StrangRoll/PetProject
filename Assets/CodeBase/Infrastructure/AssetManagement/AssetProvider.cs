using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCash = new Dictionary<string, AsyncOperationHandle>();

        public GameObject InstantiatePrefab(string path, Vector3 at = default)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity); 
        }

        public async Task<T> Load<T>(AssetReferenceGameObject assetReference) where T : class
        {
            if (_completedCash.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completeHandle))
                return completeHandle.Result as T;
            
            var handle = Addressables.LoadAssetAsync<T>(assetReference);
            
            handle.Completed += _ =>
            {
                _completedCash[assetReference.AssetGUID] = _;
            };

            return await handle.Task;
        }
    }
}
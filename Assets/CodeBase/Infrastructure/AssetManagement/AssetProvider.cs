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
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handels = new Dictionary<string, List<AsyncOperationHandle>>();

        public GameObject InstantiatePrefab(string path, Vector3 at = default)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity); 
        }

        public async Task<T> Load<T>(AssetReferenceGameObject assetReference) where T : class
        {
            if (_completedCash.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completeHandle))
                return completeHandle.Result as T;
            
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference);
            
            handle.Completed += _ =>
            {
                _completedCash[assetReference.AssetGUID] = _;
            };

            CreateHandle(assetReference.AssetGUID, handle);

            return await handle.Task;
        }

        public void CleanUp()
        {
            foreach (var resourceHandles in _handels.Values)
            {
                foreach (var handle in resourceHandles) 
                    Addressables.Release(handle);
            }
        }
        
        private void CreateHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handels.TryGetValue(key, out List<AsyncOperationHandle> resouceHandles))
            {
                resouceHandles = new List<AsyncOperationHandle>();
                _handels[key] = resouceHandles;
            }

            resouceHandles.Add(handle);
        }
    }
}
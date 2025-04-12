using System.Threading.Tasks;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssetProvider : IService
    {
        GameObject InstantiatePrefab(string path, Vector3 at = default);
        Task<T> Load<T>(AssetReferenceGameObject assetReference) where T : class;
        void CleanUp();
    }
}
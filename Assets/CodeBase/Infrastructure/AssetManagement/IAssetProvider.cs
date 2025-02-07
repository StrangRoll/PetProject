using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssetProvider
    {
        GameObject InstantiatePrefab(string path, Vector3 at = default);
    }
}
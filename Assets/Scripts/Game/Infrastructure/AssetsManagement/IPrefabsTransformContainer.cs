using UnityEngine;

namespace Game.Infrastructure.AssetsManagement
{
    public interface IPrefabsTransformContainer
    {
        public Transform GetPrefabTransform(Prefab prefabType);
        public void AddTransform(Prefab prefabType, Transform transform);
    }
}
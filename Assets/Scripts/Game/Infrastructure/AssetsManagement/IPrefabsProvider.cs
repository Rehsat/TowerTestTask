using UnityEngine;

namespace Game.Infrastructure.AssetsManagement
{
    public interface IPrefabsProvider
    { // Если прям упороться в ISP то можно разделить на 2 интерфейса, но не вижу в этом смысла
        public GameObject GetPrefab(Prefab prefabType);
        public TComponent GetPrefabsComponent<TComponent>(Prefab prefabType) where TComponent : Component;
    }
}
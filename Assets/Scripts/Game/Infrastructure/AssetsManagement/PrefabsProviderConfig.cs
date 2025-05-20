using System;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;

namespace Game.Infrastructure.AssetsManagement
{
    [CreateAssetMenu(menuName = "GameConfigs/PrefabsContainer", fileName = "PrefabsContainer")]
    public class PrefabsProviderConfig : ScriptableObject, IPrefabsProvider
    {
        [SerializeField] private SerializableDictionary<Prefab, GameObject> _prefabs;
        public GameObject GetPrefab(Prefab prefabType)
        {
            return _prefabs[prefabType];
        }

        public TComponent GetPrefabsComponent<TComponent>(Prefab prefabType) where TComponent : Component
        {
            if (GetPrefab(prefabType).TryGetComponent<TComponent>(out var component))
                return component;

            throw new Exception($"There is no component{typeof(TComponent)} on {prefabType}");
        }
    }
}
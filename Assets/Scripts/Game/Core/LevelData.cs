using System;
using Game.Infrastructure.AssetsManagement;
using Game.Infrastructure.CurrentLevelData;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    public class LevelData : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<Prefab, GameObject> _prefabs;

        [Inject]
        public void Construct(ICurrentLevelDataProvider levelDataProvider)
        {
            levelDataProvider.SetCurrentLevelData(this);
        }
        public GameObject GetPrefab(Prefab prefabType)
        {
            if (_prefabs.ContainsKey(prefabType))
                return _prefabs[prefabType];
            
            throw new Exception($"There is no prefab type of {prefabType} on the current scene");
        }

        public TComponent GetPrefabsComponent<TComponent>(Prefab prefabType) where TComponent : Component
        {
            if (GetPrefab(prefabType).TryGetComponent<TComponent>(out var component))
                return component;

            throw new Exception($"There is no component{typeof(TComponent)} on {prefabType}");
        }
    }
}

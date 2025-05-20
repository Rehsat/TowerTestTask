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
        //Дублирование кода с PrefabsProviderConfig. Надо выделить класс отдельный со всей этой историей, но словарь сериализуемый
        // не позволяет. Надо будет найти ему замену, а пока думаю не слишком критично
        
        [SerializeField] private SerializableDictionary<Prefab, GameObject> _prefabs;

        [Inject]
        public void Construct(ICurrentLevelDataProvider levelDataProvider)
        {
            levelDataProvider.SetCurrentLevelData(this);
        }
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

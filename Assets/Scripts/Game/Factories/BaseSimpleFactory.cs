using Game.Infrastructure.AssetsManagement;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Factories
{
    public abstract class BaseSimpleFactory<TPrefabType> : IFactory<TPrefabType> where TPrefabType : MonoBehaviour
    {
        private Transform _spawnPosition;
        private TPrefabType _prefab;
        public BaseSimpleFactory(IPrefabsProvider prefabsProvider, IPrefabsTransformContainer prefabsTransformContainer)
        {
            var prefabId = GetPrefabType();
            _spawnPosition = prefabsTransformContainer.GetPrefabTransform(prefabId);
            _prefab = prefabsProvider.GetPrefabsComponent<TPrefabType>(prefabId);
        }
        public TPrefabType Create()
        {
            var instantinatedObject = Object.Instantiate(
                _prefab, 
                _spawnPosition,
                true); //может быть null, не вижу в этом проблем
            return instantinatedObject;
        }

        protected abstract Prefab GetPrefabType();
    }
}

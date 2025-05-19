using Game.Infrastructure.AssetsManagement;
using UnityEngine;
using Zenject;

namespace Game.Factories
{
    public abstract class BaseSimpleFactory<TPrefabType> : IFactory<TPrefabType> where TPrefabType : MonoBehaviour
    {
        private Transform _spawnPosition;
        private TPrefabType _prefab;
        public BaseSimpleFactory(IPrefabsContainer prefabsContainer, IPrefabsTransformContainer prefabsTransformContainer)
        {
            var prefabId = GetPrefabType();
            _spawnPosition = prefabsTransformContainer.GetPrefabTransform(prefabId);
            _prefab = prefabsContainer.GetPrefabsComponent<TPrefabType>(prefabId);
        }
        public TPrefabType Create()
        {
            var instantinatedObject = Object.Instantiate(
                _prefab, 
                _spawnPosition, //может быть null, не вижу в этом проблем
                true);

            return instantinatedObject;
        }

        protected abstract Prefab GetPrefabType();
    }
}

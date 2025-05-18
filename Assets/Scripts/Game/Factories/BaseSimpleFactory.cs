using Game.Infrastructure.AssetsManagement;
using UnityEngine;
using Zenject;

namespace Game.Factories
{
    public abstract class BaseSimpleFactory<TPrefabType> : IFactory<TPrefabType> where TPrefabType : MonoBehaviour
    {
        private TPrefabType _prefab;
        public BaseSimpleFactory(IPrefabsContainer prefabsContainer)
        {
            _prefab = prefabsContainer.GetPrefabsComponent<TPrefabType>(GetPrefabType());
        }
        public TPrefabType Create()
        {
            return Object.Instantiate(_prefab);
        }

        protected abstract Prefab GetPrefabType();
    }
}

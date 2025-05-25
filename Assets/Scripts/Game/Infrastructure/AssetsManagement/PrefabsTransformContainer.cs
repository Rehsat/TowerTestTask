using System;
using Game.Infrastructure.ScreenScale;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure.AssetsManagement
{
    public class PrefabsTransformContainer : MonoBehaviour, IPrefabsTransformContainer
    {
        [SerializeField] private SerializableDictionary<Prefab, Transform> _positions;
        private CompositeDisposable _compositeDisposable;

        public Transform GetPrefabTransform(Prefab prefab)
        {
            if (_positions.ContainsKey(prefab))
                return _positions[prefab];
            
            return null;
        }

        public void AddTransform(Prefab prefabType, Transform transform)
        {
            if (_positions.ContainsKey(prefabType))
            {
                Debug.LogError($"You tried to add existing prefab transform for {prefabType}");
                return;
            }
            _positions.Add(prefabType, transform);
        }

        private void OnDestroy()
        {
            _compositeDisposable?.Dispose();
        }
    }
}
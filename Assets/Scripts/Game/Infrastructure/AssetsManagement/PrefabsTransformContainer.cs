using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;

namespace Game.Infrastructure.AssetsManagement
{
    public class PrefabsTransformContainer : MonoBehaviour, IPrefabsTransformContainer
    {
        [SerializeField] private SerializableDictionary<Prefab, Transform> _positions;
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
    }
}
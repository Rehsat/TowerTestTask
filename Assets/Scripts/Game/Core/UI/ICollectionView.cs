using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.UI
{
    public interface ICollectionView<TCollectionObjectType> where TCollectionObjectType : MonoBehaviour
    {
        public void SetListOfObjects(List<TCollectionObjectType> listOfObjects);
    }
}
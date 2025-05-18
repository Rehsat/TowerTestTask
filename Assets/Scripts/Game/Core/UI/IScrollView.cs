using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.UI
{
    public interface IScrollView<TScrollObjectType> where TScrollObjectType : MonoBehaviour
    {
        public void SetObjectsToScroll(List<TScrollObjectType> listOfObjects);
    }
}
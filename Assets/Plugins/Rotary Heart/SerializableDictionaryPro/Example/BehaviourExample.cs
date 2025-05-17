using UnityEngine;

namespace RotaryHeart.Lib.SerializableDictionaryPro
{
    public class BehaviourExample : MonoBehaviour
    {
        [System.Serializable]
        public class MyDict : SerializableDictionary<int, bool>
        {
            public MyDict() : base(false, false, false, true) { }
        }
        [System.Serializable]
        public class MyDict2 : SerializableOrderedDictionary<int, bool>
        {
            public MyDict2() : base(false, false, false, true) { }
        }

        [System.Serializable]
        public class MyDict3 : SerializableSortedDictionary<int, bool>
        {
            public MyDict3() : base(false, false, false, true) { }
        }

        public MyDict dictionary;
        public MyDict2 orderedDictionary;
        public MyDict3 sortedDictionary;

        // Use this for initialization
        void Start()
        {
            for (int i = 0; i < 15; i++)
            {
                dictionary.Add(i, false);
                orderedDictionary.Add(i, false);
                sortedDictionary.Add(i, false);
            }

            for (int i = 0; i < 4; i++)
            {
                int removal = Random.Range(0, 15);

                dictionary.Remove(removal);
                orderedDictionary.Remove(removal);
                sortedDictionary.Remove(removal);
            }

            for (int i = 15; i < 20; i++)
            {
                dictionary.Add(i, true);
                orderedDictionary.Add(i, true);
                sortedDictionary.Add(i, true);
            }
        }
    }
}
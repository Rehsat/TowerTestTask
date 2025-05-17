using System.Collections.Generic;
using UnityEditor;

namespace RotaryHeart.Lib.SerializableDictionaryPro
{
    [InitializeOnLoad]
    public class SerializableDictionaryProDefiner : Definer
    {
        static SerializableDictionaryProDefiner()
        {
            List<string> defines = new List<string>(1)
            {
                "RH_SerializedDictionary"
            };
            
            ApplyDefines(defines);
        }
    }
}
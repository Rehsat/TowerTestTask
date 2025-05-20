using System;
using UnityEditor;
using UnityEngine;

namespace Game.Core.Figures.Configs
{
    [CreateAssetMenu(menuName = "GameConfigs/FigureConfigContainer", fileName = "FigureConfigContainer")]
    public class FigureConfigContainer : ScriptableObject, IFiguresConfigContainer
    {
        [SerializeField] private bool _updateName;
        [SerializeField] private FigureConfig _figureConfig;
        public FigureConfig FigureConfig => _figureConfig;

        private void OnValidate()
        {
            //сгенерировал нейронкой, меняет имя объекта на тип+цвет при нажатии на "update name"
#if UNITY_EDITOR
            if (_updateName)
            {
                _updateName = false;
            
                string newName = $"{_figureConfig.FigureType}{_figureConfig.FigureColor}";
                string assetPath = AssetDatabase.GetAssetPath(this);
            
                if (!string.IsNullOrEmpty(assetPath))
                {
                    // Меняем имя ассета и объекта
                    AssetDatabase.RenameAsset(assetPath, newName);
                    name = newName; // Обновляем имя объекта (необязательно, но для согласованности)
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    Debug.LogWarning("Asset not saved yet. Save the asset before renaming.");
                }

                EditorUtility.SetDirty(this); // Помечаем объект как измененный
            }
#endif
        }
    }
}
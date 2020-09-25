using UnityEditor;
using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    [CustomEditor(typeof(PlanetPlaneMesh))]
    public class PlanetPlaneMeshEditor : Editor
    {
        PlanetPlaneMesh planet;
        Editor shapeEditor;

        public override void OnInspectorGUI()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();
                if (check.changed)
                {
                    planet.GeneratePlanet();
                }
            }

            DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingFoldout, ref shapeEditor);
        }

        void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
        {
            if (settings != null)
            {
                foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    if (foldout)
                    {
                        CreateCachedEditor(settings, null, ref editor);
                        editor.OnInspectorGUI();

                        if (check.changed)
                        {
                            onSettingsUpdated?.Invoke();
                        }
                    }
                }
            }
        }

        private void OnEnable()
        {
            planet = (PlanetPlaneMesh)target;
        }
    }
}

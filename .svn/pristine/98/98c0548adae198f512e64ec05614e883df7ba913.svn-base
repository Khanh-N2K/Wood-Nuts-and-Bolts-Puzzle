using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animas
{
#if UNITY_EDITOR
    using UnityEditor;
    [CustomEditor(typeof(PlateScrewHole))]
    public class PlateScrewHoleEditor : Editor
    {
        void OnEnable()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
            SceneView.duringSceneGui += this.OnSceneGUI;
        }

        void OnDisable()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }

        void OnSceneGUI(SceneView sceneView)
        {
            CheckKey();
        }
        void CheckKey()
        {
            Event e = Event.current;
            if (e.type == EventType.MouseUp)
            {
                FixScrewBoardPosition();
            }
        }

        void FixScrewBoardPosition()
        {
            PlateScrewHole plateScrewHole = target as PlateScrewHole;
            ScrewBoard screwBoard = FindObjectOfType<ScrewBoard>();

            Plate[] plates = screwBoard.GetComponentsInChildren<Plate>();
            foreach (var plate in plates)
            {
                foreach (var screwHole in plate.listScrewHole)
                {
                    if (Vector2.Distance(screwHole.transform.position, plateScrewHole.transform.position) < .2f)
                    {
                        plateScrewHole.transform.position = screwHole.transform.position;
                        return;
                    }
                }
            }
        }
    }
#endif
}

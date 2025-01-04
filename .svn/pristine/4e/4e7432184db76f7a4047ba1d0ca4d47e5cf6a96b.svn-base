using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animas
{

#if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(Screw))]
    public class ScrewEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        void OnEnable()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
            SceneView.duringSceneGui += this.OnSceneGUI;
        }

        void OnDisable()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            CheckKey();
        }
        private void CheckKey()
        {
            Event e = Event.current;
            if (e.type == EventType.MouseUp)
            {
                OnMouseUp();
            }
            if (e.type == EventType.MouseDrag)
            {
                OnMouseDrag();
            }
        }

        private void OnMouseUp()
        {
            Screw screw = target as Screw;
            screw.EditorPin();
        }

        private void OnMouseDrag()
        {
            Screw screw = target as Screw;
            if (screw.PlateScrewHoles != null && screw.PlateScrewHoles.Count > 0)
            {
                float dis = Vector2.Distance(screw.transform.position, screw.BoardScrewHole.transform.position);
                if (dis > .2f)
                {
                    screw.UnPin(true);
                }
            }
        }
    }
#endif
}

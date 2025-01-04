using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animas
{
    using Unity.VisualScripting;
#if UNITY_EDITOR
    using UnityEditor;
    [CustomEditor(typeof(Plate))]
    public class PlateEditor : Editor
    {
        public float rotate = 30;
        public List<Color> colors = new List<Color>()
        {
            new Color(127f/255,221f/255,99f/255),//xanh la
            new Color(147/255,228f/255,231f/255),//hong tim
            new Color(243f/255,164f/255,174f/255),// xanh bien
            new Color(250f/255,248f/255,131f/255),//vang
            new Color(255f/255,158f/255,242f/255),//tim nhat
            new Color(1,1,1),//trang
        };
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Plate plate = target as Plate;

            // if (GUILayout.Button("TTTTT"))
            // {
            //     plate.transform.localScale = 1.4f * plate.transform.parent.localScale;
            // }

            GUILayout.Space(3);
            if (GUILayout.Button("Rotate"))
            {
                Rotate(plate);
            }
            GUILayout.Space(3);
            // if (GUILayout.Button("Change Color"))
            // {
            //     ChangeColor(plate);
            // }
            if (GUILayout.Button("GenHole"))
            {
                GenHole(plate);
            }
            GUILayout.Space(3);
            if (GUILayout.Button("Click If Delete Screw"))
            {
                Fix(plate);
            }

            EditorUtility.SetDirty(plate);
        }
        public void ChangeColor(Plate plate)
        {
            plate.GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Count)];
        }
        public void GenHole(Plate plate)
        {
            PlateScrewHole plateScrewHole = Resources.Load<PlateScrewHole>("ScrewMapItem/PlateScrewHole");
            var obj = PrefabUtility.InstantiatePrefab(plateScrewHole, plate.transform);
            plateScrewHole = obj.GetComponent<PlateScrewHole>();
            plateScrewHole.Plate = plate;
            plate.SetGlobalScale(Vector3.one * 1.2f, plateScrewHole.transform);

            Fix(plate);
        }

        public void Fix(Plate plate)
        {
            plate.listScrewHole.Clear();
            PlateScrewHole[] plateScrewHoles = plate.GetComponentsInChildren<PlateScrewHole>();
            foreach (var item in plateScrewHoles)
            {
                plate.listScrewHole.Add(item);
            }
        }

        public void Rotate(Plate plate)
        {
            plate.transform.eulerAngles += Vector3.forward * rotate;
            plate.OriginRot = plate.transform.eulerAngles;
        }
    }
#endif
}

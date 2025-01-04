using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animas
{
#if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(PlateMask))]
    public class PlateMaskEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PlateMask plateMask = target as PlateMask;

            SpriteRenderer plateRenderer = plateMask.transform.parent.GetComponent<SpriteRenderer>();
            SpriteMask spriteMask = plateMask.GetComponent<SpriteMask>();
            spriteMask.frontSortingLayerID = plateRenderer.sortingLayerID;
            spriteMask.backSortingLayerID = plateRenderer.sortingLayerID;
            spriteMask.frontSortingOrder = plateRenderer.sortingOrder;
            spriteMask.backSortingOrder = plateRenderer.sortingOrder - 1;
        }
    }
#endif
    public class PlateMask : MonoBehaviour
    {
    }
}

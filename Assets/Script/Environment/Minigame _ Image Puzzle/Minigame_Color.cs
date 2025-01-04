using UnityEngine;

namespace Minigame_ImagePuzzle
{
    public class Minigame_Color : MonoBehaviour
    {
        [HideInInspector] public Color color;

        public void SetColor(Color newColor)
        {
            SpriteRenderer model = GetComponent<SpriteRenderer>();
            model.color = newColor;
            this.color = newColor;
        }
    }
}

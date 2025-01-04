using UnityEngine;

public class ResetPlateLayers : MonoBehaviour
{
    private void Reset()
    {
        for(int i=0; i<transform.childCount; i++)
        {
            Plate[] plates = GetComponentsInChildren<Plate>();
            foreach(Plate plate in plates)
                plate.Reset();
        }
    }
}

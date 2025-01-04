using UnityEngine;

public class BoltRemoval: MonoBehaviour
{
    private void Start()
    {
        LevelSpawner.Instance.onLevelSpawned += this.StopRemoveBolt;
    }

    public static void OnRemoveBolt()
    {
        InputManager.Instance.OnRemoveBolt(true);
        HighlightBolts(true);
        SupportFunctions_UI.Instance.ShowSupportFunctions(false, false);
        InstructionManager.Instance.ShowInstruction("Click a bolt to remove it");
    }

    private static void HighlightBolts(bool highlight)
    {
        Config.Instance.boltsManager.HighLightBolts(highlight);
    }

    public static void EndRemoveBolt()
    {
        HighlightBolts(false);
        SupportFunctions_UI.Instance.ShowSupportFunctions(true, false);
        InstructionManager.Instance.HideInstruction();
    }

    private void StopRemoveBolt()
    {
        InputManager.Instance.OnRemoveBolt(false);
        EndRemoveBolt();
    }
}

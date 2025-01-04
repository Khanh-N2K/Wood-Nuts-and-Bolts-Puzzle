using UnityEngine;

public class BombManager: MonoBehaviour
{
    public void ActiveBomb()
    {
        gameObject.SetActive(true);
        InstructionManager.Instance.ShowInstruction("Drag the bomb to blow away hanging plates");
        SupportFunctions_UI.Instance.ShowSupportFunctions(false, true);
    }

    public void DeactiveBomb()
    {
        InstructionManager.Instance.HideInstruction();
        SupportFunctions_UI.Instance.ShowSupportFunctions(true, true);
        gameObject.SetActive(false);
    }
}

using CandyCoded.HapticFeedback;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public static bool allowVibrate = true;

    public static void Vibrate()
    {
        if(allowVibrate)
            HapticFeedback.HeavyFeedback();
    }

    public static void PlayButtonSfx()
    {
        SoundManager.Instance.PlaySFX(SoundEffect.Button);
    }

    public static void PlayButton2Sfx()
    {
        SoundManager.Instance.PlaySFX(SoundEffect.Button2);
    }

    public static void PlayPopupSfx()
    {
        SoundManager.Instance.PlaySFX(SoundEffect.PopupShow);
    }
}

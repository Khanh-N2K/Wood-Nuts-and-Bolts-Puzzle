using UnityEngine.Events;

public class CustomEvent
{
    public event UnityAction OnBoltCountChanged;

    public void AddListener(UnityAction listener)
    {
        OnBoltCountChanged += listener;
    }

    public void RemoveListener(UnityAction listener)
    {
        OnBoltCountChanged -= listener;
    }

    public void ClearAllListeners()
    {
        OnBoltCountChanged = null;
    }

    public void Invoke()
    {
        OnBoltCountChanged?.Invoke();
    }
}

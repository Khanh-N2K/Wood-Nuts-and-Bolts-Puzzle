using System.Collections.Generic;

public static class StateManager
{
    private static Stack<LastMovedState> stateStack = new();
    public static int StateCount { get => stateStack.Count; }

    public static CustomEvent onStateCountChanged = new();

    public static void SaveState(LastMovedState lastState)
    {
        stateStack.Push(lastState);
        onStateCountChanged?.Invoke();
    }

    public static LastMovedState GetLastState()
    {
        if (StateCount == 0)
            return null;

        LastMovedState lastMovedState = stateStack.Pop();
        onStateCountChanged?.Invoke();
        return lastMovedState;
    }

    public static void OnNewRound()
    {
        stateStack.Clear();
        onStateCountChanged?.Invoke();
    }
}

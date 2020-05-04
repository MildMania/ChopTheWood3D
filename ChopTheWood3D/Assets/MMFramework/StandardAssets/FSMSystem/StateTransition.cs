using System;

public class StateTransition<T1, T2> 
    where T1 : IConvertible
    where T2 : IConvertible
{
    public T1 CurState;
    public T2 Transition;

    public StateTransition(T1 curState, T2 transition)
    {
        CurState = curState;
        Transition = transition;
    }

    public override int GetHashCode()
    {
        return 17 + 31 * CurState.GetHashCode() + 31 * Transition.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        StateTransition<T1, T2> other = obj as StateTransition<T1, T2>;

        if (other != null
            && CurState.Equals(other.CurState)
            && Transition.Equals(other.Transition))
            return true;

        return false;
    }
}

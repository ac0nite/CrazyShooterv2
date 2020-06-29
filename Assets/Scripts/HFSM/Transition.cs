using System;

namespace ShoriGames.HFSM
{
    public interface ITransition
    {
        IState OwnerState { get; }
        IState TargetState { get; }

        int Priority { get; }

        bool CanTransit();
    }

    public abstract class Transition<TOwner, TTarget> : ITransition
        where TOwner : IState
        where TTarget : IState
    {
        public readonly TOwner OwnerState;
        public readonly TTarget TargetState;

        IState ITransition.OwnerState { get { return OwnerState; } }
        IState ITransition.TargetState { get { return TargetState; } }

        public int Priority { get; set; }

        public Transition(TOwner ownerState, TTarget targetState, int priority = 0)
        {
            OwnerState = ownerState;
            TargetState = targetState;
            Priority = priority;
        }

        public abstract bool CanTransit();
    }

    public sealed class StateFinishedTransition : Transition<IState, IState>
    {
        public StateFinishedTransition(IState ownerState, IState targetState, int priority = 0) : base(ownerState, targetState, priority) { }

        public override bool CanTransit()
        {
            return OwnerState.IsFinished;
        }
    }
}

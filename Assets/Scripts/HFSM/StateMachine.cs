using System.Collections.Generic;
using System.Linq;

namespace ShoriGames.HFSM
{
    public class StateMachine
    {
        public sealed class ExitState : State { }

        public IState CurrentState { get; private set; }

        public readonly List<IState> States;
        public readonly List<ITransition> Transitions;

        public readonly IState EntryState;

        public static StateMachine Create(IState entryState, params ITransition[] transitions)
        {
            var statesSet = new HashSet<IState>();
            var transitionsSet = new HashSet<ITransition>();

            foreach (var transition in transitions)
            {
                transitionsSet.Add(transition);
                statesSet.Add(transition.OwnerState);
                statesSet.Add(transition.TargetState);
            }

            return new StateMachine(statesSet.ToList(), transitionsSet.ToList(), entryState);
        }

        public StateMachine(List<IState> states, List<ITransition> transitions, IState entryState)
        {
            States = states;
            Transitions = transitions;
            EntryState = entryState;
            CurrentState = EntryState;

            foreach (var transition in Transitions)
            {
                if (!transition.OwnerState.Transitions.Contains(transition))
                {
                    transition.OwnerState.AddTransitions(transition);
                }
            }
        }
        
        public virtual void Start()
        {
            foreach (var state in States)
            {
                state.Init();
            }

            CurrentState = EntryState;
            CurrentState.OnStateEnter(null);
        }

        public virtual void Stop()
        {
            CurrentState.OnStateExit(null);
            CurrentState = null;

            foreach (var state in States)
            {
                state.DeInit();
            }
        }

        public virtual bool Transit()
        {
            IState targetState;
            if (CurrentState.Transit(out targetState))
            {
                PerformStateTransition(targetState);
                return true;
            }

            return false;
        }
        
        public virtual void Update()
        {
            CurrentState.Update();
        }

        public void ForceSetState(IState targetState)
        {
#if UNITY_EDITOR
            if (!States.Contains(targetState))
            {
                throw new System.Exception("Trying to force set state which is not part of state machine, state type = " + targetState.GetType());
            }
#endif

            PerformStateTransition(targetState);
        }

        public virtual bool IsReadyToExit()
        {
            return CurrentState.GetType() == typeof(ExitState);
        }

        private void PerformStateTransition(IState targetState)
        {
            var previousState = CurrentState;
            previousState.OnStateExit(targetState);

            CurrentState = targetState;
            CurrentState.OnStateEnter(previousState);
        }
    }
}

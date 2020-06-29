using System;
using System.Collections.Generic;

namespace ShoriGames.HFSM
{
    public interface IState
    {
        bool IsFinished { get; }

        /// <summary>
        /// List of possible transitions, sorted by priority descending
        /// </summary>
        List<ITransition> Transitions { get; }

        void Init(); // Called for each state on StateMachine.Start()
        void DeInit(); // Called for each state on StateMachine.Stop()

        void OnStateEnter(IState fromState);
        void Update();
        void OnStateExit(IState toState);

        bool Transit(out IState targetState);

        void AddTransitions(params ITransition[] newTransitions);
    }

    public abstract class State : IState
    {
        public virtual bool IsFinished { get; protected set; }
        public List<ITransition> Transitions { get; private set; }

        private bool m_isInited = false;

        public State()
        {
            Transitions = new List<ITransition>();
        }

        public virtual void Init()
        {
            if (m_isInited == true) { throw new Exception("State " + GetType() + " is already inited."); }
            m_isInited = true;
        }

        public virtual void DeInit()
        {
            if (m_isInited == false) { throw new Exception("State " + GetType() + " is already deinited."); }
            m_isInited = false;
        }

        public virtual bool Transit(out IState targetState)
        {
            for (int i = 0; i < Transitions.Count; ++i)
            {
                if (Transitions[i].CanTransit())
                {
                    targetState = Transitions[i].TargetState;
                    return true;
                }
            }

            targetState = null;
            return false;
        }

        public virtual void OnStateEnter(IState fromState)
        {
            IsFinished = false;
            /*UnityEngine.Debug.Log(GetType() + ": OnStateEnter");*/
        }
        public virtual void Update() { /*UnityEngine.Debug.Log(GetType() + ": Update");*/ }
        public virtual void OnStateExit(IState toState) { /*UnityEngine.Debug.Log(GetType() + ": OnStateExit");*/ }

        public void AddTransitions(params ITransition[] newTransitions)
        {
            Transitions.AddRange(newTransitions);
            Transitions.Sort((x, y) => y.Priority.CompareTo(x.Priority));
        }
    }
}

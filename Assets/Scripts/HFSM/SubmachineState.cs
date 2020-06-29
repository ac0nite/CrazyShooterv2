namespace ShoriGames.HFSM
{
    public class SubmachineState : State
    {
        public override bool IsFinished { get { return Submachine.IsReadyToExit(); } }

        public readonly StateMachine Submachine;

        public SubmachineState(StateMachine submachine)
            : base()
        {
            Submachine = submachine;
        }

        public override void OnStateEnter(IState fromState)
        {
            base.OnStateEnter(fromState);
            Submachine.Start();
        }

        public override void OnStateExit(IState toState)
        {
            base.OnStateExit(toState);
            Submachine.Stop();
        }

        public override void Update()
        {
            base.Update();
            Submachine.Transit();
            Submachine.Update();
        }
    }
}

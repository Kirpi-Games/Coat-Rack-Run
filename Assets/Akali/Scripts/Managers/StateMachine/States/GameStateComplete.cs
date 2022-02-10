namespace Akali.Scripts.Managers.StateMachine.States
{
    public class GameStateComplete : GameStateBase
    {
        public GameEvents.GameStateEvent onEnter;
        public GameEvents.GameStateEvent onExecute;
        public GameEvents.GameStateEvent onExit;

        public override void Enter()
        {
            onEnter?.Invoke();
        }

        public override void Execute()
        {
            onExecute?.Invoke();
        }

        public override void Exit()
        {
            onExit?.Invoke();
        }
    }
}
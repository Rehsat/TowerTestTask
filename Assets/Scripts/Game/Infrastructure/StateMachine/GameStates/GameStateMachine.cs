using System.Collections.Generic;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine : StateMachine<IGameState> 
    {
        public GameStateMachine(List<IGameState> states) : base(states)
        {
            foreach (var state in States.Values)
            {
                state.SetStateMachine(this);
            }
        }
    }
}


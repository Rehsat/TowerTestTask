﻿namespace Infrastructure.StateMachine
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        
    }
}
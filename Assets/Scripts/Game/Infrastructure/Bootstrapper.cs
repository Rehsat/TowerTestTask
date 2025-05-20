using Game.Infrastructure.StateMachine.GameStates;
using Infrastructure.StateMachine;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    
/*
        Приветствую великий код-читающий, во многих местах думаю есть оверинжениринг для этой задачи, но сделан он был осознанно 
        из-за требования к "обновляемости" например стейт машина тут не особо нужна, но если для обновляемости - самое то
        Приятного прочтения) Буду крайне благодарен за фидбек, а за подробный фидбек - буду руки целовать (по желанию)
*/      
    public class Bootstrapper : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine;

        [Inject]
        public void Construct(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        public void Awake() =>
            _gameStateMachine.EnterState<BootstrapGameState>();
    }
}

using Game.Infrastructure.StateMachine;
using Game.Infrastructure.StateMachine.GameStates;
using Infrastructure.StateMachine;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    //Делаем точку входа на отдельной сцеене, где будет только бустрап, чтоб создать гарантию,
    //что ничто на сцене не инициализируется раньше бутстрапа + чтоб потенциально сдеелать экран загрузки
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

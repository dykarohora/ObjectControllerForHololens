using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UniRx;
using UnityEngine;

namespace ObjectController.Menu
{
    public class MenuButton : MonoBehaviour, IInputClickHandler
    {
        [SerializeField]
        private ControllerButtonType _buttonType;

        private readonly Subject<ControllerButtonType> _onClickedButton = new Subject<ControllerButtonType>();

        public IObservable<ControllerButtonType> OnClickeAsObservable => _onClickedButton;

        public void OnInputClicked(InputClickedEventData eventData)
        {
            _onClickedButton.OnNext(_buttonType);
        }

        public void SetButtonColor(ControllerButtonType currentButton)
        {
            GetComponent<Renderer>().material.color = 
                _buttonType == currentButton ? Color.green : Color.gray;
        }
    }
}
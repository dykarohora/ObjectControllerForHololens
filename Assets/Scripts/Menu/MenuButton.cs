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

        // ボタンがクリックされたらSubjectを発火
        public void OnInputClicked(InputClickedEventData eventData)
        {
            _onClickedButton.OnNext(_buttonType);
        }

        // UIロジック
        // ステートに応じてボタンの色を変更する
        public void SetButtonColor(ControllerButtonType currentButton)
        {
            GetComponent<Renderer>().material.color = 
                _buttonType == currentButton ? Color.green : Color.gray;
        }
    }
}
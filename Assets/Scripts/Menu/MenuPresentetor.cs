using ObjectController.Adjuster;
using UniRx;
using UnityEngine;

namespace ObjectController.Menu
{
    public class MenuPresentetor : MonoBehaviour
    {
        private void Start()
        {
            var buttons = GetComponentsInChildren<MenuButton>();

            // ボタンのクリックイベントをサブスクライブ
            foreach (var button in buttons)
            {
                button.OnClickeAsObservable
                    .Subscribe(buttonType =>
                    {
                        AdjusterManager.Instance.SetCurrentAdjuster(buttonType);
                    })
                    .AddTo(gameObject);
            }

            // Adjusterのステート変更イベントをサブスクライブ
            AdjusterManager.Instance.CurrentState
                .Subscribe(state =>
                {
                    foreach (var button in buttons)
                    {
                        button.SetButtonColor(state);
                    }
                })
                .AddTo(gameObject);
        }
    }

}


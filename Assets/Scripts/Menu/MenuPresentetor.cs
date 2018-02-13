using System.Collections;
using System.Collections.Generic;
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

            foreach (var button in buttons)
            {
                button.OnClickeAsObservable
                    .Subscribe(buttonType =>
                    {
                        AdjusterManager.Instance.SetCurrentAdjuster(buttonType);
                    })
                    .AddTo(gameObject);
            }

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


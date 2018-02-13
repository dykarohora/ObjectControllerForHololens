using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class TestManipulator : MonoBehaviour, IManipulationHandler
{
    private bool _isManipulating = false;

    private Vector3 _lastNavigatePos = Vector3.zero;
    private Vector3 _navigateVelocity = Vector3.zero;
    private Vector3 _smoothVelocity = Vector3.zero;
    

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        _isManipulating = true;
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        if(_isManipulating == false) return;

        var eventPos = eventData.CumulativeDelta;
        // 今回の指から前回の指へのベクトル
        _navigateVelocity = _lastNavigatePos - eventPos;
        _lastNavigatePos = eventPos;
        _smoothVelocity = Vector3.Lerp(_smoothVelocity, _navigateVelocity, 0.5f);
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        _isManipulating = false;
        InputManager.Instance.PopModalInputHandler();
    }

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        _isManipulating = false;
        InputManager.Instance.PopModalInputHandler();
    }

    private void Update()
    {
        if(_isManipulating == false) return;

        transform.position -= _smoothVelocity * 10f;
    }
}

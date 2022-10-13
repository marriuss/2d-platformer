using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationsSetter : MonoBehaviour
{
    private Animator _animator;

    public void SetBoolParameterTrue(string parameterName)
    {
        _animator.SetBool(parameterName, true);
    }

    public void SetBoolParameterFalse(string parameterName)
    {
        _animator.SetBool(parameterName, false);
    }

    public void SetTriggerParameter(string parameterName)
    {
        _animator.SetTrigger(parameterName);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}

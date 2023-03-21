using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Haptics : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    Gamepad gamepad;
    private void Start()
    {
        SetGamepad();
    }
    private void SetGamepad()
    {
        try
        {
            gamepad = (Gamepad)playerInput.devices[0];
        }
        catch
        {
            gamepad = null;
        }
    }
    private void OnDisable()
    {
        if (gamepad != null)
        {
            gamepad.PauseHaptics();
        }
    }
    public void Rumble(float lowFrequency, float highFrequenzy, float time)
    {
        if (gamepad == null) { return; }
        Reset();
        StartCoroutine(Rumble_Internal_Const(lowFrequency, highFrequenzy, time));
    }
    public void Rumble(float lowFrequency, float highFrequenzy, float time, AnimationCurve curve)
    {
        if (gamepad == null) { return; }
        Reset();
        StartCoroutine(Rumble_Internal_Curve(lowFrequency, highFrequenzy, time, curve));
    }
    IEnumerator Rumble_Internal_Const(float lowFrequency, float highFrequenzy, float time)
    {
        gamepad.SetMotorSpeeds(lowFrequency, highFrequenzy);
        yield return new WaitForSeconds(time);
        gamepad.PauseHaptics();
    }

    IEnumerator Rumble_Internal_Curve(float lowFrequency, float highFrequenzy, float time, AnimationCurve curve)
    {
        float timer = 0;
        float left, right;
        while (timer < time)
        {
            left = lowFrequency * curve.Evaluate(timer / time);
            right = highFrequenzy * curve.Evaluate(timer / time);
            gamepad.SetMotorSpeeds(left, right);
            timer += Time.deltaTime;
        }
        yield return new WaitForEndOfFrame();
        gamepad.PauseHaptics();
    }
    public void Reset()
    {
        gamepad.PauseHaptics();
        StopAllCoroutines();
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// from:ChatGPT
/// </summary>
public static class WaitUtility
{
    public static void WaitAndDo(float seconds, Action action)
    {
        CoroutineRunner.Instance.StartCoroutine(WaitCoroutine(seconds, action));
    }

    private static IEnumerator WaitCoroutine(float seconds, Action action) {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();//actionがnullでなければ実行
    }
}

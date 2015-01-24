using UnityEngine;
using System.Collections;

/// <summary>
/// Класс выполняет методы в UI потоке.
/// </summary>
using System;
using System.Collections.Generic;

public class CoroutineExecuter : MonoBehaviour
{
    private static CoroutineExecuter _instance;

    private static CoroutineExecuter Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("Executer");
                _instance = go.AddComponent<CoroutineExecuter>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private readonly List<Action> _tasks = new List<Action>();

    void Update()
    {
        if (_tasks.Count > 0)
        {
            foreach (var task in _tasks)
            {
                task();
            }

            _tasks.Clear();
        }
    }

    /// <summary>
    /// Выполнить метод
    /// </summary>
    /// <param name="action">Action.</param>
    public static void Execute(Action action)
    {
        Instance.ExecuteInUI(action, 0f);
    }

    /// <summary>
    /// Выполнить метод с задержкой
    /// </summary>
    /// <param name="action">Action.</param>
    /// <param name="delay">Delay.</param>
    public static void Execute(Action action, float delay)
    {
        Instance.ExecuteInUI(action, delay);
    }

    /// <summary>
    /// Выполнить корутину
    /// </summary>
    /// <param name="coroutine">Coroutine.</param>
    public static void Execute(IEnumerator coroutine)
    {
        Instance._tasks.Add(() => Instance.StartCoroutine(coroutine));
    }

    /// <summary>
    /// Выполнить метод в UI потоке
    /// </summary>
    /// <param name="action">Метод</param>
    /// <param name="delay">Задержка в сек</param>
    private void ExecuteInUI(Action action, float delay)
    {
        Instance._tasks.Add(() => Instance.StartCoroutine(ExecuteAsCoroutine(action, delay)));
    }

    /// <summary>
    /// Выполнить метод в корутине
    /// </summary>
    /// <returns>The as coroutine.</returns>
    /// <param name="action">Action.</param>
    /// <param name="delay">Delay.</param>
    private static IEnumerator ExecuteAsCoroutine(Action action, float delay)
    {
        if (delay > 0.001f)
            yield return new WaitForSeconds(delay);

        if (action != null)
            action();
    }
}

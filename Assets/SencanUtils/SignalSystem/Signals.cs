using System;
using System.Collections.Generic;

namespace SencanUtils.SignalSystem
{
    /// <summary>
    /// Signals main facade class for global, game-wide signals
    /// </summary>
    public static class Signals
    {
        private static readonly SignalHub Hub = new SignalHub();

        public static T Get<T>() where T : ISignal, new()
        {
            return Hub.Get<T>();
        }

        public static void AddListenerToHash(string signalHash, Action handler)
        {
            Hub.AddListenerToHash(signalHash, handler);
        }

        public static void RemoveListenerFromHash(string signalHash, Action handler)
        {
            Hub.RemoveListenerFromHash(signalHash, handler);
        }

    }

    /// <summary>
    /// Abstract class for Signals, provides hash by type functionality
    /// </summary>
    public abstract class ABaseSignal : ISignal
    {
        protected string _hash;

        /// <summary>
        /// Unique id for this signal
        /// </summary>
        public string Hash
        {
            get
            {
                if (string.IsNullOrEmpty(_hash))
                {
                    _hash = GetType().ToString();
                }
                return _hash;
            }
        }
    }

    /// <summary>
    /// Strongly typed messages with no parameters
    /// </summary>
    public abstract class ASignal : ABaseSignal
    {
        private Action callback;

        /// <summary>
        /// Adds a listener to this Signal
        /// </summary>
        /// <param name="handler">Method to be called when signal is fired</param>
        public void AddListener(Action handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        /// <summary>
        /// Removes a listener from this Signal
        /// </summary>
        /// <param name="handler">Method to be unregistered from signal</param>
        public void RemoveListener(Action handler)
        {
            callback -= handler;
        }

        /// <summary>
        /// Dispatch this signal
        /// </summary>
        public void Dispatch()
        {
            callback?.Invoke();
        }
    }

    /// <summary>
    /// Strongly typed messages with 1 parameter
    /// </summary>
    /// <typeparam name="T">Parameter type</typeparam>
    public abstract class ASignal<T>: ABaseSignal
    {
        private Action<T> callback;

        /// <summary>
        /// Adds a listener to this Signal
        /// </summary>
        /// <param name="handler">Method to be called when signal is fired</param>
        public void AddListener(Action<T> handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        /// <summary>
        /// Removes a listener from this Signal
        /// </summary>
        /// <param name="handler">Method to be unregistered from signal</param>
        public void RemoveListener(Action<T> handler)
        {
            callback -= handler;
        }

        /// <summary>
        /// Dispatch this signal with 1 parameter
        /// </summary>
        public void Dispatch(T arg1)
        {
            callback?.Invoke(arg1);
        }
    }

    /// <summary>
    /// Strongly typed messages with 2 parameters
    /// </summary>
    /// <typeparam name="T">First parameter type</typeparam>
    /// <typeparam name="U">Second parameter type</typeparam>
    public abstract class ASignal<T, U>: ABaseSignal
    {
        private Action<T, U> callback;

        /// <summary>
        /// Adds a listener to this Signal
        /// </summary>
        /// <param name="handler">Method to be called when signal is fired</param>
        public void AddListener(Action<T, U> handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        /// <summary>
        /// Removes a listener from this Signal
        /// </summary>
        /// <param name="handler">Method to be unregistered from signal</param>
        public void RemoveListener(Action<T, U> handler)
        {
            callback -= handler;
        }

        /// <summary>
        /// Dispatch this signal
        /// </summary>
        public void Dispatch(T arg1, U arg2)
        {
            callback?.Invoke(arg1, arg2);
        }
    }

    /// <summary>
    /// Strongly typed messages with 3 parameter
    /// </summary>
    /// <typeparam name="T">First parameter type</typeparam>
    /// <typeparam name="U">Second parameter type</typeparam>
    /// <typeparam name="V">Third parameter type</typeparam>
    public abstract class ASignal<T, U, V> : ABaseSignal
    {
        private Action<T, U, V> callback;

        /// <summary>
        /// Adds a listener to this Signal
        /// </summary>
        /// <param name="handler">Method to be called when signal is fired</param>
        public void AddListener(Action<T, U, V> handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        /// <summary>
        /// Removes a listener from this Signal
        /// </summary>
        /// <param name="handler">Method to be unregistered from signal</param>
        public void RemoveListener(Action<T, U, V> handler)
        {
            callback -= handler;
        }

        /// <summary>
        /// Dispatch this signal
        /// </summary>
        public void Dispatch(T arg1, U arg2, V arg3)
        {
            callback?.Invoke(arg1, arg2, arg3);
        }
    }
}
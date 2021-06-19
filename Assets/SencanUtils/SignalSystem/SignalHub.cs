using System;
using System.Collections.Generic;

namespace SencanUtils.SignalSystem
{
    /// <summary>
    /// A hub for Signals you can implement in your classes
    /// </summary>
    public class SignalHub
    {
        private Dictionary<Type, ISignal> signals = new Dictionary<Type, ISignal>();

        /// <summary>
        /// Getter for a signal of a given type
        /// </summary>
        /// <typeparam name="T">Type of signal</typeparam>
        /// <returns>The proper signal binding</returns>
        public T Get<T>() where T : ISignal, new()
        {
            Type signalType = typeof(T);
            
            if (signals.TryGetValue(signalType, out ISignal signal)) 
            {
                return (T)signal;
            }

            return (T)Bind(signalType);
        }

        /// <summary>
        /// Manually provide a SignalHash and bind it to a given listener
        /// (you most likely want to use an AddListener, unless you know exactly
        /// what you are doing)
        /// </summary>
        /// <param name="signalHash">Unique hash for signal</param>
        /// <param name="handler">Callback for signal listener</param>
        public void AddListenerToHash(string signalHash, Action handler)
        {
            ISignal signal = GetSignalByHash(signalHash);
            if(signal is ASignal aSignal)
            {
                aSignal.AddListener(handler);
            }
        }

        /// <summary>
        /// Manually provide a SignalHash and unbind it from a given listener
        /// (you most likely want to use a RemoveListener, unless you know exactly
        /// what you are doing)
        /// </summary>
        /// <param name="signalHash">Unique hash for signal</param>
        /// <param name="handler">Callback for signal listener</param>
        public void RemoveListenerFromHash(string signalHash, Action handler)
        {
            ISignal signal = GetSignalByHash(signalHash);
            if (signal is ASignal aSignal)
            {
                aSignal.RemoveListener(handler);
            }
        }

        private ISignal Bind(Type signalType)
        {
            if(signals.TryGetValue(signalType, out ISignal signal))
            {
                UnityEngine.Debug.LogError($"Signal already registered for type {signalType}");
                return signal;
            }

            signal = (ISignal)Activator.CreateInstance(signalType);
            signals.Add(signalType, signal);
            return signal;
        }

        private ISignal Bind<T>() where T : ISignal, new()
        {
            return Bind(typeof(T));
        }

        private ISignal GetSignalByHash(string signalHash)
        {
            foreach (ISignal signal in signals.Values)
            {
                if (signal.Hash == signalHash)
                {
                    return signal;
                }
            }

            return null;
        }
    }
}

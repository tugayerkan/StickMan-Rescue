using System.Collections.Generic;

namespace SencanUtils.SUSDKTools
{
    public class SdkParam
    {
        private Dictionary<string, object> _parameters;

        public SdkParam()
        {
            _parameters = new Dictionary<string, object>();
        }

        public void AddParameter(string key, object value)
        {
            _parameters.Add(key, value);
        }

        public void RemoveParameter(string key)
        {
            _parameters.Remove(key);
        }

        public void Clear()
        {
            _parameters.Clear();
        }

        public Dictionary<string, object> GetParameters()
        {
            return _parameters;
        }
    }
}

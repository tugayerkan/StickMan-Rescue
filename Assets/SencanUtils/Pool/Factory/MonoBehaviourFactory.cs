using UnityEngine;

namespace SencanUtils.Pool.Factory
{
	public class MonoBehaviourFactory<T> : IFactory<T> where T : MonoBehaviour 
	{
		private string name;
		private int index = 0;
		
		public MonoBehaviourFactory(string name = "GameObject") 
		{
			this.name = name;
		}

		public T Create()
		{
			GameObject tempGameObject = new GameObject();
			tempGameObject.name = name + index;
			T objectOfType = tempGameObject.AddComponent<T>();
			index++;
			return objectOfType;
		}
	}
}
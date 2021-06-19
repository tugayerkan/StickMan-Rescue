using UnityEngine;

namespace SencanUtils.Pool.Factory
{
	public class PrefabFactory<T> : IFactory<T> where T : MonoBehaviour 
	{
		private GameObject prefab;
		private string name;
		private int index;

		public PrefabFactory(GameObject prefab) : this(prefab, prefab.name) { }

		public PrefabFactory(GameObject prefab, string name) {
			this.prefab = prefab;
			this.name = name;
		}

		public T Create()
		{
			GameObject tempGameObject = Object.Instantiate(prefab);
			tempGameObject.name = name + index;
			T objectOfType = tempGameObject.GetComponent<T>();
			index++;
			tempGameObject.SetActive(false);
			return objectOfType;
		}
	}
}
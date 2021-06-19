namespace SencanUtils.Pool.Factory
{
	public interface IFactory<T>
	{
		T Create();
	}
}
namespace BaseArchitecture.Core
{
    public static class ParamsExtensions
    {
        public static bool TryGetParam<T>(this object[] paramsList, out T value, int index = 0)
        {
            if (paramsList != null && index < paramsList.Length && paramsList[index] is T param)
            {
                value = param;
                return true;
            }
            value = default;
            return false;
        }
    }
}

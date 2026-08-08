namespace BaseArchitecture.Core
{
    /// <summary>
    /// Typed access to the untyped parameter arrays passed between states and scenes.
    /// </summary>
    public static class ParamsExtensions
    {
        /// <summary>Returns the parameter at the index if it matches the requested type, otherwise
        /// outputs the default value and returns false.</summary>
        public static bool TryGetParam<T>(this object[] paramsList, out T value, T defaultValue = default, int index = 0)
        {
            if (paramsList != null && index < paramsList.Length && paramsList[index] is T param)
            {
                value = param;
                return true;
            }
            value = defaultValue;
            return false;
        }
    }
}

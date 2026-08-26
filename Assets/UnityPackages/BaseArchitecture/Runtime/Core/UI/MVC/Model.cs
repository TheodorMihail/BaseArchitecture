namespace BaseArchitecture.Core
{
    /// <summary>
    /// Model in the MVC pattern: the data layer.
    /// </summary>
    public interface IModel
    {
    }

    /// <summary>
    /// Model that takes typed parameters on initialization, passed when available.
    /// </summary>
    public interface IModelWithParams<TParam> : IModel
    {
        void InitializeWithParameters(TParam parameters);
    }

    /// <summary>
    /// Base class for Models. A Model references no View and nothing Unity-specific, which is what
    /// keeps it testable.
    /// </summary>
    public abstract class Model : IModel
    {
    }
}

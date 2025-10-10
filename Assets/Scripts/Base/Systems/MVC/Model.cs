namespace Base.Systems
{
    /// <summary>
    /// Interface for Model components in the MVC pattern.
    /// Models represent the data and business logic layer.
    /// </summary>
    public interface IModel
    {
    }

    /// <summary>
    /// Models implementing this interface can receive typed parameters during initialization.
    /// The framework automatically passes parameters when available.
    /// </summary>
    public interface IModelWithParams<TParam> : IModel
        where TParam : IScreenParam
    {
        void InitializeWithParameters(TParam parameters);
    }

    /// <summary>
    /// Base class for Models that hold state and business logic.
    /// Models should not reference Views or Unity-specific components.
    /// Keep Models framework-agnostic for better testability.
    /// </summary>
    public abstract class Model : IModel
    {
    }
}

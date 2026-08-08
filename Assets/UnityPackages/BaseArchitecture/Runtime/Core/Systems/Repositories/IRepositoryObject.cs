namespace BaseArchitecture.Core
{
    /// <summary>Object that can be stored in a repository. ObjectID is its lookup key.</summary>
    public interface IRepositoryObject
    {
        string ObjectID { get; }
    }
}

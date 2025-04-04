using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    #region Private Fields

    private static readonly Dictionary<Type, object> services = new();
    private static readonly Dictionary<Type, List<object>> collections = new();

    #endregion

    #region Register Methods

    /// <summary>
    /// Registers a service of type T.
    /// </summary>
    /// <param name="service">The service instance to be registered.</param>
    public static void Register<T>(T service) where T : class
    {
        var type = typeof(T);
        if (!services.ContainsKey(type))
        {
            services[type] = service;
        }
    }

    /// <summary>
    /// Registers an instance of type T to a collection of services.
    /// </summary>
    /// <param name="instance">The instance to be added to the collection.</param>
    public static void RegisterToCollection<T>(T instance) where T : class
    {
        var type = typeof(T);
        if (!collections.ContainsKey(type))
        {
            collections[type] = new List<object>();
        }

        collections[type].Add(instance);
    }

    #endregion

    #region Get Methods

    /// <summary>
    /// Retrieves all instances of type T from the service collections.
    /// </summary>
    /// <returns>A list of all instances of type T.</returns>
    public static List<T> GetAll<T>() where T : class
    {
        var type = typeof(T);
        if (collections.TryGetValue(type, out var list))
        {
            return list.ConvertAll(item => item as T);
        }

        return new List<T>(); // Return an empty list if no collection is found
    }

    /// <summary>
    /// Retrieves a single instance of type T from the services.
    /// </summary>
    /// <returns>The registered service of type T.</returns>
    /// <exception cref="Exception">Thrown if the service is not registered.</exception>
    public static T Get<T>() where T : class
    {
        var type = typeof(T);
        if (services.TryGetValue(type, out var service))
        {
            return service as T;
        }

        throw new Exception($"Service of type {type} not registered.");
    }

    #endregion

    #region Clear Methods

    /// <summary>
    /// Clears all registered services and collections.
    /// </summary>
    public static void Clear()
    {
        services.Clear();
        collections.Clear();
    }

    #endregion
}

﻿namespace Uncontainer
{
    /// <summary>
    /// Represents a type that is statically resolved at compile time.
    /// </summary>
    public interface IStaticImplementation : IImplementation
    {
        /// <summary>
        /// Gets the value indicating the type that will be instantiated by this implementation.
        /// </summary>
        /// <value>The target type.</value>
        System.Type TargetType { get; }
    }
}
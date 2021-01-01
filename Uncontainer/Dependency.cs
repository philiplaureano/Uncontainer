using System;

namespace Uncontainer
{
    /// <summary>
    /// Represents a service dependency.
    /// </summary>
    public class Dependency : IDependency
    {
        /// <summary>
        /// Initializes a new instance of the Dependency class.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="serviceName">The service name.</param>
        public Dependency(Type serviceType, string serviceName = null)
        {
            ServiceName = serviceName;
            ServiceType = serviceType;
        }

        /// <summary>
        /// Gets the value indicating the name of the service itself.
        /// </summary>
        /// <value>The service name.</value>
        public string ServiceName { get; }

        /// <summary>
        /// Gets a value indicating the service type.
        /// </summary>
        /// <value>The service type.</value>
        public Type ServiceType { get; }

        /// <summary>
        /// Computes the hash code using the <see cref="ServiceName"/> and <see cref="ServiceType"/>.
        /// </summary>
        /// <returns>The hash code value.</returns>
        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(ServiceName))
                return ServiceType.GetHashCode();

            return ServiceType.GetHashCode() ^ ServiceName.GetHashCode();
        }

        /// <summary>
        /// Determines whether or not the current object is equal to the <paramref name="obj">other object.</paramref>
        /// </summary>
        /// <param name="obj">The object that will be compared with the current object.</param>
        /// <returns><c>true</c> if the objects are equal; otherwise, it will return <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Dependency dependency))
                return false;

            if (string.IsNullOrEmpty(ServiceName))
                return ServiceType == dependency.ServiceType;

            return ServiceType == dependency.ServiceType && ServiceName == dependency.ServiceName;
        }

        /// <summary>
        /// Displays the dependency as a string.
        /// </summary>
        /// <returns>A string that displays the contents of the current dependency.</returns>
        public override string ToString()
        {
            var serviceName = string.IsNullOrEmpty(ServiceName) ? "{NoName}" : ServiceName;
            return $"Service Name: {serviceName}, ServiceType: {ServiceType.Name}";
        }
    }
}
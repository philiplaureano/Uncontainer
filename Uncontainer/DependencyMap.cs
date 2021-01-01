using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Optional.Collections;
using Optional.Unsafe;

namespace Uncontainer
{
    public class DependencyMap : IDependencyMap
    {
        private readonly ConcurrentDictionary<IDependency, ConcurrentBag<IImplementation>> _entries =
            new ConcurrentDictionary<IDependency, ConcurrentBag<IImplementation>>();

        public IEnumerable<IDependency> Dependencies => _entries.Keys;

        public bool Contains(IDependency dependency)
        {
            return _entries.ContainsKey(dependency);
        }

        public IEnumerable<IImplementation> GetImplementations(IDependency targetDependency,
            bool addIncompleteImplementations)
        {
            var matchingImplementations = _entries.GetValueOrNone(targetDependency);
            if (!matchingImplementations.HasValue)
                return Enumerable.Empty<IImplementation>();

            var implementations = matchingImplementations.ValueOrFailure();
            return addIncompleteImplementations ? implementations : 
                implementations.Where(impl => !impl.GetMissingDependencies(this).Any());
        }

        public void AddService(IDependency dependency, IImplementation implementation)
        {
            if(!_entries.ContainsKey(dependency))
                _entries[dependency] = new ConcurrentBag<IImplementation>();

            _entries[dependency].Add(implementation);
        }
    }
}
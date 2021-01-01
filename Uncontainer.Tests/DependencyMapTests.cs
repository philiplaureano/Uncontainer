using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Xunit;

namespace Uncontainer.Tests
{
    public class DependencyMapTests
    {
        [Fact(DisplayName = "The Dependency Map should be able to add a single service")]
        public void ShouldBeAbleToAddService()
        {
            var dependency = new Dependency(typeof(IList<string>));
            var implementation = A.Fake<IImplementation>();

            var map = new DependencyMap();
            map.AddService(dependency, implementation);

            Assert.True(map.Contains(dependency));
            Assert.Contains(dependency, map.Dependencies);
        }

        [Fact(DisplayName = "The Dependency Map should be able to add multiple services")]
        public void ShouldBeAbleToAddMultipleServices()
        {
            var numberOfDependencies = 5;
            var implementations = Enumerable.Range(0, numberOfDependencies)
                .Select(_ => A.Fake<IImplementation>()).ToArray();

            var dependencies = Enumerable.Range(0, numberOfDependencies)
                .Select(_ => A.Fake<IDependency>()).ToArray();

            var map = new DependencyMap();
            for (var i = 0; i < numberOfDependencies; i++)
            {
                var dependency = dependencies[i];
                var implementation = implementations[i];
                map.AddService(dependency, implementation);
            }

            for (var i = 0; i < numberOfDependencies; i++)
            {
                var dependency = dependencies[i];
                Assert.True(map.Contains(dependency));
                Assert.Contains(dependency, map.Dependencies);
            }
        }

        [Fact(DisplayName =
            "The Dependency Map should be able to return all available implementations for a dependency")]
        public void ShouldBeAbleToReturnAllAvailableImplementationsForADependency()
        {
            var numberOfImplementations = 5;
            var implementations = Enumerable.Range(0, numberOfImplementations)
                .Select(_ => A.Fake<IImplementation>()).ToArray();

            var dependency = new Dependency(typeof(IEnumerable<string>));
            var map = new DependencyMap();
            for (var i = 0; i < numberOfImplementations; i++)
            {
                var implementation = implementations[i];
                map.AddService(dependency, implementation);
            }

            Assert.True(map.Contains(dependency));
            Assert.Contains(dependency, map.Dependencies);

            var results = map.GetImplementations(dependency, false).ToArray();
            for (var i = 0; i < numberOfImplementations; i++)
            {
                Assert.Contains(implementations[i], results);
            }
        }

        [Fact(DisplayName =
            "The Dependency Map should be able to return all available implementations for a dependency and exclude the incomplete dependencies")]
        public void ShouldBeAbleToReturnAllAvailableImplementationsForADependencyExceptForTheIncompleteDependencies()
        {
            var numberOfImplementations = 5;
            var numberOfIncompleteImplementations = 3;

            IEnumerable<IImplementation> CreateImplementations(int numberOfImplementationsToCreate, bool isCompleted)
            {
                var incompleteDependencies =
                    isCompleted ? Enumerable.Empty<IDependency>() : new[] {A.Fake<IDependency>()};

                return Enumerable.Range(0, numberOfImplementationsToCreate)
                    .Select(_ =>
                    {
                        var currentImplementation = A.Fake<IImplementation>();
                        A.CallTo(() => currentImplementation.GetMissingDependencies(A<IDependencyMap>.Ignored))
                            .Returns(incompleteDependencies);

                        return currentImplementation;
                    }).ToArray();
            }

            var implementations = new List<IImplementation>();
            implementations.AddRange(CreateImplementations(numberOfImplementations, true));

            var incompleteImplementations = CreateImplementations(numberOfIncompleteImplementations, false)
                .ToArray();
            implementations.AddRange(incompleteImplementations);

            var dependency = new Dependency(typeof(IEnumerable<string>));
            var map = new DependencyMap();
            foreach (var implementation in implementations)
            {
                map.AddService(dependency, implementation);
            }

            Assert.True(map.Contains(dependency));
            Assert.Contains(dependency, map.Dependencies);

            var results = map.GetImplementations(dependency, false).ToArray();
            for (var i = 0; i < numberOfImplementations; i++)
            {
                Assert.Contains(implementations[i], results);
            }

            foreach (var implementation in incompleteImplementations)
            {
                Assert.DoesNotContain(implementation,results);
            }
        }
    }
}
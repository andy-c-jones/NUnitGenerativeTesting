using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace NUnitGenerativeTesting.Tests
{
    [TestFixture]
    public class RandomStringAttributeTests
    {
        [Test]
        public void should_generate_a_string([RandomString] string randomString)
        {
            Assert.That(randomString, Is.Not.Null);
        }

        [Test]
        public void should_generate_data_x_times_where_x_is_the_specified_count([Random(0, 100, 10)] int count)
        {
            var values = new RandomStringAttribute(count: count)
                .GetData(new FakeParameterInfo()).Cast<string>().ToList();
            Assert.That(values.Count, Is.EqualTo(count));
        }

        [Test]
        public void length_should_always_be_less_than_default_of_255([RandomString(count: 100)] string randomString)
        {
            Assert.That(randomString.Length, Is.LessThanOrEqualTo(255));
        }

        private readonly List<string> _alreadySeen = new List<string>();
        [Test]
        public void should_be_random_aka_not_generating_the_same_string_too_often(
            [RandomString(count: 1000)] string randomString)
        {
            _alreadySeen.Add(randomString);
            _alreadySeen.GroupBy(value => value).ToList().ForEach(g =>
                Assert.That(g.Count(), Is.LessThanOrEqualTo(10)));
        }

        [Test]
        public void length_of_strings_should_vary_between_min_and_max(
            [Random(0, 250, 10)] int min,
            [Random(251, 500, 10)] int max)
        {
            var randomStrings = new RandomStringAttribute(min, max)
                .GetData(new FakeParameterInfo());
            var values = randomStrings.Cast<string>().ToList();
            values.ForEach(s =>
            {
                Assert.That(s.Length, Is.GreaterThanOrEqualTo(min));
                Assert.That(s.Length, Is.LessThanOrEqualTo(max));
            });
            
            values.GroupBy(value => value.Length).ToList().ForEach(g =>
                Assert.That(g.Count(), Is.LessThanOrEqualTo(5)));
        }
        
        [Test]
        public void length_of_strings_should_be_varied()
        {
            var randomStrings = new RandomStringAttribute(0, 500, 1000)
                .GetData(new FakeParameterInfo());
            var values = randomStrings.Cast<string>().ToList();
            
            values.GroupBy(value => value.Length).ToList().ForEach(g =>
                Assert.That(g.Count(), Is.LessThanOrEqualTo(10), $"{g.First().Length} found {g.Count()} times."));
        }

        //todo: allowedChars

        //todo: regex
    }

    public class FakeParameterInfo : IParameterInfo
    {
        public T[] GetCustomAttributes<T>(bool inherit) where T : class => throw new NotImplementedException();
        public bool IsDefined<T>(bool inherit) where T : class => throw new NotImplementedException();
        public bool IsOptional { get; }
        public IMethodInfo Method { get; }
        public ParameterInfo ParameterInfo { get; }
        public Type ParameterType { get; }
    }
}
using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnitGenerativeTesting
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class RandomStringAttribute : NUnitAttribute, IParameterDataSource
    {
        private readonly int _min;
        private readonly int _max;
        private readonly int _count;
        private readonly Random _random = new Random();

        public RandomStringAttribute(int min = 0, int max = 255, int count = 5)
        {
            _min = min;
            _max = max;
            _count = count;
        }

        public IEnumerable GetData(IParameterInfo parameter)
        {
            for (var i = 0; i < _count; i++)
            {
                yield return GenerateRandomString();
            }
        }

        private object GenerateRandomString()
        {
            var randomLength = _random.Next(_min, _max);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[randomLength];

            for (var j = 0; j < stringChars.Length; j++)
            {
                stringChars[j] = chars[_random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
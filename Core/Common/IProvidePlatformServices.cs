using System;

using FluentAssertions.Formatting;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Defines the contract the platform-specific assembly must implement to be able to get a chance to initialize itself.
    /// </summary>
    public interface IProvidePlatformServices
    {
        Action<string> Throw { get; }
        IValueFormatter[] Formatters { get; }
        IConfigurationStore ConfigurationStore { get; }
        IReflector Reflector { get; }
    }
}
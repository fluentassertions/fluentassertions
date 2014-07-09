namespace FluentAssertions.Common
{
    /// <summary>
    /// Defines the contract the platform-specific assembly must implement to be able to get a chance to initialize itself.
    /// </summary>
    public interface IPlatformInitializer
    {
        /// <summary>
        /// Should be used to setup the correct <see cref="Services"/>.
        /// </summary>
        void Initialize();
    }
}
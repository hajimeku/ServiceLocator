namespace Packages.ServiceLocator
{
    public enum ServiceManagerLogType
    {
        /// <summary>
        /// A type was registered in the ServiceLocatorManager.
        /// </summary>
        Registered,
        /// <summary>
        /// A type was resolved but not found in the ServiceLocatorManager. 
        /// </summary>
        Missing,
        /// <summary>
        /// Fired before the ServiceLocatorManager is Reset.
        /// </summary>
        /// <remarks>After the reset the default logger will be active again.
        /// The default Logger does not log resets.</remarks>
        Resetting
    }
}
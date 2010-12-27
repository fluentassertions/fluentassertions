namespace FluentAssertions.EventMonitoring
{
    /// <summary>
    ///   Collection of static helper methods hidden away in the query namespace so they
    ///   don't clutter autocompletion for the user.
    /// </summary>
    public static class QueryHelpers
    {
        /// <summary>
        ///   Compares event with a set of parameters.
        /// </summary>
        public static bool ParametersMatch(this RecordedEvent match, params object[] parameters)
        {
            if (match.Parameters.Length != parameters.Length)
            {
                return false;
            }

            for (var index = 0; index < parameters.Length; index++)
            {
                if (!AreMatch(match.Parameters[index], parameters[index]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///   Compares single parameter.
        /// </summary>
        public static bool AreMatch(object parameter, object match)
        {
            return ReferenceEquals(parameter, match) || (parameter.Equals(match));
        }
    }
}
using System;
using Microsoft.Extensions.Hosting;

namespace Zdimk.WebApi.Extensions
{
    public static class Environment
    {
        public static bool IsDevelopment()
        {
            return IsEnvironment(environmentName: Environments.Development);
        }


        public static bool IsStaging()
        {
            return IsEnvironment(environmentName: Environments.Staging);
        }


        public static bool IsProduction()
        {
            return IsEnvironment(environmentName: Environments.Production);
        }


        private static bool IsEnvironment(string environmentName)
        {
            return string.Equals(a: GetEnvironment(), b: environmentName,
                comparisonType: StringComparison.OrdinalIgnoreCase);
        }

        public static string GetEnvironment()
        {
            return System.Environment.GetEnvironmentVariable(variable: "ASPNETCORE_ENVIRONMENT");
        }
    }
}
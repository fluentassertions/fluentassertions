using System;
using System.Reflection;

namespace FluentAssertions.Specs;

public static class FindAssembly
{
    public static Assembly Containing<T>() => typeof(T).Assembly;

    public static Assembly Stub(string publicKey = "") => new AssemblyStub(publicKey);

    private sealed class AssemblyStub : Assembly
    {
        private readonly AssemblyName assemblyName = new();

        public override string FullName => nameof(AssemblyStub);

        public AssemblyStub(string publicKey)
        {
            assemblyName.SetPublicKey(FromHexString(publicKey));
        }

        public override AssemblyName GetName() => assemblyName;

#if NET6_0_OR_GREATER
        private static byte[] FromHexString(string chars)
            => string.IsNullOrEmpty(chars)
            ? Array.Empty<byte>()
            : Convert.FromHexString(chars);
#else
    private static byte[] FromHexString(string chars)
    {
            if (chars.Length % 2 != 0)
            {
                throw new ArgumentException("Input string length must be even.");
            }

            var bytes = new byte[chars.Length / 2];

            for (var i = 0; i < bytes.Length; i++)
            {
                var bits = chars.Substring(i * 2, 2);
                bytes[i] = Convert.ToByte(bits, 16);
            }

            return bytes;
        }
#endif
    }
}

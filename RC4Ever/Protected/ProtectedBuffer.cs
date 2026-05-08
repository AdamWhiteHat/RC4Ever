using System;
using System.Security.Cryptography;

namespace RC4Ever
{
    using Key.Internal;

    public class ProtectedBuffer : IDisposable
    {
        public bool IsSecretDateSet { get; private set; }
        public bool IsCoPrimeSet { get; private set; }
        public bool IsBeginOffsetSet { get; private set; }
        public bool IsRoundsPerScrambleSet { get; private set; }

        public bool IsDisposed { get; private set; } = true;

        private byte[] _salt = null;
        private byte[] _protectedMemory16 = null;

        public ProtectedBuffer(byte[] password)
        {
            IsDisposed = false;

            _salt = new byte[16];
            _protectedMemory16 = new byte[16];

            try
            {
                using (CryptoRNG rand = new CryptoRNG())
                {
                    rand.NextBytes(_salt);
                }

                using (Rfc2898DeriveBytes derivedBytes = new Rfc2898DeriveBytes(password, _salt, 1000))
                {
                    _protectedMemory16 = derivedBytes.GetBytes(_protectedMemory16.Length);
                }
            }
            finally
            {
                CryptoRNG.ZeroBuffer(password);
                ProtectedMemory.Protect(_protectedMemory16, MemoryProtectionScope.SameLogon);
                ProtectedMemory.Protect(_salt, MemoryProtectionScope.SameLogon);
            }
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                UnprotectMem();
                ProtectedMemory.Unprotect(_salt, MemoryProtectionScope.SameLogon);

                IsDisposed = true;

                CryptoRNG.ZeroBuffer(_protectedMemory16);
                CryptoRNG.ZeroBuffer(_salt);

                _protectedMemory16 = null;
                _salt = null;
            }
        }

        private void ThrowIfDisposed()
        {
            if (IsDisposed) { throw new ObjectDisposedException(nameof(ProtectedBuffer)); }
        }

        private void SetBuffer(byte[] value, int offset)
        {
            ThrowIfDisposed();

            try
            {
                UnprotectMem();

                int index = offset;
                int maxIndex = offset + (value.Length - 1);
                while (index <= maxIndex)
                {
                    _protectedMemory16[index] = (byte)(_protectedMemory16[index] ^ value[(index - offset)]);
                    index++;
                }
                index = 0;
            }
            finally
            {
                offset = 0;
                ProtectMem();
            }
        }

        public void SetSecretDate(Int64 startDate)
        {
            ThrowIfDisposed();

            byte[] temp = null;
            try
            {
                temp = BitConverter.GetBytes(startDate);
                SetBuffer(temp, 1);
                IsSecretDateSet = true;
            }
            finally
            {
                startDate = 0;
                CryptoRNG.ZeroBuffer(temp);
                temp = null;
            }
        }

        public void SetCoPrime(UInt32 coPrime)
        {
            ThrowIfDisposed();

            byte[] temp = null;
            try
            {
                temp = BitConverter.GetBytes(coPrime);
                SetBuffer(temp, 9);
                IsCoPrimeSet = true;
            }
            finally
            {
                coPrime = 0;
                CryptoRNG.ZeroBuffer(temp);
                temp = null;
            }
        }

        public void SetBeginOffset(byte beginOffset)
        {
            ThrowIfDisposed();

            byte[] temp = null;
            try
            {
                temp = new byte[] { beginOffset };
                SetBuffer(temp, 13);
                IsBeginOffsetSet = true;
            }
            finally
            {
                beginOffset = 0;
                CryptoRNG.ZeroBuffer(temp);
                temp = null;
            }
        }

        public void SetRoundsPerScramble(byte roundsPerScramble)
        {
            ThrowIfDisposed();

            byte[] temp = null;
            try
            {
                temp = new byte[] { roundsPerScramble };
                SetBuffer(temp, 14);
                IsRoundsPerScrambleSet = true;
            }
            finally
            {
                roundsPerScramble = 0;
                CryptoRNG.ZeroBuffer(temp);
                temp = null;
            }
        }

        private void ProtectMem()
        {
            ThrowIfDisposed();

            ProtectedMemory.Protect(_protectedMemory16, MemoryProtectionScope.SameLogon);
        }

        private void UnprotectMem()
        {
            ThrowIfDisposed();

            ProtectedMemory.Unprotect(_protectedMemory16, MemoryProtectionScope.SameLogon);
        }
    }
}

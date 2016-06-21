using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace RC4Ever.Key
{
	internal class Key : IDisposable
	{
		// TODO:
		// * Use System.Security.Cryptography.RNGCryptoServiceProvider to create a bytes that will be xor'ed with below variables 
		//   so as not to store the key variables in the clear.
		// * Use System.Security.Cryptography.ProtectedMemory.Protect to lock access to key variables and xor data.

		internal int Coprime;
		internal int RoundsPerScramble;
		internal byte BeginOffsetIndex;
		internal byte[] HashedPasswordPhrase;
		internal DateTime SecretStartDate;
		//
		private bool isDisposed = true;

		public Key(int coprime, byte beginOffsetIndex, string passwordHash, int roundsPerScramble, DateTime secretStartDate)
		{
			isDisposed = false;
			Coprime = coprime;
			SecretStartDate = secretStartDate;
			BeginOffsetIndex = beginOffsetIndex;
			RoundsPerScramble = roundsPerScramble;
			HashedPasswordPhrase = Encoding.ASCII.GetBytes(passwordHash);
		}

		public void Dispose()
		{
			if (!isDisposed)
			{
				isDisposed = true;
				Coprime = int.MaxValue;
				RoundsPerScramble = int.MaxValue;
				BeginOffsetIndex = byte.MaxValue;
				SecretStartDate = DateTime.MaxValue;
				HashedPasswordPhrase = Enumerable.Repeat<byte>(byte.MaxValue, HashedPasswordPhrase.Length + 1).ToArray();
				HashedPasswordPhrase = null;
			}
		}
	}
}

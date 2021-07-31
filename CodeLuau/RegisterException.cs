using System;

namespace CodeLuau
{
	class RegisterException : Exception
	{
		public RegisterError registerError;
		public RegisterException(RegisterError error)
		{
			registerError = error;
		}
	}
}

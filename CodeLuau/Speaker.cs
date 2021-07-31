using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeLuau
{
	/// <summary>
	/// Represents a single speaker
	/// </summary>
	public class Speaker
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int? YearsOfExperience { get; set; }
		public bool HasBlog { get; set; }
		public string BlogURL { get; set; }
		public WebBrowser Browser { get; set; }
		public List<string> Certifications { get; set; }
		public string Employer { get; set; }
		public int RegistrationFee { get; set; }
		public List<Session> Sessions { get; set; }


		/// <summary>
		/// Register a speaker
		/// </summary>
		/// <returns>speakerID</returns>
		
		public RegisterResponse Register(IRepository repository)
		{
            try
            {
				return RegisterSpeaker(repository);
			}
			catch(RegisterException exception)
            {
				return new RegisterResponse(exception.registerError);
            }
		}


		private RegisterResponse RegisterSpeaker(IRepository repository)
        {
			ValidateRegisteration();
			AssignApprovmentToSessions();
			RegistrationFee = RegisterationFeesTable.GetRegisterationFeesByExperience(YearsOfExperience);
			return SaveSpeakerToRepository(repository);
		}

		private void ValidateRegisteration()
        {
			ValidateRegisterationData();
			ValidateSpeaker();
		}

		private void ValidateRegisterationData()
        {
			if (IsNotValidFirstName())
				throw new RegisterException(RegisterError.FirstNameRequired);
			if (IsNotValidLastName())
				throw new RegisterException(RegisterError.LastNameRequired);
			if (IsNotValidEmail())
				throw new RegisterException(RegisterError.EmailRequired);
		}

		private void ValidateSpeaker()
        {
			if (IsNotValidSpeaker())
				throw new RegisterException(RegisterError.SpeakerDoesNotMeetStandards);
			if (HasNotSessions())
				throw new RegisterException(RegisterError.NoSessionsProvided);
			if (HasAllOldSessions())
				throw new RegisterException(RegisterError.NoSessionsApproved);
		}


        private RegisterResponse SaveSpeakerToRepository(IRepository repository)
        {
			try
			{
				return GetRegisterResponseAfterSaving(repository);
			}
			catch (Exception error)
			{
				throw new RegisterException(RegisterError.SaveError);
			}
		}

		private RegisterResponse GetRegisterResponseAfterSaving(IRepository repository)
        {
			int? speakerId = repository.SaveSpeaker(this);
			return new RegisterResponse((int) speakerId);
		}

		private bool IsNotValidEmail()
		{
			return string.IsNullOrWhiteSpace(Email);
		}

		private bool IsNotValidFirstName()
		{
			return string.IsNullOrWhiteSpace(FirstName);
		}

		private bool IsNotValidLastName()
		{
			return string.IsNullOrWhiteSpace(LastName);
		}

		private bool IsNotValidSpeaker()
		{
			bool isValidSpeaker = ValidateSpeakerBySkills();
			return isValidSpeaker? !isValidSpeaker : ExceptionalSpeaker();
		}

		private bool ValidateSpeakerBySkills()
        {
			if (YearsOfExperience > 10)		return true;
			if (HasBlog)					return true;
			if (Certifications.Count() > 3) return true;
			var preferredEmployer = new List<string>() { "Pluralsight", "Microsoft", "Google" };
			if (preferredEmployer.Contains(Employer))	return true;
			return false;
		}

		private bool ExceptionalSpeaker()
        {
			if (IsNotValidEmailDomain()) return true;
			if (Browser.IsNotValidBrowser()) return true;
			return false;
		}

		private bool IsNotValidEmailDomain()
        {
			var domains = new List<string>() { "aol.com", "prodigy.com", "compuserve.com" };
			string emailDomain = Email.Split('@').Last();
			return domains.Contains(emailDomain);
		}


		private bool HasNotSessions()
		{
			return Sessions.Any() == false;
		}


		private void AssignApprovmentToSessions()
        {
			foreach (var session in Sessions)
			{
				session.Approved = session.IsNewSession();
			}
		}

		private bool HasAllOldSessions()
        {
			foreach (var session in Sessions)
            {
                if (session.IsNewSession())
                {
					return false;
				}
            }
            return true;
		}
	}
}
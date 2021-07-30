using System.Collections.Generic;

namespace CodeLuau
{
	/// <summary>
	/// Represents a single conference session
	/// </summary>
	public class Session
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public bool Approved { get; set; }

		public Session(string title, string description)
		{
			Title = title;
			Description = description;
		}

		public bool IsOnTechnology(string Technology)
        {
			return Title.Contains(Technology) || Description.Contains(Technology);
        }


		public bool IsNewSession()
        {
			var oldTechnologies = new List<string>() { "Cobol", "Punch Cards", "Commodore", "VBScript" };

			foreach (var technology in oldTechnologies)
			{
				if (IsOnTechnology(technology)) return false;

			}
			return true;
		}
	}
}

using ECommer.DAL.Entities;

namespace ECommer.Models
{
	public class CityViewModel : City
	{
		#region Properties
		public Guid StateId { get; set; }
		#endregion
	}
}

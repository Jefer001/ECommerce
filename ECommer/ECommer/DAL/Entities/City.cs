﻿using System.ComponentModel.DataAnnotations;

namespace ECommer.DAL.Entities
{
    public class City : Entity
    {
		#region Properties
		[Display(Name = "Ciudad")]
        [MaxLength(50)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

		[Display(Name = "Estado")]
		public State State { get; set; }

		[Display(Name = "Usuarios")]
		public ICollection<User> Users { get; set; }
		#endregion
	}
}

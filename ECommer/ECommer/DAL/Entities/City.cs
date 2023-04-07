﻿using System.ComponentModel.DataAnnotations;

namespace ECommer.DAL.Entities
{
    public class City : Entity
    {
        [Display(Name = "Ciudad")]
        [MaxLength(50)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }
        public State State { get; set; }
    }
}

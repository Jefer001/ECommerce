using System.ComponentModel.DataAnnotations;

namespace ECommer.Models
{
	public class LoginViewModel
	{
		#region Properties
		[Display(Name = "Email")]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[EmailAddress(ErrorMessage = "Debes ingresar un correo válido.")]
		public string Username { get; set; }

		[Display(Name = "Contraseña")]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[MinLength(6, ErrorMessage = "El campo {0} debe tener al menos {1} caráteres.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display (Name = "Recordarme en este navegador")]
		public bool RememberMe { get; set; }
		#endregion
	}
}

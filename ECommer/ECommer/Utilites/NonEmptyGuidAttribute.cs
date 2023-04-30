using System.ComponentModel.DataAnnotations;

namespace ECommer.Utilites
{
    public class NonEmptyGuidAttribute : ValidationAttribute
    {
        #region Protecte methods
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult(ErrorMessage);

            if (value is not Guid) return new ValidationResult("El valor proporcionado no es un Guid.");

            Guid guid = (Guid)value;
            Guid zeroGuid = new Guid("00000000-0000-0000-0000-000000000000");

            if (guid.Equals(Guid.Empty) || guid.Equals(zeroGuid)) return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
        #endregion
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tomware.Microsts.Web
{
  public class ApiResourceViewModel
  {
    public int? Id { get; set; }

    public bool Enabled { get; set; }

    [Required]
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public List<string> UserClaims { get; set; } = new List<string>();

    [RequireScopeEntryAttribute(0)]
    public List<string> Scopes { get; set; } = new List<string>();
  }

  public class RequireScopeEntryAttribute : ValidationAttribute
  {
    public int Amount { get; }

    public RequireScopeEntryAttribute(int amount)
    {
      Amount = amount;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var model = (ApiResourceViewModel)validationContext.ObjectInstance;

      if (model.Scopes.Count < this.Amount)
      {
        return new ValidationResult(GetErrorMessage());
      }

      return ValidationResult.Success;
    }

    public string GetErrorMessage()
    {
      if (Amount == 1)
      {
        return $"Requires a scope entry!";
      }

      return $"Requires {Amount} scope entries!";
    }
  }
}

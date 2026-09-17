using System.ComponentModel.DataAnnotations;

namespace CantineApi.CustomAttributes
{
    public class AntiInjectionAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if(value is string s)
            {
                return !s.Contains(";");
            }
            return true;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Boutique
{
    // Déclaration de délégué
    // Associer un nom StringValidator à un prototype spécifique
    public delegate (bool valid, string? message) Validators<T>(T? value);
    public delegate (bool valid, string? message) StringValidator(string? value);
    // <=> Func<string,(bool,string)>
}

namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Exira.CsrfValidation.Owin")>]
[<assembly: AssemblyProductAttribute("Exira.CsrfValidation.Owin")>]
[<assembly: AssemblyDescriptionAttribute("Exira.CsrfValidation.Owin is an OWIN middleware to prevent CSRF attacks by validating HTTP Headers and Cookies.")>]
[<assembly: AssemblyVersionAttribute("0.3.8")>]
[<assembly: AssemblyFileVersionAttribute("0.3.8")>]
[<assembly: AssemblyMetadataAttribute("githash","e873da574d716afd971c96420faf090ecca73e6e")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.3.8"

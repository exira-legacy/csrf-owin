namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Exira.CsrfValidation.Owin")>]
[<assembly: AssemblyProductAttribute("Exira.CsrfValidation.Owin")>]
[<assembly: AssemblyDescriptionAttribute("Exira.CsrfValidation.Owin is an OWIN middleware to prevent CSRF attacks by validating HTTP Headers and Cookies.")>]
[<assembly: AssemblyVersionAttribute("0.2.7")>]
[<assembly: AssemblyFileVersionAttribute("0.2.7")>]
[<assembly: AssemblyMetadataAttribute("githash","2e67b31034377bcbd1bdef56ee593ebd67210feb")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.2.7"

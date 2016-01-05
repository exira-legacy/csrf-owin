namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Exira.CsrfValidation.Owin")>]
[<assembly: AssemblyProductAttribute("Exira.CsrfValidation.Owin")>]
[<assembly: AssemblyDescriptionAttribute("Exira.CsrfValidation.Owin is an OWIN middleware to prevent CSRF attacks by validating HTTP Headers and Cookies.")>]
[<assembly: AssemblyVersionAttribute("0.3.9")>]
[<assembly: AssemblyFileVersionAttribute("0.3.9")>]
[<assembly: AssemblyMetadataAttribute("githash","86297a2bda906b56e3de37252745edb316d400dd")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.3.9"

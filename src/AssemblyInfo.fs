namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Exira.EventStore.Owin")>]
[<assembly: AssemblyProductAttribute("Exira.EventStore.Owin")>]
[<assembly: AssemblyDescriptionAttribute("Exira.EventStore.Owin is an OWIN middleware to keep an EventStore connection shared per application")>]
[<assembly: AssemblyVersionAttribute("0.3.10")>]
[<assembly: AssemblyFileVersionAttribute("0.3.10")>]
[<assembly: AssemblyMetadataAttribute("githash","2584660d218d65d4e4db185cbf040aa427be072b")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.3.10"

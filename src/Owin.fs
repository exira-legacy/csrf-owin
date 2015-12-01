namespace Exira.CsrfValidation.Owin

[<AutoOpen>]
module CsrfValidation =
    open Owin
    open Microsoft.Owin

    open System
    open System.Collections.Generic
    open System.Threading.Tasks
    open System.Runtime.CompilerServices
    open System.Security.Claims
    open System.Net.Http.Headers
    open System.Net.Http
    open System.Linq
    open System.Text.RegularExpressions

    let private excludedPaths = [ "" ]

    let [<Literal>] private CsrfHeader = "X-CSRF"
    let [<Literal>] private CsrfClaimType = "csrf"

    let [<Literal>] private CsrfCookie = "csrf"
    let [<Literal>] private CsrfTokenLength = 40

    let [<Literal>] private Characters = "abcdefghijklmnopqrstuvwxyz0123456789"
    let private charsLen = Characters.Length
    let private rng = Random()

    /// Create a random string consisting of the lower case alphabet and 0-9
    let private random length =
        String [| for _ in 0..length -> Characters.[rng.Next(charsLen)] |]

    type CsrfValidationHelpers() =
        static member BuildCsrfClaim () =
            Claim(CsrfClaimType, random CsrfTokenLength)

        static member WriteCsrfCookie (response: HttpResponseMessage) (csrf: Claim) path (domain: string option) =
            let csrfCookie = CookieHeaderValue(CsrfCookie, csrf.Value)
            csrfCookie.Path <- path

            if domain.IsSome then
                csrfCookie.Domain <- domain.Value

            response.Headers.AddCookies [ csrfCookie ]

    type CsrfValidationOptions() =
        member val ExcludedPaths = excludedPaths with get, set
        member val CsrfHeader = CsrfHeader with get, set

    type CsrfValidationMiddleware(next: Func<IDictionary<string, obj>, Task>, options: CsrfValidationOptions) =
        let awaitTask = Async.AwaitIAsyncResult >> Async.Ignore

        let excludedPathsRegex =
            excludedPaths
            |> List.map (fun excludedPath -> sprintf "^%s$" excludedPath)
            |> List.map (fun excludedPath -> Regex(excludedPath, RegexOptions.Compiled))

        let updateOrAdd key value (dict: dict<'Key, 'T>) =
            lock dict <| fun () -> dict.[key] <- value
            dict

        let findCsrfClaim (principal: ClaimsPrincipal) =
            match principal.FindFirst CsrfClaimType with
            | null -> None
            | claim -> Some claim.Value

        let findCsrfHeader (headers: IDictionary<string, string[]>) =
            let (success, csrfHeader) = headers.TryGetValue options.CsrfHeader
            if success then csrfHeader.First() |> Some
            else None

        let sendUnauthorized environment =
            updateOrAdd "owin.ResponseStatusCode" (401 :> obj) environment |> ignore

        member this.Invoke (environment: IDictionary<string, obj>) =
            let owinContext = OwinContext environment
            let requestHeaders = environment.["owin.RequestHeaders"] :?> IDictionary<string, string[]>

            async {
                if owinContext.Request.Method = "GET" then
                    // No CSRF for GET requests
                    do! next.Invoke environment |> awaitTask

                elif requestHeaders.ContainsKey "Authorization" && not(requestHeaders.ContainsKey "Cookie") then
                    // We are probably using Bearer authentication, resume as usual
                    do! next.Invoke environment |> awaitTask

                elif owinContext.Request.Method = "POST" && owinContext.Request.Path.HasValue && (excludedPathsRegex |> List.exists (fun r -> r.IsMatch(owinContext.Request.Path.ToUriComponent()))) then
                    // We are trying to login, resume as normal
                    do! next.Invoke environment |> awaitTask

                elif not(requestHeaders.ContainsKey options.CsrfHeader) then
                    // No CSRF header, dont even bother...
                    sendUnauthorized environment

                elif owinContext.Authentication.User.Identity.IsAuthenticated then
                    // We got a valid Cookie from .UseCookieAuthentication, and we also have a CSRF header, and we are not trying to login
                    // Need to check if we have a matching claim and header
                    let csrfClaim = findCsrfClaim owinContext.Authentication.User
                    let csrfHeader = findCsrfHeader requestHeaders

                    match csrfClaim, csrfHeader with
                    | Some c, Some h when c = h ->
                        do! next.Invoke environment |> awaitTask
                    | _ -> sendUnauthorized environment

                else
                    sendUnauthorized environment
            } |> Async.StartAsTask :> Task

    [<ExtensionAttribute>]
    type AppBuilderExtensions =
        [<ExtensionAttribute>]
        static member UseCsrfValidation(appBuilder: IAppBuilder, options: CsrfValidationOptions) =
            appBuilder.Use<CsrfValidationMiddleware>(options)

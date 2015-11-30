(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(**
csrf-owin [![NuGet Status](http://img.shields.io/nuget/v/Exira.CsrfValidation.Owin.svg?style=flat)](https://www.nuget.org/packages/Exira.CsrfValidation.Owin/)
======================

<div class="row">
  <div class="span1"></div>
  <div class="span6">
    <div class="well well-small" id="nuget">
      The csrf-owin library can be <a href="https://nuget.org/packages/Exira.CsrfValidation.Owin">installed from NuGet</a>:
      <pre>PM> Install-Package Exira.CsrfValidation.Owin</pre>
    </div>
  </div>
  <div class="span1"></div>
</div>

Exira.CsrfValidation.Owin is an OWIN middleware to prevent CSRF attacks by validating HTTP Headers and Cookies.

## Usage

`Exira.CsrfValidation.Owin` is intended to be used together with `Microsoft.Owin.Security.Cookies`.

You can wire it up in your pipeline as follows:

*)
open Exira.CsrfValidation.Owin

let registerCookieAuthentication (app: IAppBuilder) =
    let cookieOptions =
        CookieAuthenticationOptions(
            AuthenticationMode = AuthenticationMode.Active,
            CookieHttpOnly = true,
            CookieSecure = CookieSecureOption.SameAsRequest,
            SlidingExpiration = true,
            AuthenticationType = "example",
            CookieName = "auth",
            CookiePath = "/")

    if webConfig.Debug.Authentication.UseCookieDomain then
        cookieOptions.CookieDomain <- ".example.org"

    let csrfOptions = CsrfValidationOptions()

    app.UseCookieAuthentication cookieOptions |> ignore
    app.UseCsrfValidation csrfOptions |> ignore
(**

To emit the cookie you can use the following, where `claims` is a `Claim list`:

*)
open Exira.CsrfValidation.Owin

let csrf = CsrfValidationHelpers.BuildCsrfClaim()
let claims = claims @ [csrf]
let response = controller.Request.CreateResponse(HttpStatusCode.OK, "OK")

let path = "/"
let domain =
    if webConfig.Debug.Authentication.UseCookieDomain then Some ".example.org"
    else None

CsrfValidationHelpers.WriteCsrfCookie response csrf path domain
controller.Request.GetOwinContext().Authentication.SignIn(new ClaimsIdentity(claims, "example"))

response
(**

### Cloning

`git clone git@github.com:exira/csrf-owin.git -c core.autocrlf=input`

### Contributing and copyright

The project is hosted on [GitHub][gh] where you can [report issues][issues], fork
the project and submit pull requests. You might also want to read the
[library design notes][readme] to understand how it works.

For more information see the [License file][license] in the GitHub repository.

  [content]: https://github.com/exira/csrf-owin/tree/master/docs/content
  [gh]: https://github.com/exira/csrf-owin
  [issues]: https://github.com/exira/csrf-owin/issues
  [readme]: https://github.com/exira/csrf-owin/blob/master/README.md
  [license]: https://github.com/exira/csrf-owin/blob/master/LICENSE.txt
*)

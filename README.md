# csrf-owin [![NuGet Status](http://img.shields.io/nuget/v/Exira.CsrfValidation.Owin.svg?style=flat)](https://www.nuget.org/packages/Exira.CsrfValidation.Owin/)

## Exira.CsrfValidation.Owin

Exira.CsrfValidation.Owin is an OWIN middleware to prevent CSRF attacks by validating HTTP Headers and Cookies.

### Usage

`Exira.CsrfValidation.Owin` is intended to be used together with `Microsoft.Owin.Security.Cookies`.

You can wire it up in your pipeline as follows:

```fsharp
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

    let csrfOptions =
        CsrfValidationOptions(
            ExcludedPaths =
                [
                    "/users/"
                    "/users/.*/login"
                ])

    app.UseCookieAuthentication cookieOptions |> ignore
    app.UseCsrfValidation csrfOptions |> ignore
```

To emit the cookie you can use the following, where `claims` is a `Claim list`:

``` fsharp
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
```

## Cloning

```git clone git@github.com:exira/csrf-owin.git -c core.autocrlf=input```

## Copyright

Copyright © 2015 Cumps Consulting BVBA / Exira and contributors.

## License

csrf-owin is licensed under [BSD (3-Clause)](http://choosealicense.com/licenses/bsd-3-clause/ "Read more about the BSD (3-Clause) License"). Refer to [LICENSE.txt](https://github.com/exira/csrf-owin/blob/master/LICENSE.txt) for more information.

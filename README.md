# MicroSTS - a token issuer service with IdentityServer and an Angular admin UI

This project aims for a fully working and minimalistic [Security Token Service](https://en.wikipedia.org/wiki/Security_token_service) with local user administration.

It takes advantage of the following existing functionality:
- **[IdentityServer](https://identityserver.io/)**: handles all the [OpenID Connect](https://openid.net/connect/) and OAuth 2.0 protocol stuff
- **[ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio)**: handles the general public available login functionality and manages users, passwords, profile data, roles, claims, tokens, email confirmation, and more
- **[Angular](https://angular.io/)**: provides a minimalistic ui for administrate users, clients and resources.


## Running MicroSTS from within VS Code
As simple as hit `F5`!

Once on the login page login with the username `Admin` and its default password `Pass123$`. After that it is strongly recommended to change the admin's password ;-)!

## Running MicroSTS from docker
```bash
docker run -it --rm -p 5000:5000 --name microsts tomware/microsts:<version>
```

Or prepare volume mapping directories and files and run:

```bash
docker run -it --rm -p 5000:5000 --name microsts tomware/microsts:<version> /
-v C:/work/docker/microsts/data:/app/data /
-v C:/work/docker/microsts/logs:/app/logs /
-v C:/work/docker/microsts/appsettings.json:/app/appsettings.json
```

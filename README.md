# herokufy-dotnet

![Actions Status](https://github.com/jeremymaya/herokufy-dotnet/workflows/build/badge.svg)

Author: Kyungrae Kim

Endpoint: <http://tinyplants.herokuapp.com/api/inventory>

---

## Description

This is a proof of concept that an ASP.NET web application with a relational database can be continuously integrated and deployed to [Heroku](https://www.heroku.com) by combining the power of[Docker](https://www.docker.com) and [GitHub Actions](https://github.com/features/actions).

[Azure](https://azure.microsoft.com/en-us) is often costly despite being one of the popular options for hosting ASP.NET web applications. This project has swapped the following from Azure to minimize the hosting cost while fully showcasing your project:

* Azure App Services => Heroku
* Azure SQL Database => Heroku Postgres
* Azure Blob Storage (images) => Google Photos

---

## Hosting an ASP.NET application on Heroku

While Azure offers many great features and easy hosting options for ASP.NET web applications, it is **EXPENSIVE**. With recent Docker support for ASP.NET applications, it is now possible to host the application on Heroku!

Let's set up a continuous deployment to Heroku from a GitHub repository using GitHub Actions.

### Deploying the web app

1. Add `Docker Support` by following the steps in [Create a Multi-Container App with Docker Compose](https://docs.microsoft.com/en-us/visualstudio/mac/docker-multi-container?view=vsmac-2019)
    * It should be compatible with Linux
2. Modify the generated `Dockerfile` to work with GitHub Actions and Heroku by following the steps in [Deploying to Heroku from GitHub Actions](https://dev.to/heroku/deploying-to-heroku-from-github-actions-29ej).
    * **IMPORTANT**: Modify the `workflow.yml` file further by adding  a `cd` command to change the current directory to where `Dockerfile` is located before running Heroku CLI commands

    For [example](https://github.com/jeremymaya/herokufy-dotnet/blob/main/.github/workflows/workflow.yml):

    ```yaml
    cd ./Herokufy/Herokufy
    ```

    * Do not forget to update the port in `Dockerfile` to `$PORT` by following the steps in [Deploy .Net Core 3.1 Web API to Heroku](https://adevtalks.com/programming/deployment/deploy-net-core-3-1-web-api-to-heroku/).
    * Do not forget to update the `COPY`, `RUN`, `WORKDIR` as shown in [Deploy .Net Core 3.1 Web API to Heroku](https://adevtalks.com/programming/deployment/deploy-net-core-3-1-web-api-to-heroku/).
3. Add the following to GitHub Secrets.

    ```text
    Name: HEROKU_APP_NAME
    Value: Name of your Heroku application

    Name: HEROKU_API_KEY
    Value: Token
    ```

    You can generate the Heroku API key by following the steps in [How should I generate an API key that allows me to use the Heroku Platform API?](https://help.heroku.com/PBGP6IDE/how-should-i-generate-an-api-key-that-allows-me-to-use-the-heroku-platform-api)

    After a push and/or pull to the repo depending on your `workflow.yml`, the website should be now live on Heroku! 🎉

### Deploying the database

You can run PostgreSQL in a development environment using Docker as well. For the instruction on how to run PostgreSQL locally via Docker, please follow the steps in [Using Postgres Instead of SQL Server on Docker from Code-401-Async-Inn-API](https://github.com/jeremymaya/Code-401-Async-Inn-API).

1. Make sure your project is using the following packages from Nuget Package Manager:
    * EntityFrameworkCore
    * EntityFrameworkCore.Design
    * EntityFrameworkCore.PostgreSQL

2. Set up a free-tier PostgreSQL database by following the steps in [How to setup a free PostgreSQL database on Heroku](https://dev.to/prisma/how-to-setup-a-free-postgresql-database-on-heroku-1dc1)
3. Add the database URI to GitHub Secrets.

    ```text
    Name: DATABASE_URL
    Value: Database URI
    ```

4. Update the `Dockerfile` to accept DATABASE_URL as an argument by adding the following:

    ```dockerfile
    ARG DATABASE_URL
    ```

5. Update the `workflow.yml` to pass DATABASE_URL from GitHub Secrets by changing the following:

    ```yaml
    heroku container:push web -a ${{ secrets.HEROKU_APP_NAME }}
    ```

    to

    ```yaml
    heroku container:push web -a ${{ secrets.HEROKU_APP_NAME }} --arg DATABASE_URL=${{ secrets.DATABASE_URL }}
    ```

6. Generate a database connection string that will work with the ASP.NET application with the below function taken from [Deploying a Dockerized ASP.NET Core app using a PostgreSQL DB to Heroku](https://n1ghtmare.github.io/2020-09-28/deploying-a-dockerized-aspnet-core-app-using-a-postgresql-db-to-heroku/) - a HUGE thanks to [n1ghtmare](https://github.com/n1ghtmare) for the post!

    ```c#
    private string GetHerokuConnectionString()
    {
        string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

        var databaseUri = new Uri(connectionUrl);

        string db = databaseUri.LocalPath.TrimStart('/');
        string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

        return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
    }
    ```

7. Either run migration or update command remotely with the Heroku Postgres connection string.

    Your deployed web app should now work with Heroku Postgres! 🎉

---

## Credits

* [Microsoft - Create a Multi-Container App with Docker Compose](https://docs.microsoft.com/en-us/visualstudio/mac/docker-multi-container?view=vsmac-2019)
* [DEV - Deploying to Heroku from GitHub Actions](https://dev.to/heroku/deploying-to-heroku-from-github-actions-29ej)
* [codeburst.io - Deploy a Containerized ASP.NET Core App to Heroku using GitHub Actions](https://codeburst.io/deploy-a-containerized-asp-net-core-app-to-heroku-using-github-actions-9e54c72db943)
* [A Dev Talks - Deploy .Net Core 3.1 Web API to Heroku](https://adevtalks.com/programming/deployment/deploy-net-core-3-1-web-api-to-heroku/)
* [DEV - How to setup a free PostgreSQL database on Heroku](https://dev.to/prisma/how-to-setup-a-free-postgresql-database-on-heroku-1dc1)
* [GitHub - Deploying a Dockerized ASP.NET Core app using a PostgreSQL DB to Heroku](https://n1ghtmare.github.io/2020-09-28/deploying-a-dockerized-aspnet-core-app-using-a-postgresql-db-to-heroku/)

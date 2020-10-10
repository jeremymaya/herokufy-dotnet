# this-is-it

Deployed Endpoint: <http://tinyplants.herokuapp.com>

1. Add Docker Support
    [Microsoft - Tutorial: Create a multi-container app with Docker Compose](https://docs.microsoft.com/en-us/visualstudio/containers/tutorial-multicontainer?view=vs-2019)
2. Enable automatic deployment to Heroku with GitHub Actions
    [Heroku - How should I generate an API key that allows me to use the Heroku Platform API?](https://help.heroku.com/PBGP6IDE/how-should-i-generate-an-api-key-that-allows-me-to-use-the-heroku-platform-api)

    * Go to GitHub Secrets with `Settings` -> `Secrets` -> `New secret` and add the following Secrets
        * Name: HEROKU_APP_NAME
            * Value: Heroku Application Name
        * Name: HEROKU_API_KEY
            * Value: Token

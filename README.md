# First-App
You can open deployed application via link https://list-app-web.azurewebsites.net/
# Docker
To run this app via docker open base directory, and run 
```sh
docker-compose up --build
```
after that navigate to http://localhost:8081/ Make sure that you do not have any applications that are using ports 8081, 5433, 5152 or you would get an error

# Starting Frontend localy
To start frontend first thing you need to go to `.\First-App-Web\`  and then install packages via
```sh
npm install
```
then run the application in development mode
```sh
ng serve
```
# Starting Backend localy

to run backend first thing you need to specify your connection string `DefaultConnection` to the database. You can do it in `First-App-Api\Api\appsettings.Development.json` if you want to run development mode with your own database
Then run 
```sh
dotnet restore
```
to install dependecies and packages.
Then run your application
```sh
#to run in the dev mode
dotnet run --environment Development




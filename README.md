# First-App
You can open deployed application via link (https://list-app-web.azurewebsites.net/)
# Docker
To run this app via docker open base directory, and run 
```sh
docker-compose up build
```
after that navigate to (http://localhost:8081/).Make sure that you do not have any applications that are using ports 8081, 5433, 5152 or you would get an error

# Starting Frontend localy
To start frontend first thing you need to install packages via
```sh
npm install
```
then run the application
```sh
ng serve
```

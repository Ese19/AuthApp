
# Auth App

This ia an authentication application with register, login, logout functionalities with ASP.NET Identity.
There are additional features like email confirmation, reset password and user profile.

Live Link:

## Built With

C#
ASP.NET MVC

## Libraries/Tools Used

EntityFramework
MailKit
AutoMapper
Cloudinary

## How To Run Locally

To run this project locally please follow these steps.

### Prequisites

* .Net 6
* IDE(Code Editor)
    * Visual Studio
    * Visual Studio Code
        * Install the C# extension
* Enable less secure apps to access Gmail with your google account
    * Add your email and the password generated to appsettings.json file
* Cloudinary account
    * Add Cloudinary credentials to the appsettings.development.json file
* PostgreSQL
    * Add yor username and password to the appsettings.development.json file

### Installation

* Clone the repository
* Open the project folder
* In the terminal run the following commands
    * ```sh dotnet tool install --global dotnet-ef ```
    * ```sh dotnet ef database update ```
* Update both appsettings file with your information
* Run the Application
    * For Visual Studio, click the play button
    * For Visual studio code, run the command
      ```sh dotnet run ```
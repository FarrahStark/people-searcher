# people-searcher

## Prerequisites

* Visual Studio 2017 (v15.9.9) with local DB enabled
* .NET core 2.2 SDK
* NodeJS v. 10.14.2

## Running the Project

* Clone this repository
* Open Visual Studio 2017
* Open the .sln file in this repository
* Make sure the PeopleSearch is selected from the run drop down and not IIS Express
* Press the run button or F5
* A console window will open and run the database migrations which will take a minute
* Your browser window should open to https://localhost:5001
* Keep an eye on Visual Studio. The migrations can be a little finicky so after the first
run the app might stop, but you can just press F5 in visual studio again and the app will launch in your browser with fully seeded data.
* Enjoy!

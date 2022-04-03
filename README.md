# multilingual-app-using-google-translate
Implementation of multilingual app using google translate API (Basic version) in Angular & .Net Core API project.

# Structure:
* This project contains API project and Angular project
* demo-project directory contains Angular project
* Bootstrap template is used for demo purpose 

# Description:
* To implement multilingual functionality, set of Keys are used to store all data which can be translated using Google translate API.
* Sample keys are given in WeatherForecast controller.
* API will convert data into any language from English.
* Translated data will be stored in local storage in client-side project, and it will be applied to all components present.
* Current API calls Google translate API parallelly for all data keys to optimize duration of API call.

# Future modifications and improvements:
* Keys can be moved into appsettings
* Dataset can be moved into JSON file or Database as per requirements.
* User can apply versioning to data-set, so that whenever data content changes, front can recall the API and get the latest content.

# To run this demo project:
1. In demo-project directory, install node modules
2. Start local server using ng serve
3. Run API project and set your Google API Client Id at 2 places in Controllers > Weatherforecast controller (Find <<GOOGLE_API_CLIENT_ID>> and replace)
4. Start local server

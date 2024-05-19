# WebAPICba

WebAPICba is an ASP.NET Core web application that interacts with the Central Bank of Armenia's (cba.am) SOAP Web Service to retrieve and store exchange rates. The API allows retrieving exchange rate data in JSON format.

## Features

- Retrieve exchange rates by date via the SOAP Web Service.
- Save exchange rate data to a database.
- API endpoint to get saved exchange rates in JSON format.
- Convenient testing through Swagger UI.

## Installation and Usage

1. Clone the repository.
2. Configure the database connection in `appsettings.json`.
3. Install dependencies and apply database migrations.
4. Run the application and use Swagger UI for testing the API.

## API Endpoints

### Retrieve Exchange Rates

```http
GET /api/exchangerates?DateFrom=YYYY-MM-DD&DateTo=YYYY-MM-DD&ISOCodes=USD,EUR

### Retrieve Exchange Rates by Date Range

Retrieve exchange rates for a specified date range and list of currency ISO codes.

#### HTTP Request

```http
GET /api/exchangerates?DateFrom=YYYY-MM-DD&DateTo=YYYY-MM-DD&ISOCodes=USD,EUR

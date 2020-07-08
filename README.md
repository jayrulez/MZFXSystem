# Overview
**MZFX** is a *.NET Core* console application that performs conversions between currencies.

Conversions can be either direct (where the *source* or the *target* currency is ***CAD***) or indirect (where neither *source* nor *target* currency is ***CAD***).

For direct conversions the exchange rate is taken for a given date directly from https://www.bankofcanada.ca/valet/observations/FX{PAIR}/json where ***{PAIR}*** is the combination of currencies e.g.: ***CADUSD***.

For indirect conversions, the *source currency* is first converted to ***CAD*** then the ***CAD*** value is converted to the *target currency*.

## Supported Currencies
For the purpose of the demo the list of supported currencies is limited to:
- CAD
- USD
- EUR
- JPY
- GBP
- AUD
- CHF
- CNY
- HKD
- MXN
- INR.

This list can be easily expanded as required.

# Requirements

- [x] Convert between foreign currencies and Canadian dollars, and from Canadian dollars to foreign currencies
- [x] Use the daily average exchange rate published by the Bank of Canada for conversion between foreign currencies and Canadian dollars
- [x] Permit the user to specify the foreign currency to convert to/from by currency code (ISO 4217)
- [x] Permit the user to optionally specify the date of the conversion (for converting historical values)
- [x] Display the output value to a precision of 4 decimal places
- [x] Display the conversion rate and date along with the output value
- [x] Convert at least the following foreign currencies: USD, EUR, JPY, GBP, AUD, CHF, CNY, HKD, MXN, INR

# Notes

According to https://www.bankofcanada.ca/rates/exchange/daily-exchange-rates/
The daily average exchange rates are published once each business day by 16:30 ET. Exchange rates are expressed as 1 unit of the foreign currency converted into Canadian dollars.
Therefore, data may not be available for the evaluation date depending on the time the evaluation is done.

I did not do any caching of the exchange rates so the Bank Of Canada API is hit for each conversion call (twice for indirect conversions).

I did not implement any automated testing because of the small scope of the project.

Only the list of currencies from the requirements are supported.

I did not do any service configurations (e.g injecting IOptions<T> to configure services) because of the small scope of the project.

Console UI and service are in a single project since the project doesn't require otherwise (The service could be in a lib so it could be consumed by other projects).

# Assumptions

There is an internet connection available and www.bankofcanada.ca can be reached.

Evaluation machine is using the latest .NET Core 3.1.


# Screenshots

## Main Menu
![alt MainMenu][MainMenu]

## Conversion Menu

### Enter Source Currency
![alt Enter Source Currency][ConversionMenu1]

### Enter Target Currency
![alt Enter Target Currency][ConversionMenu2]

### Enter Amount
![alt Enter Amount][ConversionMenu3]

### Enter Date
![alt Enter Date][ConversionMenu4]

### Conversion Completed
![alt Conversion Completed][ConversionCompleted]

[MainMenu]: ./docs/screenshots/MainMenu.PNG "Main Menu"
[ConversionMenu1]: ./docs/screenshots/ConversionMenu1.PNG "Conversion Menu1"
[ConversionMenu2]: ./docs/screenshots/ConversionMenu2.PNG "Conversion Menu2"
[ConversionMenu3]: ./docs/screenshots/ConversionMenu3.PNG "Conversion Menu3"
[ConversionMenu4]: ./docs/screenshots/ConversionMenu4.PNG "Conversion Menu4"
[ConversionCompleted]: ./docs/screenshots/ConversionCompleted.PNG "Conversion Completed"


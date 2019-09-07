# TaxCalculator

To deploy your database, you can publish from the database project 'TaxCalculatorDb' using TaxCalculatorDb.publish.
It will create a database on your localdb.  The appsettings in the TaxCalculator.Api is already setup to connect to this database.
I've used Dapper to connect and insert into the database.  I initially wanted to allow for updating the different tax types, including the progressive table, in the database, but I've run out of time.

When running, 2 Servers will startup, one for the api and one for the web ui.  
The api uses Swagger and Swashbuckle to document the api.  I then used Nswag to generate the api code.

I didn't do anything to fancy on the front-end, because of the Razor-only requirement.

I have included unit-tests and I've mocked the db context class.

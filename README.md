#### Overview

A collection of projects designed to assist with web-api / RESTful development using minimal Api's, making use of:
- Dapper
- Logging (Serilog, ApplicationInsights, OpenTelemetry)
- Maui
- Minimal API (RESTful, Dependency Injection etc.)
- Roslyn Source Generation
- SignalR
- T-SQL (database design, stored procedures, CTE, JSON, pivoting)
- Unit-tests (X-Unit, Moq, Autofixture)

## Guide - Cognas.ApiTools.SourceGenerators

Contains various incremental generators used to automatically create various classes at compile time.
To use, a reference to this project should be added and updated with the following properties:

- ReferenceOutputAssembly="false"
- OutputItemType="Analyzer"

The generated classes can be viewed after successfuly compilation in:

*(Your Project)* > Dependencies > Analyzers > Cognas.ApiTools.SourceGenerators > *(sub folders...)*

### Attributes used for source generation
- **CommandScaffoldAttribute** - set on a model to generate command business logic and RESTful endpoints
(POST / PUT / DELETE).
- **QueryScaffoldAttribute** - set on a model to generate query business logic and RESTful endpoints
(GET all / GET by id).
- **IdAttribute** - set on a property of a model to flag correspondence with a primary key in SQL.
- **IncludeInModelIdServiceAttribute** - set on a model to include it within the ModelIdService
(used to get / set Id properties of a model to avoid reflection).

## Contact

To get in touch with any suggestions or feedback etc., please freel free to mail at hello@cognas.co.uk
# Message for cloners...

Hi folks.\If you have any feedback,suggestions etc. then please feel free to get in touch - hello@cognas.co.uk.\Thanks!

# Cognas.ApiTools

A collection of projects designed to assist with web-api / RESTful development using minimal Api's.

## Cognas.ApiTools.Data

Contains various classes for data access. Uses Dapper and stored procedures.

## Cognas.ApiTools.Shared

Various extension methods and services used throughout the solution.

## Cognas.ApiTools.SourceGenerators

Incremental Generators used to automatically create various classes at compile time.
To use, a reference to this project should be added and updated with the following properties:

- ReferenceOutputAssembly="false"
- OutputItemType="Analyzer"

The generated classes can be viewed after successfuly compilation in:

*(Your Project)* > Dependencies > Analyzers > Cognas.ApiTools.SourceGenerators > *(sub folders...)*

### Attributes used for source generation *(namespace: Cognas.ApiTools.SourceGenerators.Attributes)*

- **CommandScaffoldAttribute** - set on a model to generate command business logic and RESTful endpoints (POST / PUT / DELETE).
- **QueryScaffoldAttribute** - set on a model to generate query business logic and RESTful endpoints (GET all / GET by id).
- **IdAttribute** - set on a property of a model to flag correspondence with a primary key in SQL.
- **IncludeInModelIdServiceAttribute** - set on a model to include the model within the ModelIdService\(used to get / set Id properties of a model to avoid reflection).
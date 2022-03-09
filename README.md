# AlbumPrinter

Album printer is created in .NET 6 and utilizes some new functionality introduced with this framework. In order to run locally, a SQL Server database needs to be set up, which can be easily deployed through AlbumPrinter.Database.

## Installation

Application can be run through Visual studio ASP.NET Core startup. Only prerequisite is to first Publish database to a (local) DB server. Minor changes to the application can enable In-Memory DB and running without dependencies.
![image](https://user-images.githubusercontent.com/26219443/157491361-c98a0b2a-3972-44cb-aaf5-d3845a37c6df.png)

## Project details
Project follows a pretty basic N-layer architecture, which aims to create separation of concerns between logical layers of the system. It is split into 3:
1. API layer, with controller(s)
2. Business logic layer, which implements this logic trough Services
3. Data access layer, which is implemented using repository pattern. A possible improvement here would be unit of work.

## Calculation
Bin width calculation is driven through database values. This approach is not optimal in terms of speed, as a query is needed in order to retrieve values (this can be cached), but offers full configurability to the system administration team, as new products can be added dynamically, and existing ones can be modified.

### Other Projects
1. **Database** project holds DB model as well as merge seed script(s) for data population on initial creation. ProductTypes are populated by default, while other dynamic data should be populated exclusively by running the system.
2. **Common** class library holds domain agnostic code that can be shared between solutions and holds helper classes
3. **Resources** class library holds domain specific resources and code
4. **Dto** class library separates domain model from the end user, and hides our internal logic
5. **Model** class library contains domain model for the database. In this architecture, these are anemic models
6. **Tests** project holds unit tests for 3 layers

### Quality

Project is fully unit tested, with over 95% code coverage on all relevant areas. These include services, repositories and controllers. Testing framework is MSTest, that is selected purely since it is default, as no preference exists between other alternatives as well.

Additional quality improvements could be enabling docker support, adding static code analysis such as Sonar, which are out of scope.

## Considerations
### Domain driven design
DDD was considered for the project as it is recommended as an industry best practice currently. However, as DDD adds additional complexity to the project, it is mostly recommended for systems that will see great level of business logic - which isn't the case with this project.

### Minimal API's
Minimal API's are an addition in .NET 6 that allows very light projects with basic structure and even less code. This was also a consideration for the project, however, since it doesn't offer good readability and maintainability for the future, rather is reserved for projects that need to stay minimal, it was not implemented. This is due to recommendations given in the project requirements.


## Alternatives
1. In memory database in order to make the project dependency-free. Currently, the project needs an operating SQL Server database to run.
2. SmartEnums (through 3rd party libraries) in order to streamline processing of key-value pairing between products and their prices. It is not extensible enough in order to be future proof, so it was not implemented.
3. XUnit or NUnit for testing
4. Migration scripts instead of DB project could be used, however this approach proves to be more resilient for longer and more requirement intensive projects

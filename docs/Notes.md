# Dev Diary

## 2024-06-22

### Swagger configuration for minimal API's

- logs state 'No action descriptors found. This may indicate an incorrectly configured application or missing application parts'
  (probably some left over Swagger config from old controller version of this solution, d'oh)

### Caching via MemoryCache?

## 2024-06-27

- Unit testing
- look at adding autogeneraton to Pagination, providing additional validation on orderBy to restrict to model property names
- look at nullable ID to avoid CommandMappingServiceBase.RequestToModel ?? 0 on ID
- look at moving GenericServiceRegistration and MultipleServiceRegistration to extensions
- TODO: update Ensure response success for delete when 404 is expected (microservice translates to 500)
- TODO: consistent operation ordering in Swagger
- TODO: https://www.youtube.com/watch?v=CkGFV5bekbY

## 2024-06-29

- Mixable tracks - extend for full enumeration, update response to return which mix with mix / which don't
- Mixable tracks - bad request for passing less than 2 tracks
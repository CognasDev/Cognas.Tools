# Dev Diary

## 2024-06-22

### Swagger configuration for minimal API's

- logs state 'No action descriptors found. This may indicate an incorrectly configured application or missing application parts'

## 2024-06-23

### Versioning

- Allow multiple Command/ Query Scaffold attributes with different versions - Details objects need to take params int[] for api versions,
  then enumerate those in GenerateSource (NB: look at loop flattening to avoid nested loop).

### Caching via MemoryCache?
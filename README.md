## Cleanish architecture

**Tasks:** [https://github.com/hroudaadam/Cleanish-architecture/projects/1](https://github.com/hroudaadam/Cleanish-architecture/projects/1)


**TODOS**
- merge Infrastructure.DataAccess a Infrastructure.Core
- layers
    - Presentation
        - Cleanish.Pres.WebApi
            - Controllers
            - Middlewares
            - Models
            - Exceptions
    - Job
        - Cleanish.Job.Common
        - Cleanish.Job.DailyNotifications
    - Application
        - Cleanish.App.Logic   
            - UseCases
            - Models
            - Validations
            - Exceptions
        - Cleanish.App.Data
            - Cache
            - OAuth,Password,Token providers
            - Localization
            - Repositories
            - Entities
    - Shared
        - Cleanish.Shared
            - Clock
            - Extensions
            - Helpers
            - Structures
            - Exceptions
    - Implementation
        - Cleanish.Impl.App.Data   
        - Cleanish.Impl.Shared
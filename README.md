## Onion architecture

**Tasks:** [https://github.com/hroudaadam/onion-architecture/projects/1](https://github.com/hroudaadam/onion-architecture/projects/1)


**TODOS**
- merge Infrastructure.DataAccess a Infrastructure.Core
- layers
    - Presentation
        - Onion.Pres.WebApi
            - Controllers
            - Middlewares
            - Models
            - Exceptions
    - Job
        - Onion.Job.Common
        - Onion.Job.DailyNotifications
    - Application
        - Onion.App.Logic   
            - UseCases
            - Models
            - Validations
            - Exceptions
        - Onion.App.Data
            - Cache
            - OAuth,Password,Token providers
            - Localization
            - Repositories
            - Entities
    - Shared
        - Onion.Shared
            - Clock
            - Extensions
            - Helpers
            - Structures
            - Exceptions
    - Implementation
        - Onion.Impl.App.Data   
        - Onion.Impl.Shared
# SimpleVtuApp
This repository contains a virtual top up application that can be used to purchase/resale data to customers from different vendors.

## How to run this app
1. Download
   - Clone this repository using `https://github.com/anointedMtc/SimpleVtuApp`
   - Or download the zip file and extract

2. Resolve dependencies
   - You need to turn on your internet
   - Open the solution file in visual studio (preferably) or vsCode
   - If opened with visual studio, it automatically downloads/resolve dependencies for you
   - If opened with vsCode, you need to run the command `dotnet restore`

3. Run docker
   - Run Docker Desktop 
   - From your terminal window, navigate to the webApi host project which contains the *docker-compose.yml* file using `pathToThisProject/VtuHost.WebApi`
   - Run the command `docker-compose up`
   - It would spin up container instances for the following:
     * papercut-smtp: which we would use to receive all the emails sent from the app
     * rabbitmq (MassTransit's version because delayed messaging is configured on it): which would be our message broker
     * redis: for caching
     * redisInsight: a dashboard for viewing and interacting with our cache
     * datalust/seq: to help us read our logs dynamically

4. Dash-boards
   - With the help of Docker Desktop, you can navigate to the dash-boards of Papercut, Rabbitmq, RedisInsight, and Seq 
   - Or you can type in the following urls into your browser
     * For Papercut: `http://localhost:8080/`
     * For RabbitMQ: `http://localhost:15672` and login with userName of `guest` and password of `guest`
     * For RedisInsight: `http://localhost:5540/` then click add database and use `simpleVtuApp-redis` as the host while `6379` is the port
     * For Seq: `http://localhost:8081`

5. Run MSSQL
   - Run Microsoft SQL Management Studio locally on your pc

6. Change the MSSQL server name in the connection strings
   - There are five modules that each have their own connection strings in their own appsettings.json files... and you would need to change the server instance/name to that of **your machine**
   - Change the `CHIKURDEE\\SQLEXPRESS` in `Data Source =CHIKURDEE\\SQLEXPRESS;` to your own for every ***ConnectionStrings*** in the following files:
     * For IdentityModule: `identityModuleSettings.json`
     * For NotificationModule: `notificationModuleSettings.json`
     * For VtuAppModule: `vtuAppModuleSettings.json`
     * For WalletModule: `walletModuleSettings.json`
     * For SagaStateMachinesModule: `sagaStateMachinesModuleSettings.json`

7. Update database
   - In your package manager console window or terminal, use the following commands to update the database (create required tables) usign the _migrations_ already applied for the different modules
     * For IdentityModule: `dotnet ef database update InitialCreate -c ApplicationDbContext -p Identity.Infrastructure -s VtuHost.WebApi`
     * For NotificationModule: `dotnet ef database update InitialCreate -c EmailDbContext -p Notification.Infrastructure -s VtuHost.WebApi`
     * For VtuAppModule: `dotnet ef database update InitialCreate -c VtuDbContext -p VtuApp.Infrastructure -s VtuHost.WebApi`
     * For WalletModule: `dotnet ef database update InitialCreate -c WalletDbContext -p Wallet.Infrastructure -s VtuHost.WebApi`
     * For SagaStateMachinesModule: `dotnet ef database update InitialCreate -c SagaStateMachineDbContext -p SagaOrchestrationStateMachine -s VtuHost.WebApi`

8. Start the app
   - Click the run button
   - Expand the terminal window that pops up and keep it on screen the entire time to watch and monitor the logs. - I have chosen to keep MassTransit minimum log level to `DEBUG` as it gives better insight into the whole background processes... but you can choose to change it to `INFORMATION`... (at the program.cs class and in the appsettings.json file)
   - The only error you should see would be that of `No such host is known` for vtuNation's Api which was configured using Refit and Microsoft's Resilience package. Because of the Resilience package used to register this in the service container, it would retry so many times - which is good-
   - This action would:
     * Seed your database - the identity Module **only** - because we want to use the Id of users generated for us in the database as correlationId for Wallet/Owner/Customer in other modules... it would seed your AspNetUsers table with four users with different important roles
       + `godsEye@gmail.com` - the highest role with all the priviledges possible
       + `adminuser@gmail.com` - this is the next in command, but cannot do everything
       + `staffuser@gmail.com` - an employee who has some basic authorizations to perform specifi tasks
       + `standarduser@gmail.com` - a regular customer/visitor with no special access
     * Start up swagger and open the default page containing all the endpoints
   - Health Checks: You can navigate to the following urls
     * /api/healthcheck
     * /dashboard
     * /health/ready
     * /health/live

9. Test run
   - Login with `godsEye@gmail.com` as it is the only one authorized to _manually_ trigger the sagaStateMachines for newly created users (as last resort - becuase it is automated for newly registered users)
     * GodsEye has two factor Authorisation enabled... so you have to check the **papercut** dashboard for your mail and use the token sent.
     * You can choose to disable the two-fac-auth for `godsEye@gmail.com` if you feel like it slows you down for subsequent re-login since you can easily turn it back on again...
   - Scroll down to the `Admin_Saga_Utility_Controller` and use any of the AspNetUser emails above to trigger the creation of `Wallet/Owner` and `Customer` for that user in WalletModule and VtuAppModule respectively... but this action is handled by MassTransit's State Machines in the SagaStateMachines Module...
     * The action will raise an `ApplicationUserEmailConfirmedEvent` which triggers a `UserCreatedSagaStateMachine` instance to run and process the request.
     * Watch the logs closely using the `terminal window that pops up at start up - remember to enlarge it to view real time analytics` you can choose to use **Seq** dashboard as well... just refresh it to get new data. The logs also write to a file at the root of your directory... you can open the .json file in vsCode, rightClick and choose `format` to _pretty print_ the json for you
     * The userCreatedSagaStateMachine is configured such that after all actions have succeeded, it waits/delays for 30 seconds before sending an email to the user to notify him/her of new wallet creation with bonus balance and directions on what to do.
     * You can trigger this statemachine for all four users above
     * If something goes wrong for any particular user, you can check the current state of the creation process using the AspNetUserId as the correlationId and depending on the stage delete the created Wallet/Owner/Customer **and** the sagaStateMachine instance as well and re-run the request/trigger again

10. Register as a new User
    - Register as a new user and watch as everything is automated for you...

11. Carry out a transaction
    - As directed in the email for new users, you can login and transfer some or all of your bonus to your wallet to start making purchase... make the transfers bit by bit ***just for fun***
    - Watch the logs in the terminal to see all the actions carried out/automated during this process
    - You can login as *admin* or *godsEye* and get the transaction history / wallets / customer for various users. (this endponts can only be accessed by ***adminAndAbove***)

12. Buy airtime / data
    - You ***cannot*** by airtime or data at the moment becuase `api.vtuNation.com` does not exist or isn't up and running... but we can easily subscribe to someother vendors, change the baseaddress, reconfigure the requests/responses and buy/resale
    - To confirm that the setup is working, an *ExternalServiceModule* was created which calls `https://jsonplaceholder.typicode.com` and retrieves data from it.
    - The calls to `typicode` and `vtuNation` are configured with ***delegating handlers*** that supply JWT Bearer - token authentication headers for all outgoing requests...

## Tech Stack and Architecture
   - Full Modular Monolith .NET application with Domain-Driven Design approach.
   - Clean Architecture + CQRS + MediatR
   - Repository + Specification Patterns using MSSQL Database
     * Filtering - Paging - Sorting - Searching functionalities
     * Caching using redis
   - Authentication + Authorization
     * Role Based - Attribute/Claim Based - Resource Based access controls using Asp.Net Identity
   - Logging - Serilog
   - Versioning
   - Cors_Policy
   - RateLimiting/throtteling
   - Health-Checks
   - Global Exception Handling
   - Email - Fluent Email
   - Queues/Messaging/Event Driven Design - MassTransit (statemachines) - RabbitMQ
   - Docker - Redis + RedisInsight - RabbitMQ - PaperCut - Seq
   - Hangfire - Background jobs
   - Refit
   - MediatR pipeline behaviours
     * Validation pipeline behaviour
     * Caching pipeline behaviour
     * Error-Handling pipeline behaviour
     * Logging - pipeline behaviour
     * Performance - piepline behaviour
   - Fluent Validation + Auto Mapper


## TODO
- [ ] OpenTelemetry + Distributed Tracing and more observability packages

 

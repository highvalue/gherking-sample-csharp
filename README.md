# Gherkin Sample in C#
This solution contains a fictional use case consisting of
- Two ASP.NET Core 3.1 micro services
- A testing project using SpecFlow orchestrating both microservices in memory


# Solution Architecture & Principles
An effectively and efficiently testable ASP.NET Core micro service should have certain properties:

- Must support mockable dependency injection
- Should enable the tester to mock as little as possible
- Should enable the tester to separately test aspects and parts of the service
- Should enable the tester to begin writing tests "by contract" parallel to the implementation

The structure of the testing project is explained in it's own readme.

In this example this is acchieved by separating a service in four separate parts.
That can be evolved to a classic layered architecture or a business oriented "circular" DDD, clean or hexagonal architecture.

- **Dependencies point inwards to the Core**
    -  Contracts between the service and external actors exist as separate projects
    -  Gates depend on the Core and Contracts and call the Core to execute Use Cases
    -  The Core exposes Interfaces, depends only on itself, uses top level Provider Interfaces
    -  Providers expose Interfaces and depend on DTOs and object from the Core
- **Use Cases with anemic business may start by skipping unnecessary mapping overhead**
    - For example by sending Contracts from Gate -> Core -> Provider for persistence.
    - Should the need arise mappings and entities can be introduced at will

#### Gate
External actors trigger activities arrive here. This folder can contain Controllers, EventHandlers, CLIs etc.

- Gates ensure schematically correct input data and transform and call the Core functionality.
- Gates don't implement business code.
- Testing a Gates behavior in isolation should not depend on mocked providers, only on mocked core services

#### Core
Contains the pure business logic and rules. Is called by Gates and calls Providers.

- Has no knowledge of persistence or transport technologies.
- Ensures business rules, not technical (i.e. persitence) rules.
- Exposes Interfaces for Gates to use and for mocking in test scenarios
- Should be the dominant force in the system -> Contracts, Gates and Providers adapt to the business needs and use DTOs and objects from the Core
 
#### Provider
Encapsulates knowledge of persistence or transport technologies and communicates with external systems

- Exposes Interfaces expected by the Core, but holds the Interfaces itself in order to reduce confusion.
- Maps to Core DTOs and objects
- Acts as Anti-Corruption layer and spilling zone for nasty technical transformations

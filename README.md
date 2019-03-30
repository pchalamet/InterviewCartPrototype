# cart-prototype
This is an implementation of a cart api done using C#, Orleans, WebApi and a console clients.

# Architecture
The prototype is built around 3 layers:
* a service layer built with Orleans (an agent oriented framework)
* a webapi to handle client requests - it delegates to service layer. - the webapi exposes a swagger contract so it can be easily consumed by clients
* a client using the swagger api (exposed by the webapi) 

````                     
                             +--------+                       +--------+           
+--------+                 +--------+ |                     +--------+ | 
| client | ------------> +-+------+ |-+  -------------->  +-+------+ |-+
+--------+               | WebApi |-+                     | Grains |-+               
                         +--------+                       +--------+      
        
                        Public facing                   Internal services
````

## WebApi interface
This is the public facing endpoint (consumed by the client). The interface helps dealing with addition & removal of items in the cart.
There is are limitations: 
* the cart id *must* be given - which explains why it must be provided in the uri.
* no security

Note this is a prototype, improvements must include:
* identify user
* link cart id to user profile
Again, this is not provided here as it's a prototype.

The webapi has no state - it just delegates to the service layer. This can help scaling the front end. Implementation is fully async to help processing more requests.

The api applies an operation (add or remove) and returns the final state to the client. This has been designed this way to deal with concurrency (see below: service layer section).

Operation | Verb   | Description
----------|--------|-------------
Add       | POST   | Add all items specified in the CartItems. Note max item quantity is `int.MaxValue`.
Remove    | POST   | Remove all items specified in the CartItems. Items with no quantity will be removed from cart.
Clear     | DELETE | Clear cart.


Api http error codes are as follow:

Code | Meaning
-----|--------
200  | Everything is ok.
400  | Something bad happened - check the reason to understand why.
503  | System failure.


For more information and client integration, you can check the Swagger API (see `client/Cart/Cart.nswag.json`).


## Service layer
The service layer is implemented using Orleans to deal with 2 problems:
- state persistence
- concurrency

Orleans does solve the state management problem - allowing to persist and above all route requests to the right cart instance (a grain instance).
Orleans also ensures serialized access to the grain instance - no concurrency - without impacting overall scalability.
Note that data is "persisted" in memory for the moment.

Api returns a tuple (status, value) - because no exception shall be raised. Tuples are used because it's convenient to decorate results such way but it's still better to use union types (but alas, C# does not support this).


## Client
The client has been created using the Swagger service definition. It randomly applies operation on a random cart (so one can run concurrent client to test behaviour). Client use the error info on error and display it as well.

# Build
Some requirements in order to build everything:
* dotnet core 2.2 sdk must be installed
* docker
* powershell core

Main solution file is `cart-prototype.sln` - this can either be built using `dotnet` or `VS Studio`.

Run `build.ps1` to build, test and create docker images.
Run `run.ps1` to run locally an Orleans silo, the WebApi and kick clients.

# Alternative build
Docker images are provided as well, which are used in the GCP build via cloudbuild.yaml.
Note images are not functionnal as some configuration should be done (endpoints are localhost for the moment).
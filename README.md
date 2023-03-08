Some things I wanted to do but could not make it in the time i had: 

1) Add business logic layer - the controller will call the BL layer and not directly like they are doing right now
2) Making the DB user class singleton
3) Adding global error handling
4) In the controller - I have some duplicated code and validations, I wanted to extract it to validation classes + api service that will be responsible for the requests (And not as it is now - from the controller)
5) constants files for strings and things that repeting themself.
6) Add unit test
7) Add error handling (generic gobal one + in the controller methods)
8) The UpdateUserData method is accepting id inside the request body and not doing any thing with it, I wanted to create another model without the id for this method


Here is an image of the requests working with swagger: 

[workingExample.pdf](https://github.com/Tamir198/Abra-dot-net/files/10919956/workingExample.pdf)

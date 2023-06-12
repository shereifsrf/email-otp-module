<!-- how to run -->
## How to run
1. Clone this repository
2. build the project ````dotnet build````
3. run the project ````dotnet run --project Email.Runner/Email.Runner.csproj````

<!-- about project modules -->
## About project modules
This project is divided into 2 modules:
1. Email.Runner
2. EmailOTP

### Email.Runner
This module is the entry point of the project. It is responsible for running the project and calling the EmailOTP module to send the email.
It is also responsible for reading user inputs and validating them.

### EmailOTP
This is the enterprice module that can be used for sending and verifying otps. It is responsible for sending the otp to the user and verifying the otp sent by the user.

#### Services
1. ````ISendService````: This service is responsible for sending the otp to the user.
2. ````IUserService````: This service is responsible for verifying the otp sent by the user, creating user etc

#### Models
1. ````User````: This model is used to store the user information.
2. ````OTP````: This model is used to store the otp information.

#### Enums
1. ````EmailStatus````: This enum is used to store the status of the email sent to the user.
2. ````OTPStatus````: This enum is used to store the status of the otp verification.


<!-- unit test -->
## Unit test
Added a module ````EmailOTP.Tests```` for unit testing the EmailOTP module. This can be used to add unit test cases for the services in the EmailOTP module. I have provided util service test ````Generate.cs````.
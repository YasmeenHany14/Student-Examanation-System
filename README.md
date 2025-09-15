# Student Examination System

A full-stack Student Examination System built with ASP.NET Core (.NET) for the backend and Angular for the frontend. This website enables educational institutions to efficiently manage and monitor student examinations, automate exam generation, and streamline evaluation processes.

## Features

- **Admin Dashboard**: Monitor student exams, manage users, and view analytics.
- **Question Bank Management**: Admins can create, and organize a question bank.
- **Dynamic Exam Generation**: Exams are generated on-the-fly from the question bank, ensuring each student receives a unique set of questions.
- **Student Portal**: Students can request and take exams online.
- **Notifications**: Real-time notifications using SignalR for exam updates and results.
- **Microservices Architecture**: Includes a dedicated evaluation microservice for grading, communicating via RabbitMQ.

## Technologies Used

- **Backend**: ASP.NET Core (.NET 6+), Entity Framework Core, SignalR, RabbitMQ
- **Frontend**: Angular, Tailwind CSS
- **Messaging**: RabbitMQ (for microservice communication)
- **Real-time**: SignalR (for notifications)

## Video Demo
https://github.com/user-attachments/assets/6b69c387-1acc-4ace-bc87-70d343003481


## Project Structure

- `StudentExaminationSystem-API/` - Backend solution (API, Application, Domain, Infrastructure, Shared, EvaluationService)
- `StudentExaminationSystem-Frontend/` - Angular frontend application
- `src/diagrams/` - System diagrams and documentation

## Getting Started

### Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- [Node.js & npm](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
- [RabbitMQ](https://www.rabbitmq.com/download.html)

### Backend Setup
1. Navigate to the API folder:
   ```sh
   cd src/StudentExaminationSystem-API
   ```
2. Restore dependencies and build:
   ```sh
   dotnet restore
   dotnet build
   ```
3. Update `appsettings.json` as needed (connection strings, RabbitMQ, etc).
4. Run the API:
   ```sh
   dotnet run --project WebApi/WebApi.csproj
   ```

### Frontend Setup
1. Navigate to the frontend folder:
   ```sh
   cd src/StudentExaminationSystem-Frontend
   ```
2. Install dependencies:
   ```sh
   npm install
   ```
3. Run the Angular app:
   ```sh
   ng serve
   ```

### Microservices & Messaging
- Ensure RabbitMQ is running locally or update the connection settings in the backend configuration.
- The evaluation microservice will listen for exam submissions and process grading asynchronously.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

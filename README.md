# PitaraBaati - Digital Clock with Timezone Display

A modern web application built with .NET Core backend and React frontend that displays current time in different time zones.

## Technology Stack

### Backend
- **.NET Core 8.0+** - RESTful API
- **Entity Framework Core** - ORM
- **SQL Server** - Database

### Frontend
- **React 18+** - UI Framework
- **TypeScript** - Type-safe JavaScript
- **Vite** - Build tool
- **Tailwind CSS** - Styling

## Project Structure

```
PitaraBaati/
├── backend/          # .NET Core API
├── frontend/         # React Application
└── docs/            # Documentation
```

## Getting Started

### Backend Setup

```bash
cd backend
dotnet restore
dotnet build
dotnet run
```

The API will run on `http://localhost:5000`

### Frontend Setup

```bash
cd frontend
npm install
npm run dev
```

The React app will run on `http://localhost:5173`

## Features

- **Real-time Digital Clock** - Updates every second
- **Multiple Timezones** - View time in different time zones simultaneously
- **24/7 Accessibility** - Works around the clock
- **Responsive Design** - Mobile-friendly interface
- **Clean UI** - Modern and intuitive design

## API Endpoints

- `GET /api/timezone` - Get all supported timezones
- `GET /api/timezone/{timezone}` - Get current time in specific timezone
- `GET /swagger` - API documentation

## Development Commands

### Backend
- `dotnet build` - Build the solution
- `dotnet run` - Run the application
- `dotnet test` - Run tests

### Frontend
- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run lint` - Lint code

## Contributing

Please follow the project guidelines when contributing.

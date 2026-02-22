---

<br/>
<p align="center">
  <a href="https://github.com/donpotts/MudBlazorCrmApp">
    <img src="https://avatars.githubusercontent.com/u/1085058?v=4" alt="Logo" width="80" height="80">
  </a>

  <h1 align="center">MudBlazor CRM</h1>

  <p align="center">
    A full-featured CRM application built with Blazor WASM and .NET 9, featuring a drag-and-drop dashboard, deal pipeline, activity tracking, reporting, and more.
    <br />
    <a href="#-getting-started"><strong>Get Started »</strong></a>
    <br />
    <br />
    <a href="https://github.com/donpotts/MudBlazorCrmApp/issues">Request Feature</a>
  </p>
</p>

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/donpotts/MudBlazorCrmApp/MudBlazorCrmApp.yml?logo=github&style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![License](https://img.shields.io/github/license/donpotts/MudBlazorCrmApp?style=for-the-badge)

---

![MudBlazor CRM](./MudBlazorCrmApp/Assets/MudBlazorCRM.png)

---

## About The Project

This repository is a comprehensive, modern web application built on the latest .NET stack. It serves as a practical example of a Customer Relationship Management (CRM) system using a Blazor WASM client and an ASP.NET Core server.

The goal is to demonstrate best practices and the seamless integration of powerful open-source technologies to build a real-world, data-driven application.

**Key highlights include:**
*   A clean, responsive UI powered by **MudBlazor**.
*   Secure user management with **ASP.NET Core Identity**.
*   An efficient and queryable API using **OData**.
*   A lightweight and cross-platform **SQLite** database.

## Features

### Core CRM

- **Customer Management** - Track customers with addresses, contacts, tags, and full interaction history.
- **Contact Management** - Manage contacts linked to customers with email, phone, and notes.
- **Lead Tracking** - Capture and qualify leads from multiple sources with status progression.
- **Opportunity Management** - Track deals with name, value, probability, source, stage, estimated close date, and linked customer/contact.
- **Sales Records** - Record completed sales tied to customers, products, and services.
- **Support Cases** - Log and track customer support issues.

### Pipeline & Workflow

- **Deal Pipeline Board** - Kanban-style drag-and-drop board for managing opportunities through stages (Prospecting, Qualification, Proposal, Negotiation, Closed Won/Lost). Shows per-stage value totals.
- **Kanban Task Board** - Drag-and-drop task management with customizable columns.
- **Activity Tracking** - Log calls, emails, meetings, notes, and tasks against any entity (customer, contact, lead, or opportunity). Tracks duration, direction, and status.
- **Entity Timeline** - Chronological view of all interactions, sales, and support cases for any customer, contact, lead, or opportunity.

### Dashboard & Analytics

- **Interactive CRM Dashboard** - Drag-and-drop dashboard powered by GridStack.js with persistent layout. Cards include:
  - Sales Over Time (line chart)
  - Lead Sources (doughnut chart)
  - Pipeline Value by Stage (bar chart)
  - Deals by Stage (bar chart)
  - KPI cards: New Leads, Total Revenue, Conversion Rate, Pipeline Value, Deals Closed, Avg Deal Size, Total Customers, Open Support Cases
  - Recent Activity feed
  - Top Opportunities list
  - Quick Notes
- **Reports Page** - Date-filtered reports with Chart.js visualizations for sales trends, pipeline analysis, lead source breakdown, and activity summaries.
- **CSV Export** - Export any entity list to CSV.

### Data & Integration

- **CSV Import** - Bulk import customers, contacts, leads, opportunities, and other entities from CSV files with preview and validation.
- **Global Search** - Search across customers, contacts, leads, and opportunities from the app bar.
- **Tagging System** - Create color-coded tags and apply them to any entity for flexible categorization and filtering.
- **OData API** - Full OData v4 support for server-side filtering, sorting, and pagination on all entity endpoints.
- **Swagger/OpenAPI** - Interactive API documentation at `/swagger`.

### UI & Theming

- **Dark Mode** - Toggle between light and dark themes.
- **Theme Customization** - Change primary color with a theme picker that persists to local storage.
- **Responsive Layout** - Works on desktop and mobile with a collapsible navigation drawer.
- **MudBlazor Components** - Professional Material Design UI throughout.

### Security

- **ASP.NET Core Identity** - User registration, login, password change, and role-based authorization.
- **Role Management** - Administrator role with user management capabilities.
- **JWT Authentication** - Token-based API authentication for the Blazor WASM client.

---

## Tech Stack

*   🖥️ **Modern Frontend**: A rich, single-page application (SPA) experience with **Blazor WASM**.
*   🎨 **Beautiful UI Components**: Leverages the extensive and professional [MudBlazor](https://mudblazor.com/) component library.
*   🔐 **Secure Authentication**: Full user registration and login system via **ASP.NET Core Identity**.
*   🚀 **High-Performance Backend**: Built on the fast and reliable **ASP.NET Core Kestrel** web server.
*   🗃️ **Flexible Data Queries**: **OData** support allows for powerful and efficient API queries directly from the client.
*   📝 **Interactive API Docs**: Includes **Swagger (OpenAPI)** for easy API exploration and testing.
*   📊 **Kanban Task Management**: A sample Kanban board to demonstrate dynamic UI and data interaction.
*   🪶 **Lightweight Database**: Uses **Entity Framework Core** with **SQLite** for simple setup and development.

### Technical Features
- 🖥️ **Modern Frontend**: Single-page application with **Blazor WASM**
- 🎨 **Beautiful UI**: Professional [MudBlazor](https://mudblazor.com/) components
- 🔐 **Secure Authentication**: **ASP.NET Core Identity** with JWT tokens
- 📊 **Interactive Dashboard**: Real-time KPIs and analytics charts
- 🔍 **Advanced Querying**: **OData** for flexible data access
- 📝 **API Documentation**: **Swagger/OpenAPI** integration
- 📋 **Audit Logging**: Track all entity changes with Login/Logout events
- 💬 **Communication Tracking**: Log all customer interactions
- 📤 **Data Export**: CSV export functionality

### Enterprise-Ready
- ✅ Data validation with annotations
- ✅ Proper entity relationships
- ✅ Indexed database queries
- ✅ Rate limiting protection
- ✅ Role-based authorization
- ✅ Automatic timestamp tracking
- ✅ Soft delete support
- ✅ Auditable entities (CreatedBy, ModifiedBy)

---

## Project Structure

This project is built with a curated set of modern technologies:

| Technology                                                                                           | Description                              |
| ---------------------------------------------------------------------------------------------------- | ---------------------------------------- |
| ![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet)                                         | Core application framework               |
| ![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?logo=dotnet)                         | Web framework for building the server    |
| ![Blazor](https://img.shields.io/badge/Blazor-512BD4?logo=blazor)                                     | Frontend C# web framework                |
| ![MudBlazor](https://img.shields.io/badge/MudBlazor-1E88E5?logo=M&logoColor=fff)                      | Material Design component library        |
| ![Entity Framework Core](https://img.shields.io/badge/EF_Core-512BD4?logo=entityframework)           | Object-Relational Mapper (ORM)           |
| ![SQLite](https://img.shields.io/badge/SQLite-003B57?logo=sqlite&logoColor=white)                     | Embedded database engine                 |
| ![OData](https://img.shields.io/badge/OData-F26825?logo=odata&logoColor=white)                        | Standard for building RESTful APIs       |
| ![Swagger](https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=black)                  | API documentation and testing            |

---

## 🏁 Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing.

### Prerequisites

Make sure you have the following tools installed:

*   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (or the latest version)
*   [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) with the **ASP.NET and web development** workload installed.
*   [Git](https://git-scm.com/)

### Quick Start

1.  **Clone the repository**
    ```sh
    git clone https://github.com/donpotts/MudBlazorCrmApp.git
    ```

2.  **Navigate to the project directory**
    ```sh
    cd MudBlazorCrmApp
    ```

3.  **Open the solution in Visual Studio**
    *   Open `MudBlazorCrmApp.sln` with Visual Studio 2022.

4.  **Run the application**
    *   Set `MudBlazorCrmApp` as the startup project.
    *   Press `F5` or the "Run" button to build and start the application.
    *   The application will launch in your default web browser. The database will be created and seeded on the first run.

---

## 🔌 API Endpoints

### Dashboard API
```
GET /api/dashboard/kpis              # Key performance indicators
GET /api/dashboard/sales-trend       # Sales over time
GET /api/dashboard/lead-sources      # Lead source distribution
GET /api/dashboard/pipeline-stages   # Sales pipeline data
GET /api/dashboard/recent-activity   # Activity feed
GET /api/dashboard/top-opportunities # Top deals
```

### Audit & Activity APIs
```
GET    /api/auditlog                 # Entity change history
GET    /api/auditlog/{id}            # Audit log details
POST   /api/authactivity/login       # Log login event
POST   /api/authactivity/logout      # Log logout event
```

### Core CRUD APIs
```
GET/POST       /api/customer
GET/PUT/DELETE /api/customer/{id}

GET/POST       /api/lead
GET/PUT/DELETE /api/lead/{id}

This application uses ASP.NET Core Identity for user authentication. To log in, navigate to the login page and enter your credentials.

Administrator

Username: adminUser@example.com

---

Normal user

Username: normalUser@example.com

Password: testUser123!

---

## Contact

Don Potts - Don.Potts@DonPotts.com

Project Link: [https://github.com/donpotts/MudBlazorCrmApp](https://github.com/donpotts/MudBlazorCrmApp)

---

## 🙏 Acknowledgments

- [MudBlazor](https://mudblazor.com/) - Amazing Blazor component library
- [Chart.js](https://www.chartjs.org/) - Beautiful charts
- [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) - Robust backend framework

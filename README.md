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
  </p>
</p>

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/donpotts/MudBlazorCrmApp/MudBlazorCrmApp.yml?logo=github&style=for-the-badge)

---

![MudBlazor CRM](./MudBlazorCrmApp/Assets/MudBlazorCRM.png)

---

## About The Project

MudBlazor CRM is a comprehensive Customer Relationship Management system built on the .NET 9 stack. It goes beyond simple data entry to provide a real-world CRM experience with visual pipeline management, interaction tracking, live dashboards, and reporting.

The application uses a Blazor WebAssembly frontend with a MudBlazor UI, backed by an ASP.NET Core API server with OData support and a SQLite database.

---

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

| Technology | Description |
|---|---|
| .NET 9 | Core application framework |
| ASP.NET Core | Web framework and API server |
| Blazor WebAssembly | Frontend SPA framework |
| MudBlazor | Material Design component library |
| Entity Framework Core | ORM with code-first migrations |
| SQLite | Embedded database |
| OData | Queryable REST API standard |
| Chart.js | Dashboard and report charts |
| GridStack.js | Drag-and-drop dashboard layout |
| Swagger/OpenAPI | API documentation |

---

## Project Structure

```
MudBlazorCrmApp/
├── MudBlazorCrmApp/                    # ASP.NET Core server
│   ├── Controllers/                    # 23 API controllers
│   │   ├── DashboardController.cs      # Aggregate KPI/chart endpoints
│   │   ├── SearchController.cs         # Global search API
│   │   ├── ImportController.cs         # CSV import API
│   │   ├── ReportController.cs         # Date-filtered report API
│   │   ├── ActivityController.cs       # Activity/interaction CRUD
│   │   ├── TagController.cs            # Tag management
│   │   ├── EntityTagController.cs      # Tag-to-entity associations
│   │   └── ...                         # Entity CRUD controllers
│   ├── Data/
│   │   └── ApplicationDbContext.cs     # EF Core context with all entities
│   └── Program.cs                      # App configuration and OData setup
│
├── MudBlazorCrmApp.Shared/             # Shared models and DTOs
│   └── Models/                         # 24 model classes
│       ├── Customer.cs, Contact.cs, Lead.cs, Opportunity.cs
│       ├── Activity.cs                 # Interaction tracking entity
│       ├── Tag.cs, EntityTag.cs        # Tagging system
│       ├── DashboardDto.cs             # KPI, chart, and timeline DTOs
│       ├── DashboardCard.cs            # Dashboard layout model
│       └── ...                         # All CRM entities
│
├── MudBlazorCrmApp.Shared.Blazor/      # Blazor WASM UI
│   ├── Pages/                          # 68 page components
│   │   ├── CRMDashboard.razor          # Main dashboard with charts
│   │   ├── PipelineBoard.razor         # Deal pipeline Kanban
│   │   ├── KanbanTodoTask.razor        # Task Kanban board
│   │   ├── Reports.razor               # Reporting page
│   │   ├── ImportData.razor            # CSV import page
│   │   └── ...                         # List/Add/Update pages per entity
│   ├── Components/
│   │   ├── Dashboard.razor             # Reusable GridStack dashboard
│   │   ├── EntityTimeline.razor        # Chronological activity timeline
│   │   ├── TagSelector.razor           # Tag management chip selector
│   │   ├── Themes/ThemesMenu.razor     # Theme picker
│   │   └── ConfirmDeleteDialog.razor   # Delete confirmation
│   ├── Layout/
│   │   └── NavMenu.razor               # Navigation with global search
│   ├── Services/
│   │   ├── AppService.cs               # Central HTTP client (1800+ lines)
│   │   └── ThemeService.cs             # Dark mode state management
│   └── wwwroot/
│       ├── js/chart-renderer.js        # Chart.js rendering module
│       ├── js/dashboard-manager.js     # GridStack layout management
│       └── css/app.css                 # Application styles
│
└── MudBlazorCrmApp.Blazor/             # WASM host project
    └── wwwroot/index.html              # Entry point with loading UI
```

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) with the **ASP.NET and web development** workload, or any code editor with .NET support
- [Git](https://git-scm.com/)

### Installation & Running

1. **Clone the repository**
    ```sh
    git clone https://github.com/donpotts/MudBlazorCrmApp.git
    ```

2. **Navigate to the project directory**
    ```sh
    cd MudBlazorCrmApp
    ```

3. **Run the application**
    ```sh
    dotnet run --project MudBlazorCrmApp
    ```
    The application will launch and be available at `https://localhost:5001` (or the URL shown in the console). The SQLite database is created and seeded automatically on first run.

    Alternatively, open `MudBlazorCrmApp.sln` in Visual Studio, set `MudBlazorCrmApp` as the startup project, and press F5.

---

## Authentication

This application uses ASP.NET Core Identity for user authentication.

### Default Accounts

| Role | Username | Password |
|---|---|---|
| Administrator | adminUser@example.com | testUser123! |
| Normal User | normalUser@example.com | testUser123! |

The Administrator account has access to user management features.

---

## API

The server exposes OData-enabled REST endpoints for all entities. Visit `/swagger` when running locally to explore the full API.

### Dashboard Endpoints

| Endpoint | Description |
|---|---|
| `GET /api/dashboard/kpis` | Aggregate KPIs (leads, revenue, conversion, pipeline, deals) |
| `GET /api/dashboard/sales-over-time` | Monthly sales totals |
| `GET /api/dashboard/lead-sources` | Lead count by source |
| `GET /api/dashboard/pipeline-by-stage` | Opportunity value/count by stage |
| `GET /api/dashboard/recent-activity` | Latest logged activities |
| `GET /api/dashboard/top-opportunities` | Highest-value open opportunities |

### Other API Endpoints

| Endpoint | Description |
|---|---|
| `GET /api/search?q={query}` | Global search across entities |
| `POST /api/import/{entityType}` | CSV import for any entity |
| `GET /api/reports/...` | Date-filtered report data |
| `GET /api/timeline/{entityType}/{entityId}` | Entity interaction timeline |
| `GET /odata/{Entity}` | OData CRUD for all entities |

---

## Entities

The CRM manages the following entities:

| Entity | Description |
|---|---|
| Customer | Companies or individuals you do business with |
| Contact | People associated with customers |
| Lead | Potential customers from various sources |
| Opportunity | Deals in your sales pipeline with value and stage tracking |
| Activity | Logged interactions (calls, emails, meetings, notes, tasks) |
| Sale | Completed sales transactions |
| Product | Products available for sale |
| Service | Services offered |
| Product Category | Product categorization |
| Service Category | Service categorization |
| Support Case | Customer support tickets |
| Todo Task | Tasks with Kanban board support |
| Vendor | Supplier/vendor records |
| Address | Addresses linked to customers |
| Reward | Customer reward records |
| Tag | Color-coded labels for categorizing any entity |

---

## Contact

Don Potts - Don.Potts@DonPotts.com

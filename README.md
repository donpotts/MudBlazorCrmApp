---

<br/>
<p align="center">
  <a href="https://github.com/donpotts/MudBlazorCrmApp">
    <img src="https://avatars.githubusercontent.com/u/1085058?v=4" alt="Logo" width="80" height="80">
  </a>

  <h1 align="center">MudBlazor CRM</h1>

  <p align="center">
    A full-featured CRM application built with Blazor WASM and .NET 10, featuring a drag-and-drop dashboard, deal pipeline, activity tracking, email integration, reporting, and more.
    <br />
    <a href="#-getting-started"><strong>Get Started »</strong></a>
    <br />
    <br />
    <a href="https://github.com/donpotts/MudBlazorCrmApp/issues">Request Feature</a>
  </p>
</p>

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/donpotts/MudBlazorCrmApp/MudBlazorCrmApp.yml?logo=github&style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
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
*   Email integration via **Microsoft Graph API**.

## Features

### Core CRM

- **Customer Management** - Track customers with addresses, contacts, tags, attachments, and full interaction history.
- **Contact Management** - Manage contacts with company, title, email, phone, LinkedIn, preferred contact method, status, and notes. Send emails directly from contact records.
- **Lead Tracking** - Capture and qualify leads from multiple sources with status progression, tags, and timeline.
- **Opportunity Management** - Track deals with name, value, probability, source, stage, estimated close date, and linked customer/contact.
- **Sales Records** - Record completed sales tied to customers, products, and services.
- **Support Cases** - Log and track customer support issues with priority, status, and escalation.

### Pipeline & Workflow

- **Deal Pipeline Board** - Kanban-style drag-and-drop board for managing opportunities through stages (Prospecting, Qualification, Proposal, Negotiation, Closed Won/Lost). Shows per-stage value totals.
- **Kanban Task Board** - Drag-and-drop task management with customizable columns.
- **Activity Tracking** - Log calls, emails, meetings, notes, and tasks against any entity (customer, contact, lead, or opportunity). Tracks duration, direction, and status.
- **Entity Timeline** - Chronological view of all interactions, sales, and support cases for any customer, contact, lead, or opportunity.
- **Calendar View** - Month calendar showing activities and tasks with color-coded event chips and day detail view. Filter by activities, tasks, or both.

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

### Email & Communication

- **Email Integration** - Send emails via Microsoft Graph API directly from the CRM. Emails are automatically logged as Activity records for complete interaction history.
- **Email Templates** - 20 pre-built, customizable email templates across 7 categories:
  - **Onboarding** - Welcome New Contact, New Customer Welcome
  - **Sales** - Lead Follow-Up, Opportunity Proposal, Deal Won/Lost Confirmation, Re-engagement, Referral Request
  - **Communication** - Meeting Request, Meeting Follow-Up, Thank You Note
  - **Support** - Case Created, Case Resolved, Case Escalation
  - **Account Management** - Quarterly Business Review, Contract Renewal, Feedback Request
  - **Marketing** - Product/Service Update, Event Invitation
  - **Billing** - Invoice/Payment Reminder
- **Template Placeholders** - Templates support merge fields like `{{ContactName}}`, `{{CompanyName}}`, `{{SenderName}}`, `{{OpportunityName}}`, etc.
- **Communication Tracking** - Log all customer interactions with full history.

### Notifications

- **Bell Icon Notifications** - Real-time notification badge in the app bar with unread count.
- **Notification Panel** - Popover panel showing all notifications with type icons and timestamps.
- **Auto-Generated Alerts** - Automatic notifications for overdue support cases, follow-up reminders, and opportunities closing within 7 days.
- **Mark as Read** - Mark individual or all notifications as read.

### Data & Integration

- **CSV Import** - Bulk import customers, contacts, leads, opportunities, sales, and other entities from CSV files with preview and validation.
- **Global Search** - Search across customers, contacts, leads, and opportunities from the app bar with instant navigation.
- **Tagging System** - Create color-coded tags and apply them to any entity for flexible categorization and filtering.
- **File Attachments** - Upload and manage file attachments on any entity (customers, contacts, leads, opportunities, support cases). Supports files up to 20MB with type-specific icons.
- **OData API** - Full OData v4 support for server-side filtering, sorting, and pagination on all entity endpoints.
- **Scalar API Docs** - Interactive API documentation.

### UI & Theming

- **Dark Mode** - Toggle between light and dark themes.
- **Theme Customization** - Change primary color with a theme picker that persists to local storage.
- **Responsive Layout** - Works on desktop and mobile with a collapsible navigation drawer.
- **MudBlazor Components** - Professional Material Design UI throughout.

### Security & Auditing

- **ASP.NET Core Identity** - User registration, login, password change, and role-based authorization.
- **Role Management** - Administrator role with user management capabilities.
- **JWT Authentication** - Token-based API authentication for the Blazor WASM client.
- **Audit Trail** - Automatic tracking of all entity changes (create, update, delete) with old/new values, user info, and timestamps via EF Core interceptor.
- **Activity Logging** - Login/logout events and entity access tracking.
- **Soft Delete** - Entities are soft-deleted (marked as deleted) rather than permanently removed, preserving data integrity.

---

## Tech Stack

| Technology | Description |
| --- | --- |
| ![.NET](https://img.shields.io/badge/.NET_10-512BD4?logo=dotnet) | Core application framework |
| ![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?logo=dotnet) | Web framework for building the server |
| ![Blazor](https://img.shields.io/badge/Blazor-512BD4?logo=blazor) | Frontend C# web framework |
| ![MudBlazor](https://img.shields.io/badge/MudBlazor-1E88E5?logo=M&logoColor=fff) | Material Design component library |
| ![Entity Framework Core](https://img.shields.io/badge/EF_Core-512BD4?logo=entityframework) | Object-Relational Mapper (ORM) |
| ![SQLite](https://img.shields.io/badge/SQLite-003B57?logo=sqlite&logoColor=white) | Embedded database engine |
| ![OData](https://img.shields.io/badge/OData-F26825?logo=odata&logoColor=white) | Standard for building RESTful APIs |
| ![Microsoft Graph](https://img.shields.io/badge/Microsoft_Graph-0078D4?logo=microsoft&logoColor=white) | Email integration via Graph API |

### Technical Features
- **Modern Frontend**: Single-page application with **Blazor WASM**
- **Beautiful UI**: Professional [MudBlazor](https://mudblazor.com/) components
- **Secure Authentication**: **ASP.NET Core Identity** with JWT tokens
- **Interactive Dashboard**: Real-time KPIs and analytics charts with GridStack.js
- **Email Integration**: **Microsoft Graph API** for sending emails with template support
- **Advanced Querying**: **OData** for flexible data access
- **API Documentation**: **Scalar** interactive API docs
- **Audit Logging**: Track all entity changes with old/new values
- **Communication Tracking**: Log all customer interactions
- **Data Import/Export**: CSV import and export functionality
- **File Attachments**: Upload and manage files on any entity
- **Notifications**: Real-time alerts for overdue items and upcoming deadlines

### Enterprise-Ready
- Data validation with annotations
- Proper entity relationships with navigation properties
- Indexed database queries
- Rate limiting protection (fixed window + sliding window)
- Role-based authorization
- Automatic timestamp tracking (CreatedDate, ModifiedDate)
- Soft delete support (ISoftDelete interface)
- Auditable entities (CreatedBy, ModifiedBy tracking)
- Audit trail with old/new value comparison

---

## Project Structure

```
MudBlazorCrmApp/
├── MudBlazorCrmApp/                    # ASP.NET Core Server
│   ├── Controllers/                    # API Controllers
│   │   ├── DashboardController.cs      # Dashboard KPIs & analytics
│   │   ├── EmailController.cs          # Email sending via Graph API
│   │   ├── EmailTemplateController.cs  # Email template CRUD
│   │   ├── SearchController.cs         # Global search
│   │   ├── ImportController.cs         # CSV import
│   │   ├── AuditLogController.cs       # Audit trail
│   │   ├── NotificationController.cs   # Notifications
│   │   ├── AttachmentController.cs     # File attachments
│   │   └── ...                         # Entity CRUD controllers
│   ├── Data/
│   │   └── ApplicationDbContext.cs     # EF Core context with audit interceptor
│   ├── Services/
│   │   ├── EmailService.cs             # Microsoft Graph email service
│   │   └── ImageService.cs             # Image upload/resize
│   └── *.Data.json                     # Seed data files
├── MudBlazorCrmApp.Shared/             # Shared Models
│   └── Models/                         # Entity models (20+ entities)
├── MudBlazorCrmApp.Shared.Blazor/      # Blazor UI Components
│   ├── Pages/                          # All pages (List/Add/Update per entity)
│   │   ├── CRMDashboard.razor          # Interactive dashboard
│   │   ├── PipelineBoard.razor         # Deal pipeline Kanban
│   │   ├── CalendarView.razor          # Calendar view
│   │   ├── Reports.razor               # Reporting page
│   │   ├── ImportData.razor            # CSV import
│   │   └── ...
│   ├── Components/                     # Reusable components
│   │   ├── TagSelector.razor           # Tag management
│   │   ├── EntityTimeline.razor        # Interaction timeline
│   │   ├── EntityAttachments.razor     # File attachments
│   │   └── SendEmailDialog.razor       # Email compose dialog
│   ├── Layout/
│   │   └── NavMenu.razor               # Navigation with search & notifications
│   └── Services/
│       └── AppService.cs               # HTTP client service
└── MudBlazorCrmApp.Blazor/             # Blazor WASM host
```

---

## 🏁 Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing.

### Prerequisites

Make sure you have the following tools installed:

*   [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (or the latest version)
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

3.  **Run the application**
    ```sh
    dotnet run --project MudBlazorCrmApp
    ```
    The application will launch at `http://localhost:5025`. The database will be created and seeded on the first run.

4.  **Or open in Visual Studio**
    *   Open `MudBlazorCrmApp.sln` with Visual Studio 2022.
    *   Set `MudBlazorCrmApp` as the startup project.
    *   Press `F5` to build and start.

### Email Configuration (Optional)

To enable email sending via Microsoft Graph API, configure user secrets:

```sh
cd MudBlazorCrmApp
dotnet user-secrets set "EmailSettings:TenantId" "<your-tenant-id>"
dotnet user-secrets set "EmailSettings:ClientId" "<your-client-id>"
dotnet user-secrets set "EmailSettings:ClientSecret" "<your-client-secret>"
dotnet user-secrets set "EmailSettings:SendFromUserId" "<sender-email>"
dotnet user-secrets set "EmailSettings:CcRecipient" "<optional-cc-email>"
```

Requires an Azure AD app registration with `Mail.Send` permission (application type).

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

### Email API
```
GET  /api/email/status               # Check if email is configured
POST /api/email/send                 # Send email (logs as Activity)
```

### Notification API
```
GET    /api/notification             # List notifications
GET    /api/notification/unread-count # Unread count
PUT    /api/notification/{id}/read   # Mark as read
PUT    /api/notification/read-all    # Mark all as read
POST   /api/notification/generate    # Auto-generate alerts
DELETE /api/notification/{id}        # Delete notification
```

### Audit & Activity APIs
```
GET    /api/auditlog                 # Entity change history
GET    /api/auditlog/{id}            # Audit log details
GET    /api/activitylog              # Login/logout events
POST   /api/authactivity/login       # Log login event
POST   /api/authactivity/logout      # Log logout event
```

### File Attachments API
```
GET    /api/attachment               # List all attachments
GET    /api/attachment/entity/{type}/{id}  # Attachments for entity
POST   /api/attachment               # Upload file (multipart, 20MB max)
DELETE /api/attachment/{id}          # Delete attachment
```

### Search & Import APIs
```
GET    /api/search?q={query}         # Global search
POST   /api/import/{entityType}      # CSV import
```

### Core CRUD APIs (OData-enabled)
```
GET/POST       /api/{entity}
GET/PUT/DELETE /api/{entity}/{id}
```

Supported entities: `customer`, `contact`, `lead`, `opportunity`, `sale`, `supportcase`, `todotask`, `activity`, `tag`, `emailtemplate`, `communication`, `product`, `service`, `vendor`, `address`, `reward`, `productcategory`, `servicecategory`

### OData Endpoints
```
GET /odata/{Entity}?$top=10&$skip=0&$orderby=Name&$filter=contains(Name,'test')&$count=true
```

All entities support OData v4 query options for server-side filtering, sorting, and pagination.

---

## Default Credentials

This application uses ASP.NET Core Identity for user authentication.

**Administrator**
- Email: `adminuser@example.com`
- Password: `testUser123!`

**Normal User**
- Email: `normaluser@example.com`
- Password: `testUser123!`

---

## Seed Data

The application seeds the following data on first run:

| Entity | Count | Description |
|--------|-------|-------------|
| Customers | 12 | Sample companies |
| Contacts | 12 | With company, title, status |
| Leads | 12 | Various sources and statuses |
| Opportunities | 12 | Across all pipeline stages |
| Sales | 12 | Completed sales records |
| Support Cases | 12 | Various priorities and statuses |
| Tasks | 12 | To-do items |
| Activities | 12 | Calls, emails, meetings, notes |
| Products | 12 | Sample products |
| Services | 12 | Sample services |
| Tags | 8 | Color-coded labels |
| Email Templates | 20 | Across 7 categories |
| Users | 2 | Admin + normal user |

---

## Contact

Don Potts - Don.Potts@DonPotts.com

Project Link: [https://github.com/donpotts/MudBlazorCrmApp](https://github.com/donpotts/MudBlazorCrmApp)

---

## Acknowledgments

- [MudBlazor](https://mudblazor.com/) - Amazing Blazor component library
- [Chart.js](https://www.chartjs.org/) - Beautiful charts
- [GridStack.js](https://gridstackjs.com/) - Drag-and-drop dashboard layouts
- [Microsoft Graph](https://learn.microsoft.com/en-us/graph/) - Email integration
- [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) - Robust backend framework

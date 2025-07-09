---

<br/>
<p align="center">
  <a href="https://github.com/donpotts/MudBlazorCrmApp">
    <img src="https://avatars.githubusercontent.com/u/1085058?v=4" alt="Logo" width="80" height="80">
  </a>

  <h1 align="center">MudBlazor CRM</h1>

  <p align="center">
    A feature-rich sample CRM application built with Blazor WASM and .NET, showcasing the power of MudBlazor and a modern ASP.NET Core backend.
    <br />
  </p>
</p>

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/donpotts/MudBlazorCrmApp/MudBlazorCrmApp.yml?logo=github&style=for-the-badge)

---

![MudBlazor CRM](./MudBlazorCrmApp/Assets/MudBlazorCRM.png)

---

## 🚀 About The Project

This repository is a comprehensive, modern web application built on the latest .NET stack. It serves as a practical example of a Customer Relationship Management (CRM) system using a Blazor WASM client and an ASP.NET Core server.

The goal is to demonstrate best practices and the seamless integration of powerful open-source technologies to build a real-world, data-driven application.

**Key highlights include:**
*   A clean, responsive UI powered by **MudBlazor**.
*   Secure user management with **ASP.NET Core Identity**.
*   An efficient and queryable API using **OData**.
*   A lightweight and cross-platform **SQLite** database.

---

## ✨ Features

*   🖥️ **Modern Frontend**: A rich, single-page application (SPA) experience with **Blazor WASM**.
*   🎨 **Beautiful UI Components**: Leverages the extensive and professional [MudBlazor](https://mudblazor.com/) component library.
*   🔐 **Secure Authentication**: Full user registration and login system via **ASP.NET Core Identity**.
*   🚀 **High-Performance Backend**: Built on the fast and reliable **ASP.NET Core Kestrel** web server.
*   🗃️ **Flexible Data Queries**: **OData** support allows for powerful and efficient API queries directly from the client.
*   📝 **Interactive API Docs**: Includes **Swagger (OpenAPI)** for easy API exploration and testing.
*   📊 **Kanban Task Management**: A sample Kanban board to demonstrate dynamic UI and data interaction.
*   🪶 **Lightweight Database**: Uses **Entity Framework Core** with **SQLite** for simple setup and development.

---

## 🛠️ Tech Stack

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

### Installation & Running

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

## Authentication

This application uses ASP.NET Core Identity for user authentication. To log in, navigate to the login page and enter your credentials.

Administrator

Username: adminUser@example.com

Password: testUser123!

Normal user

Username: normalUser@example.com

Password: testUser123!

---

## 📬 Contact

Don Potts - Don.Potts@DonPotts.com

Project Link: [https://github.com/donpotts/MudBlazorCrmApp](https://github.com/donpotts/MudBlazorCrmApp)
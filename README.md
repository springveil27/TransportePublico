<div align="center">
  <h1>🚌 Transporte Público RD - API</h1>
  <p><em>Sistema de gestión de rutas, paradas y horarios de transporte público.</em></p>
  
  <!-- Badges de Tecnologías -->
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#">
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET">
  <img src="https://img.shields.io/badge/Entity_Framework-512BD4?style=for-the-badge&logo=entityframework&logoColor=white" alt="EF">
  <img src="https://img.shields.io/badge/HTML5-E34F26?style=for-the-badge&logo=html5&logoColor=white" alt="HTML">
  <img src="https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black" alt="JS">
  <img src="https://img.shields.io/badge/GitHub_Actions-2088FF?style=for-the-badge&logo=github-actions&logoColor=white" alt="GitHub Actions">
</div>

<br>

## 📖 Descripción del Proyecto
**Transporte Público RD** es una aplicación web full-stack diseñada para administrar la información del transporte público. Permite gestionar rutas, paradas y horarios de manera eficiente. 

El backend está construido como una **API RESTful** robusta en C# (.NET), aplicando principios de **Clean Architecture (Arquitectura Limpia)** y patrones de diseño empresariales. El frontend es una interfaz ligera en HTML, CSS y JavaScript puro que consume los endpoints de la API.

---

## 🏗️ Arquitectura y Patrones de Diseño
Este proyecto destaca por su estructuración profesional, separando responsabilidades en 4 capas principales:

*   **🌐 API (Presentation Layer):** Controladores REST que reciben las peticiones HTTP, validan los datos y devuelven respuestas JSON.
*   **⚙️ Application (Business Logic):** Contiene los **Servicios** que orquestan la lógica de negocio, los **DTOs** (Data Transfer Objects) para mover datos de forma segura, y las interfaces de los servicios.
*   **🧱 Domain (Core):** Las entidades puras del negocio (`PublicRoute`, `Stops`, `Schedule`) sin dependencias externas.
*   **🗄️ Infrastructure (Data Access):** Implementación de Entity Framework Core (`DbContextApp`), el **Patrón de Repositorio** (`GenericRepository`, `RouteRepository`, etc.) y **Unit of Work** para transacciones atómicas.

---

## ✨ Características Principales
- ✅ **CRUD Completo:** Crear, leer, actualizar y eliminar Rutas, Paradas y Horarios.
- ✅ **Separación de Intereses:** Uso de DTOs para evitar exponer directamente las entidades de la base de datos al cliente.
- ✅ **Inyección de Dependencias:** Configuración nativa de .NET para un código desacoplado y fácil de testear.
- ✅ **Frontend Integrado:** Interfaz web funcional que consume la API mediante peticiones `fetch`.
- ✅ **CI/CD Básico:** Workflows de GitHub Actions configurados para integración continua.

---

## 🚀 Cómo ejecutar el proyecto

### 1️⃣ Backend (API en .NET)
**Requisitos:** [.NET SDK](https://dotnet.microsoft.com/download) instalado.

```bash
# Clonar el repositorio
git clone https://github.com/springveil27/TransportePublico.git
cd TransportePublico

# Restaurar dependencias de NuGet
dotnet restore

# Ejecutar la API 
dotnet run --project TransportePublicoRD.API

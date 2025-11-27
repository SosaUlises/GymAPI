# 🏋️‍♂️ Sosa Gym API
API RESTful para la gestión de clientes, rutinas, ejercicios, progreso físico y cuotas mensuales.  
Construida con **.NET**, **Clean Architecture**, **PostgreSQL** e **Identity + JWT**.

---

## 🚀 Características Principales

### 🔐 Autenticación y Autorización
- Registro y login con ASP.NET Core Identity.
- Hash seguro de contraseñas.
- JWT para autenticar peticiones.
- Protección por roles:
  ```csharp
  [Authorize(Roles = "Administrador")]
  ```
- Roles incluidos: **Administrador**, **Cliente**.

---

## 📦 Gestión de Datos (CRUD)

### 👥 Clientes
- Crear, editar, eliminar y consultar clientes.
- Asociado directamente al usuario Identity.

### 🧑‍🏫 Rutinas
- Rutinas → Días → Ejercicios.
- CRUD completo.

### 📈 Progreso del Cliente
- Registro del avance físico del cliente.

### 💰 Cuotas Mensuales
- Crear y gestionar cuotas por cliente.
- Estados: Pendiente / Pagado.
- Filtros por estado con comparación case-insensitive.
- Validaciones completas.

---

## 🧠 Calidad de Código y API
- Validación avanzada con FluentValidation.
- Manejo global de excepciones.
- Documentación Swagger.
- Mapeos limpios con AutoMapper.
- CQRS (Commands y Queries).
- Arquitectura limpia (Clean Architecture).

---

## 🏗️ Arquitectura

### 🧱 Capas del proyecto
```
📂 Domain          → Entidades y reglas base
📂 Application     → Commands, Queries, Validaciones, Servicios
📂 Infrastructure  → EF Core, Identity, JWT, Repositorios
📂 Api             → Controladores, Middlewares
```

### ⚙️ CQRS
- **Commands** → escriben datos (CreateClienteCommand, CreateRutinaCommand…)
- **Queries** → leen datos (GetClienteQuery, GetRutinaByIdQuery…)

### 💉 Inyección de Dependencias
- Configurada desde *DependencyInjectionService* e *InfrastructureService*.

---

## 🧰 Stack Tecnológico

| Categoría         | Tecnología |
|------------------|------------|
| Framework        | .NET 9 |
| Base de Datos    | PostgreSQL |
| ORM              | Entity Framework Core |
| Autenticación    | ASP.NET Core Identity |
| Autorización API | JWT |
| Validación       | FluentValidation |
| Mapeo            | AutoMapper |
| Documentación    | Swagger |

---

## ▶️ Cómo ejecutar el proyecto

```bash
dotnet restore
dotnet ef database update
dotnet run
```

---

## 🔐 Configuración de JWT con Secret Manager

```bash
dotnet user-secrets set "Jwt:Key" "TU_CLAVE_SECRETA"
dotnet user-secrets set "Jwt:Issuer" "Sosa.Gym.API"
dotnet user-secrets set "Jwt:Audience" "Sosa.Gym.API.FrontEnd"
```

---

## 🙌 Autor
Proyecto desarrollado por **Sosa Ulises** como API para gestión de gimnasio.


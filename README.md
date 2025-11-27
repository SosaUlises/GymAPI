# 🏋️‍♂️ Sosa Gym API
API RESTful para la gestión de clientes, rutinas, ejercicios, progreso físico y cuotas mensuales.  
Construida con **.NET**, **Clean Architecture**, **PostgreSQL** e **Identity + JWT**.

> [!NOTE]
> **🚀 DESPLIEGUE EN VIVO (LIVE DEMO)**
>
> El proyecto se encuentra desplegado en **Render**. Al hacer clic en el enlace, accederás a la interfaz de **Swagger UI**, donde podrás probar los endpoints de la API, autenticarte y ver los esquemas de datos.
>
> 👉 **[Ver Documentación y API en Vivo](https://gymapi-yln2.onrender.com/index.html)**

### 🔐 Guía para Probar la API

La API cuenta con seguridad JWT. Para probar los endpoints protegidos, sigue estos pasos según el rol que quieras testear:

#### 1. Rol Administrador (Acceso Total)
Utiliza estas credenciales precargadas para acceder a funcionalidades de gestión:

| Campo | Valor |
| :--- | :--- |
| **Email** | `admin@sosa.com` |
| **Password** | `Admin123!` |

**Pasos para autenticarse:**
1. Ve al endpoint `POST /api/Auth/login`.
2. Ingresa las credenciales de arriba y ejecuta ("Execute").
3. Copia el `token` que recibirás en la respuesta.
4. Sube al inicio de la página, haz clic en el botón verde **Authorize**.
5. Escribe: `Bearer TU_TOKEN_AQUI` (respetando el espacio después de Bearer) y dale a **Authorize**.

#### 2. Rol Cliente (Nuevo Usuario)
Si deseas probar el flujo de un usuario normal:

1. Ve al endpoint `POST /api/Cliente` (Crear Cliente).
2. Rellena el formulario (JSON) con tus datos y ejecútalo para registrarte.
3. Luego, usa tu nuevo email y contraseña en el endpoint de `Login` para obtener tu token de acceso.

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
| Framework        | .NET 8 |
| Base de Datos    | PostgreSQL |
| ORM              | Entity Framework Core |
| Autenticación    | ASP.NET Core Identity |
| Autorización API | JWT |
| Validación       | FluentValidation |
| Mapeo            | AutoMapper |
| Documentación    | Swagger |

---

## 🙌 Autor
Proyecto desarrollado por **Sosa Ulises** como API para gestión de gimnasio.


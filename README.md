# 🏋️‍♂️ Sosa Gym API

API REST para la gestión integral de un gimnasio: usuarios, clientes, entrenadores, rutinas, cuotas y generación de rutinas asistida por IA.

Desarrollada con **.NET 8**, **Clean Architecture**, **CQRS**, **PostgreSQL**, **ASP.NET Identity + JWT**

---

## 🚀 Demo en Vivo (Swagger)

La API se encuentra desplegada en **Render** y cuenta con documentación interactiva mediante **Swagger UI**:

👉 https://gymapi-yln2.onrender.com/index.html

Desde Swagger podés:
- Autenticarte con JWT
- Probar endpoints según el rol
- Explorar modelos y contratos de la API

---

## 👥 Roles del Sistema

| Rol | Descripción |
|----|-------------|
| **Administrador** | Acceso total al sistema |
| **Entrenador** | Gestión de rutinas, días, ejercicios y asignaciones |
| **Cliente** | Lectura de rutinas asignadas y consulta de estados |

---

## 🔐 Autenticación y Autorización

- ASP.NET Core Identity
- JWT Bearer Tokens
- Autorización por roles mediante atributos

Ejemplo:

```csharp
[Authorize(Roles = "Administrador")]
```

### Credenciales Administrador (Demo)

- **Email:** admin@sosa.com  
- **Password:** Admin123!

---

## 🧩 Funcionalidades Principales

### 🧑‍🏫 Rutinas de Entrenamiento

- CRUD completo de rutinas
- Estructura jerárquica:
  - Rutina → Días → Ejercicios
- Asignación y desasignación de rutinas a clientes
- Un cliente puede tener múltiples rutinas activas
- Control de permisos por rol (Administrador / Entrenador)

---

### 💰 Cuotas Mensuales

- Generación automática de cuotas mensuales
- Estados de cuota:
  - Pendiente
  - Pagada
  - Vencida
- Cálculo automático de vencimiento por período
- Validación de acceso según estado de cuota

---

### 🪪 Acceso por DNI

- Ingreso de clientes mediante DNI
- Verificación automática de:
  - Existencia del cliente
  - Estado de la cuota
- Bloqueo de acceso si la cuota está vencida

---

### 🤖 IA – Generador de Rutinas (Preview)

- Integración con OpenAI
- Generación de rutinas personalizadas según:
  - Objetivo
  - Nivel
  - Días por semana
  - Duración por sesión
  - Equipamiento
  - Restricciones
- Endpoint de tipo **preview** 
- Respuesta estrictamente en JSON
- Compatible con el modelo del dominio
- Blindaje de seguridad: `pesoUtilizado = 0` en todos los ejercicios

---

## 🧠 Arquitectura y Calidad de Código

- Clean Architecture
- CQRS (Commands / Queries)
- AutoMapper
- FluentValidation
- Manejo global de excepciones
- Separación estricta de responsabilidades
- Código preparado para escalar

---

## 🏗️ Arquitectura del Proyecto

### 🧱 Capas

```
📂 Domain          → Entidades y reglas de negocio
📂 Application     → Commands, Queries, Validaciones, Servicios
📂 Persistence     → EF Core, Configuraciones, Migrations
📂 API             → Controllers, Auth, Swagger
```

---

## 🧰 Stack Tecnológico

| Categoría | Tecnología |
|---------|------------|
| Framework | .NET 8 |
| API | ASP.NET Core |
| Base de Datos | PostgreSQL (Neon) |
| ORM | Entity Framework Core |
| Autenticación | Identity + JWT |
| Validación | FluentValidation |
| Mapping | AutoMapper |
| IA | OpenAI API |
| Deploy | Render |
| Documentación | Swagger |

---

## 👤 Autor

**Ulises Sosa**

Proyecto desarrollado como **portfolio backend profesional**, con foco en arquitectura limpia, seguridad por roles, lógica de negocio real e integración con inteligencia artificial.


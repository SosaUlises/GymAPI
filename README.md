# 🏋️‍♂️ Sosa Gym API

API REST para la gestión integral de un gimnasio, desarrollada en **.NET 8** con un enfoque técnico y orientado a arquitectura limpia, seguridad por roles y lógica de negocio real.

---

## 📌 Descripción Técnica

Sosa Gym API implementa **Clean Architecture** y **CQRS** para separar responsabilidades, facilitar el mantenimiento y permitir la escalabilidad del sistema. Utiliza **ASP.NET Core Identity + JWT** para autenticación y autorización, **PostgreSQL (Neon)** como base de datos y **Entity Framework Core** como ORM.

Incluye además un módulo de **IA (OpenAI)** para la generación de rutinas de entrenamiento en modo *preview*, pensado como extensión inteligente del dominio.

---

## 🚀 Demo en Vivo

Swagger UI desplegado en Render:

👉 https://gymapi-yln2.onrender.com/index.html

Desde Swagger se puede:
- Autenticarse con JWT
- Probar endpoints protegidos por rol
- Inspeccionar contratos y esquemas

---

## 👥 Roles del Sistema

| Rol | Capacidades |
|----|-------------|
| **Administrador** | Acceso total al sistema |
| **Entrenador** | Gestión de rutinas, días, ejercicios y asignaciones |
| **Cliente** | Lectura de rutinas asignadas y estado de cuotas |

La autorización se maneja mediante atributos:

```csharp
[Authorize(Roles = "Administrador,Entrenador")]
```

---

## 🔐 Seguridad

- ASP.NET Core Identity
- JWT Bearer Tokens
- Hash seguro de contraseñas
- Autorización basada en roles

---

## 🧩 Funcionalidades Principales

### 🧑‍🏫 Rutinas de Entrenamiento
- CRUD completo de rutinas
- Estructura jerárquica:
  - Rutina → Días → Ejercicios
- Asignación y desasignación de rutinas a clientes
- Múltiples rutinas activas por cliente

### 💰 Cuotas Mensuales
- Generación automática de cuotas por período
- Estados:
  - Pendiente
  - Pagada
  - Vencida
- Cálculo automático de vencimiento
- Validación de acceso según estado de cuota

### 🪪 Acceso por DNI
- Ingreso de clientes mediante DNI
- Verificación automática:
  - Existencia del cliente
  - Estado de cuota
- Bloqueo de acceso si la cuota está vencida

### 🤖 IA – Generador de Rutinas (Preview)
- Integración con OpenAI
- Generación de rutinas personalizadas según:
  - Objetivo
  - Nivel
  - Días por semana
  - Duración
  - Equipamiento
  - Restricciones
- Endpoint *preview* 
- Respuesta estrictamente en JSON
- Compatible con modelos del dominio
- Blindaje de seguridad: `pesoUtilizado = 0`

---

## 🧠 Arquitectura

### Capas del Proyecto

```
📂 Domain        → Entidades y reglas de negocio
📂 Application   → Commands, Queries, Validaciones, Servicios
📂 Persistence   → EF Core, Configuraciones, Migrations
📂 API           → Controllers, Auth, Swagger
```

### CQRS
- **Commands**: operaciones de escritura
- **Queries**: operaciones de lectura

---

## 🧪 Calidad de Código

- FluentValidation
- AutoMapper
- Manejo global de excepciones
- Separación estricta de responsabilidades
- Código preparado para escalar y extender

---

## 🧰 Stack Tecnológico

| Categoría | Tecnología |
|---------|------------|
| Framework | .NET 8 |
| API | ASP.NET Core |
| Base de Datos | PostgreSQL (Neon) |
| ORM | Entity Framework Core |
| Auth | Identity + JWT |
| Validación | FluentValidation |
| Mapping | AutoMapper |
| IA | OpenAI API |
| Deploy | Render |
| Docs | Swagger |

---

## 👤 Autor

**Ulises Sosa**  
Proyecto desarrollado como portfolio backend profesional, con foco en arquitectura limpia, seguridad, dominio realista e integración con IA.


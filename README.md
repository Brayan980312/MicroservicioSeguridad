# MicroServicioSeguridad

**Microservicio para autenticación y gestión de usuarios con JWT**

MicroServicioSeguridad es un proyecto desarrollado con **ASP.NET Core 8** que expone endpoints para registro de usuarios, login y consulta de usuarios existentes. Implementa **JWT** para autenticación, encriptación de datos sensibles y utiliza **ASP.NET Core Identity** para la gestión segura de contraseñas.

El proyecto sigue **arquitectura limpia** y respeta los principios **SOLID**, aplicando patrones de diseño como **Repository, Service Layer, DTO, Service Locator y Unit of Work**.

---

## Tecnologías utilizadas

- **Backend:** ASP.NET Core 8
- **Autenticación:** JWT
- **Gestión de contraseñas:** ASP.NET Core Identity
- **Encriptación de datos sensibles**
- **Patrones de diseño:** Repository, Service Layer, DTO, Service Locator, Unit of Work
- **Principios de arquitectura:** SOLID, arquitectura limpia
- **Documentación de API:** Swagger/OpenAPI para visualización y prueba de endpoints
- **Dependencias externas:** Proyecto `Utilitarios` ([https://github.com/Brayan980312/Utilitarios](https://github.com/Brayan980312/Utilitarios))

---

## Requisitos previos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- **Visual Studio 2022**
- **.NET 8 SDK**
- **Proyecto `Utilitarios`** en la **misma raíz** que `MicroServicioSeguridad`
- Conexión a la base de datos configurada correctamente

---

## Configuración

1. Clona el repositorio:

```bash
   git clone https://github.com/Brayan980312/MicroservicioSeguridad
```

2. Descarga también el proyecto de utilitarios:
   (Asegúrate de que ambos proyectos estén en la misma carpeta raíz.)

```bash
   git clone https://github.com/Brayan980312/Utilitarios
```

3. Abre MicroServicioSeguridad en Visual Studio 2022 y establece API como proyecto de inicio.

4. Configura el archivo appsettings.json para que apunte a tu instancia de base de datos:

```bash
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "FlyHub": "Server=<SERVIDOR>;Database=<BASE_DATOS>;Persist Security Info=True;User ID=<USUARIO>;Password=<CLAVE>;MultipleActiveResultSets=True;App=EntityFramework;Encrypt=False"
  },
  "AllowedHosts": "*"
}

```

---

## Arquitectura del proyecto

El proyecto sigue **arquitectura limpia** con capas bien definidas:

- **API**: Controladores que exponen los endpoints
- **Domain / DTOs**: Modelos de dominio y Data Transfer Objects, Lógica de negocio y validaciones
- **Infrastructure / Repository**: Acceso a base de datos
- **Utilitarios**: Funcionalidades compartidas (proyecto externo)
- **Patrones aplicados**: Repository, Service Layer, DTO, Service Locator, Unit of Work

## Buenas prácticas y recomendaciones

- Mantener actualizado el proyecto **Utilitarios** para evitar errores de compilación.
- Revisar siempre las rutas y configuraciones de **appsettings.json** antes de ejecutar.
- Validar la seguridad del **JWT Secret** y cambiarlo en producción.
- Seguir los principios **SOLID** para cualquier nueva implementación.
- Documentar cualquier nuevo endpoint siguiendo la misma estructura.

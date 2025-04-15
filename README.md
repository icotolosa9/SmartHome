# SmartHome - API RESTful

Este proyecto es una API RESTful desarrollada en .NET 8 con C#, como parte del proyecto académico de las materias Diseño de Aplicaciones 1 y 2 (Universidad ORT Uruguay).  
Incluye un frontend en Angular conectado a la API.

## 🛠️ Tecnologías utilizadas

- .NET 8 + C#
- Entity Framework Core
- Angular (frontend)
- JWT (autenticación)
- SQL Server
- Git
- xUnit (TDD)

## 🧱 Arquitectura

- Arquitectura en capas (Presentation, Logic, Repository, Domain)
- Aplicación de principios SOLID y Clean Code
- Uso de patrones de diseño (Factory, Strategy, Adapter, etc.)
- Separación de responsabilidades
- Manejo estructurado de errores y validaciones

## ⚙️ Cómo correr el proyecto

1. Clonar este repositorio
2. Abrir la solución `.sln` en Visual Studio
3. Configurar la cadena de conexión a SQL Server en `appsettings.json`
4. Ejecutar migraciones si es necesario
5. Correr la API y el frontend desde Angular (`ng serve`)

## 🧪 Testing

- Desarrollo orientado a pruebas (TDD)
- Pruebas automatizadas con xUnit en la capa lógica
- Pruebas de integración básicas

## 👤 Autor

Francisco Tolosa  
[LinkedIn](https://www.linkedin.com/in/franciscotolosa9) | [GitHub](https://github.com/icotolosa9)

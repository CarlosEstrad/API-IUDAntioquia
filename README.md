# 🚀 API Core: Playlist Manager Backend (.NET 8)

Este repositorio constituye el núcleo lógico de la solución "Playlist Manager". Proporciona una API RESTful robusta, segura y escalable para la gestión de usuarios, canciones y listas de reproducción.

---

# 📝 Descripción del Proyecto

El sistema permite la administración centralizada de bibliotecas musicales personales. Los usuarios pueden autenticarse, explorar un catálogo global de canciones y organizar su contenido en listas personalizadas con persistencia total en base de datos relacional.

---

# 🛠️ Tecnologías Utilizadas

Lenguaje: C# 12

Framework: .NET 8 Web API

ORM: Entity Framework Core & Dapper (híbrido para ejecución de SPs)

Base de Datos: SQL Server 2019+

Seguridad: JWT (JSON Web Tokens) & ASP.NET Core Identity

Documentación: Swagger (OpenAPI 3.0)

---

# 🏗️ Explicación de la Arquitectura

Se ha implementado una Arquitectura en Capas para separar responsabilidades y facilitar el mantenimiento:

Capa de Presentación (Controllers): Maneja las peticiones HTTP y valida los modelos de entrada.

Capa de Negocio (Services): Contiene la lógica de validación, reglas de negocio y orquestación de datos.

Capa de Acceso a Datos (Data Layer): Utiliza un enfoque Database-First donde la lógica compleja (CRUD de canciones y playlists) reside en Procedimientos Almacenados para maximizar el rendimiento.

Capa de Modelos (DTOs): Desacopla las entidades de la base de datos de los esquemas de respuesta JSON.

---

# 🗄️ Modelo de Base de Datos y Programabilidad

Diseño Relacional

El modelo se basa en un esquema normalizado que garantiza integridad referencial mediante el uso de:

Users: Perfiles y credenciales.

Playlists: Cabeceras de listas (Relación 1:N con Users).

Songs: Catálogo maestro de pistas.

PlaylistSongs: Tabla asociativa (Relación M:N) con borrado en cascada.

Consultas, Vistas e Índices

Vista vw_GetAllSongs: Abstrae la conversión de segundos a formato MM:SS.

Procedimientos Almacenados: Se implementaron sp_CreatePlaylist, sp_AddSongToPlaylist y sp_CreateUser para asegurar que las transacciones sean atómicas a nivel de motor.

Índices Críticos:

IX_Users_Email: Optimiza el Login (búsqueda O(log n)).

IX_Playlists_UserId: Agiliza la carga de la biblioteca del usuario.

---

# 🚀 Instalación y Ejecución

Clonar: git clone https://github.com/CarlosEstrad/API-IUDAntioquia.git

Base de Datos: Ejecutar los scripts de la carpeta /database en su instancia de SQL Server.

Configurar Variables: Editar appsettings.json con sus credenciales.

Restaurar y **Ejecutar**:
```bash
dotnet restore
dotnet run
```


Swagger: Disponible en https://localhost:[PORT]/swagger.

---

# ⚙️ Variables de Entorno Necesarias

Para el correcto funcionamiento, asegúrese de configurar:

ConnectionStrings:DefaultConnection: Cadena de conexión a SQL Server.

Jwt:Key: Llave secreta para la firma de tokens (mínimo 32 caracteres).

Jwt:Issuer / Jwt:Audience: Identificadores para la validación de tokens.

---

# 🧪 Instrucciones para Pruebas

La API incluye pruebas unitarias y de integración:
```bash
dotnet test
```

Las pruebas cubren la lógica de los servicios y la correcta generación de respuestas del objeto Reply<T>.

---

# 💡 Justificación de Decisiones Técnicas

Uso de Stored Procedures: Se eligieron sobre LINQ para operaciones de escritura complejas para reducir la carga de memoria en el servidor web y aprovechar los planes de ejecución precompilados de SQL Server.

JWT en lugar de Cookies: Para permitir el desacoplamiento total del Frontend y facilitar el uso de la API en futuras aplicaciones móviles.

Arquitectura Híbrida EF Core/Dapper: EF Core para mapeo de entidades simples y Dapper para la ejecución rápida de Procedimientos Almacenados de alto rendimiento.

---

# 💾 Plan de Backup y Recuperación

Full Backup: Programado diariamente a las 00:00 hrs.

Differential Backup: Cada 6 horas para capturar cambios recientes.

Transaction Log: Cada 30 minutos (Point-in-time recovery) en entornos de producción.

Recuperación: El script 01_Initial_Setup.sql permite reconstruir la estructura base en menos de 1 minuto ante un desastre total.

---

# 🤖 Documentación del Uso de IA

Durante el desarrollo de este proyecto, se utilizó Inteligencia Artificial (LLM) como asistente de productividad para:

Generación de plantillas base de Procedimientos Almacenados.

Optimización de consultas SQL y sugerencia de índices.

Creación de esqueletos para pruebas unitarias en el Frontend.

Redacción de documentación técnica y READMEs.

**Nota:** Todo el código generado por IA fue revisado, refactorizado y validado manualmente para asegurar que cumple con los requerimientos específicos de la prueba.

---

**Autor:** Carlos Estrada | Año: 2026

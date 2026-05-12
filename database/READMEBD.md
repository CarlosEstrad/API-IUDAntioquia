# 🗄️ Capa de Persistencia: MusicPlaylistDB

Este apartado contiene la definición técnica, el esquema relacional y la lógica programable que sustenta el sistema de gestión de listas de reproducción.

---

# 🚀 Guía de Despliegue

Para garantizar la integridad referencial, los scripts deben ejecutarse en el siguiente orden dentro de SQL Server Management Studio (SSMS):

* **01_Initial_Setup.sql:** Creación de tablas core (Users, Playlists, Songs), relaciones de integridad, borrado en cascada y carga de catálogo inicial (Seed Data).

* **02_Programmability.sql:** Implementación de la capa de abstracción mediante Vistas y Procedimientos Almacenados.

---

# 🏗️ Arquitectura de Datos

La base de datos sigue un modelo relacional normalizado diseñado para el alto rendimiento y la integridad de la información.

Seguridad: Relación directa entre usuarios y sus listas de reproducción mediante llaves foráneas.

Flexibilidad: Relación muchos-a-muchos (M:N) entre listas y canciones a través de la tabla asociativa PlaylistSongs.

Mantenibilidad: Implementación de ON DELETE CASCADE para evitar registros huérfanos al eliminar perfiles o listas.

---

# 🛠️ Programabilidad y Lógica

Se ha delegado parte de la lógica de negocio al motor de base de datos para reducir la latencia y optimizar el procesamiento en el servidor de aplicaciones.

Estructuras de Análisis (Vistas):

vw_GetAllSongs: Catálogo maestro que transforma automáticamente la duración técnica (segundos) a un formato legible por el usuario (MM:SS).

vw_GetPlaylistsSongs: Vista de tipo Master-Detail que permite recuperar listas completas con sus canciones en una sola operación de lectura.

Procedimientos Almacenados:

Gestión de Identidad: Uso de SCOPE_IDENTITY() para el retorno seguro de IDs generados tras inserciones.

Integridad de Listas: Lógica de validación en sp_AddSongToPlaylist para prevenir la adición duplicada de canciones.

Eficiencia: Implementación de SET NOCOUNT ON para minimizar el tráfico de red con la API de .NET.

---

# 📊 Estrategia de Optimización (Índices)

Se han implementado índices no agrupados (NON-CLUSTERED) en campos estratégicos para garantizar una respuesta ágil:

IX_Users_Email: Optimización de velocidad crítica en el inicio de sesión.

IX_Playlists_UserId: Agilidad en la carga de bibliotecas personales por usuario específico.

IX_Songs_Title: Mejora en los tiempos de respuesta del buscador global de canciones.

---

# 🧪 Validación de Datos

El esquema ha sido probado para soportar:

Concurrencia: Integridad en inserciones simultáneas de canciones.

Consistencia: Validación de tipos de datos y restricciones de unicidad en correos y nombres de usuario.

---

# ✒️ Autor

Carlos Estrada - Desarrollador Full Stack

Tecnología: SQL Server / T-SQL

---
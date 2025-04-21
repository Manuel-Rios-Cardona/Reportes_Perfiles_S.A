# PerfilesSA
## Sistema de Administración de Controles PerfilesSA

🚀 **Detalle** 📜

Sistema web creado para la administración de PerfilesSA, facilitando la gestión eficaz y ordenada de empleados y departamentos.

## Las Tecnologías Empleadas
- Lenguaje de Escriptura: C#
- Esquema: ASP.NET Forms Web
- Registro de Datos: SQL Server MySQL
- Backend: HTML, CSS, JavaScript, Javascript
- Regulación de Versiones: Git

## Instalación:
Clona el repositorio: 
```bash
git clone https://github.com/Manuel-Rios-Cardona/Reportes_Perfiles_S.A.git
```
- Abre el proyecto en Visual Studio.
- Restaura los paquetes NuGet necesarios.
- Configura la cadena de conexión en el archivo Web.config para apuntar a tu instancia de SQL Server.
- Ejecuta el script BaseDeDatos.sql para crear la base de datos.
- Inicia la aplicación desde Visual Studio.

💻 **Requisitos Antecedentes**
- Visual Studio 2019 y más avanzado
- SQL Server 2019 o más avanzado
- .NET Framework 4.7.2 u otro equivalente
- Windows 10 o más recientes

## Instalación de manera detallada Paso a Paso

### 1: Configuración de la Base de Datos
- Abrir el estudio de gestión de SQL Server.
- Establecer una reciente base de datos denominada "PerfilesSA".
- Examinar el archivo BaseDeDatos.sql ubicado en la raíz del proyecto y abrirlo.
- Implementar el script íntegro para la creación de las tablas:
  - Trabajadores (Empleados)
  - Departamentos (Secciones)
  - Y sus correspondientes vínculos

### 2. Configuración del Proyecto
- Clonar o descargar el repositorio
- Abrir la solución PerfilesSA.sln en Visual Studio
- Abrir el archivo Web.config
- Modificar la cadena de conexión:
  - Aplicar a Empleado.cs
  - Reemplazar "TU_SERVIDOR" con el nombre de tu servidor SQL Server

### 3. Compilación y Ejecución
- Restaurar los paquetes NuGet (Click derecho en la solución → Restaurar paquetes NuGet)
- Compilar la solución (F6 o Build → Build Solution)
- Ejecutar el proyecto (F5 o IIS Express)

## 📁 Estructura del Proyecto
- Inicio.aspx - Página principal con gestión de empleados
- Departamentos.aspx - Gestión de departamentos
- Reportes.aspx - Reportes y estadísticas
- Modelos/ - Clases de modelo (Empleado.cs, Departamento.cs)
- Servicios/ - Lógica de negocio
- BaseDeDatos.sql - Script de creación de base de datos

## Respuesta a Problemas Compartidos

### Conexión fallida con la base de datos:
- Comprobar que SQL Server se encuentre en funcionamiento.
- Verificar que la cadena de vinculación sea adecuada
- Garantizar la presencia del usuario con permisos en la base de datos

### Invalidez en la compilación:
- Es importante confirmar que todos los paquetes NuGet se encuentren restaurados.
- Garantizar el uso de la versión adecuada de.NET Framework.

### Error al realizar la ejecución del script SQL:
- Garantizar la existencia de la base de datos "PerfilesSA"
- Comprobar que no existan tablas repetidas.

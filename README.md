# Sistema de YourBus

Este es un sistema de reservas de viajes en autobús, desarrollado con **ASP.NET Core MVC** y **Entity Framework Core**. La aplicación permite a los usuarios registrarse, iniciar sesión y gestionar sus reservas de viajes. También incluye un sistema de autenticación basado en **ASP.NET Identity**.

## Video de Presentación del proyecto

[LINK VIDEO](#)


## Funcionalidades

1. **Registro e Inicio de Sesión de Usuarios**: Los usuarios deben poder registrarse y autenticarse en el sistema. Validación de datos de entrada y manejo de errores.
2. **Búsqueda de Rutas**: Permitir a los usuarios buscar rutas de buses por origen, destino, y fecha. Mostrar resultados de búsqueda relevantes y filtrables.
3. **Selección de Asientos**: Los usuarios deben poder ver la disposición de asientos y seleccionar uno o más
asientos disponibles.
4. **Procesamiento de Pagos**: Integrar un sistema de pago simulado (puede ser una pasarela de pago ficticia). Confirmar pagos y generar un recibo o ticket digital.
5. **Gestión de Reservas**: Los usuarios pueden ver su historial de reservas y detalles de las mismas. Implementar funcionalidad para cancelar reservas dentro de un tiempo
determinado

## Estructura del Proyecto

- **Modelos**:
  - **Rutas de Buses**: Información sobre buses, horarios, precios y destinos.
  - **Usuarios**: Datos sobre usuarios registrados, historial de compras y datos personales.
  - **Reservas**: Detalles de las reservas, incluyendo usuario, ruta, asiento seleccionado y estado del pago.

- **Vistas**:
  - **Página Principal**
  - **Resultados de Búsqueda**
  - **Detalles de Ruta**
  - **Formulario de Reserva**
  - **Confirmación de Reserva**

- **Controladores**:
  - **Controlador de Búsqueda**
  - **Controlador de Reserva**
  - **Controlador de Pago**
  - **Controlador de Usuario**

## Tecnologías Utilizadas

- **ASP.NET Core MVC**: Framework principal de la aplicación.
- **Entity Framework Core**: ORM utilizado para interactuar con la base de datos SQL Server.
- **ASP.NET Identity**: Para manejar la autenticación y el registro de usuarios.
- **SQL Server**: Base de datos relacional para almacenar usuarios, rutas y reservas.

## Instalación

1. Clona el repositorio:

```js
git clone https://github.com/jorneycr/YourBus-UNED
```

2. Restaura las dependencias

```js
dotnet restore
```

3. Instalcion de SQL SERVER

```js
 Sql Server
```

3. Ajustas credenciales en el archivo de appsettings.json

```js
         "DefaultConnection":"Server=tcp:jorney.database.windows.net,1433;Initial Catalog=YourBus;Persist Security Info=False;User ID=jorney;Password=TU_PASSWORD*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

4. Si no tienes un certificado personalizado, puedes generar uno de desarrollo con

```js
     dotnet dev-certs https --trust
```

5. Ejecución del Proyecto

```js
dotnet run

dotnet run --launch-profile https

```


6. Ejecución del Proyecto con watch

```js
dotnet watch run
```
7. URL

```js
     https://localhost:7176/
```


## Migraciones de Base de Datos usando la CLI de .NET

1. Crea una nueva migración, si ya existen, ir al paso 2

```js
dotnet ef migrations add InitialMigration
```

2. Aplica las migraciones

```js
dotnet ef database update
```

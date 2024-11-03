# TechnicalTest APP

Este repositorio contiene el proyecto TechnicalTest API, una aplicación web sencilla en .NET 8 que permite el registro y autenticación de usuarios mediante autenticación basada en tokens.

## Instrucciones de Configuración

1. **Configuración de la Base de Datos**
   - Dirígete a la carpeta `Scripts` y ejecuta el script `InitScript.sql` proporcionado.
   - Este script creará la base de datos `TechnicalTest` junto con dos tablas: `Users` y `Phones`.
   - Si tienes SQL Server instalado, puedes hacer doble clic en el archivo de script para abrirlo y ejecutarlo.

2. **Configuración del Proyecto**
   - Abre el archivo de solución ubicado en la raíz del proyecto. Si tienes Visual Studio instalado, al hacer doble clic en el archivo `.sln` se abrirá automáticamente. De lo contrario, instala Visual Studio y vuelve a intentarlo.
   - Asegúrate de que `TechnicalTest.API` esté configurado como el proyecto de inicio en Visual Studio.

3. **Ejecutar el Proyecto**
   - Presiona `F5` o selecciona la opción "Run" en Visual Studio. Esto lanzará la aplicación y abrirá Swagger en tu navegador predeterminado.
   - Utiliza Swagger para explorar los endpoints disponibles para registrar y autenticar usuarios.

## Endpoints de la API

### Registrar un Usuario

Request
```http

POST /api/Register
{
  "Name": "Carlos",
  "Email": "Carlos@gmail.com",
  "Password": "Asdf123456",
  "Phones": [
    {
      "Number": 8889999,
      "CityCode": 1,
      "CountryCode": 57
    }
  ]
}
```

Response

```http
{
  "Message": null,
  "Content": {
    "Id": "b6291a62-173a-41bf-b94e-4dc3f857d66f",
    "Created": "2024-11-03T05:12:23.1690509Z",
    "Modified": "2024-11-03T05:12:23.1690959Z",
    "LastLogin": "2024-11-03T05:12:23.1691405Z",
    "IsActive": true,
    "Name": "Carlos",
    "Email": "Carlos@gmail.com",
    "Token": "<JWT Token>"
  }
}
```

### Iniciar sesión con un Usuario

POST /api/Login

Request

```http
{
  "Email": "Carlos@gmail.com",
  "Password": "Asdf123456"
}
```

Response

```http
{
  "Message": null,
  "Content": {
    "Name": "Carlos",
    "Email": "Carlos@gmail.com",
    "Token": "<JWT Token>"
  }
}

```

## Appsettngs
### El archivo appsettings.json contiene configuraciones esenciales:

```http
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=DESKTOP-8HN9PD6\\SQLEXPRESS;Database=TechnicalTest;Integrated Security=True;TrustServerCertificate=True;"
  },
  "Validations": {
    "EmailRegex": "[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$",
    "PasswordRegex": "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]*$",
    "PasswordMinLengh": 8,
    "PasswordMaxLengh": 20
  },
  "Authentication": {
    "Duration": 120,
    "SecretKey": "26b@2d15*2e9$3@#%D87@#$%V%5BHD:D"
  },
  "AllowedHosts": "*"
}
```

## General DIagram
![Logo](https://raw.githubusercontent.com/juanRosario77/TechnicalTest/refs/heads/main/Documentation/TechnicalTest%20APP%20-%20General%20Diagram.png)


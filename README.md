# Sistema de Ventas Core

Este proyecto es un **Sistema de Ventas** que permite la gestión de productos, clientes y ventas. Los usuarios deben iniciar sesión para acceder al sistema. Está diseñado con una interfaz amigable y segura para manejar las transacciones.

## Características

- **Autenticación de Usuarios**: Login con credenciales seguras.
- **Gestión de Productos**: Crear, leer, actualizar y eliminar productos (CRUD).
- **Gestión de Clientes**: Administrar información de clientes.
- **Gestión de Ventas**: Registro de ventas realizadas.
- **Reportes de Ventas**: Generación de reportes para analizar el rendimiento.
- **Control de Acceso**: Usuarios con roles de administrador y vendedor.

## Tecnologías Utilizadas

- **Backend**:
  - ASP .Net
  - Base de datos: Microsoft SQL Server 
  
- **Frontend**:
  - Angular
  - Bootstrap o Material UI para el diseño
- **Otros**:
  - Git y GitHub para control de versiones
- **Deployado**
- Vercel
- Azure

## Instalación

1. Clonar el repositorio:

   ```bash
   git clone https://github.com/usuario/sistema-de-ventas.git
   cd sistema-de-ventas
   ```

2. Instalar dependencias del backend:

```bash
cd backend
npm install
```

3.Configurar variables de entorno:

```bash
DB_HOST=localhost
DB_USER=root
DB_PASSWORD=tu_password
DB_NAME=sistema_ventas
JWT_SECRET=tu_secreto_jwt
```

4.Ejecutar el servidor backend:

```bash
npm start
```

```bash
cd frontend
npm install
```

5.Ejecutar el servidor frontend:

```bash
npm start
```

## Uso

1. Abre tu navegador y accede a `http://localhost:4200`.
2. Inicia sesión con las credenciales proporcionadas.
3. Utiliza las siguientes funcionalidades:

### Autenticación

- **Login**: Los usuarios pueden iniciar sesión utilizando su nombre de usuario y contraseña.
- **Control de acceso**: Dependiendo del rol del usuario (administrador o empleado), se tendrá acceso a diferentes secciones del sistema.

### Gestión de Productos

- **Crear producto**: Ingresa la información del nuevo producto, como nombre, precio y categoría.
- **Eliminar producto**: Borra productos del sistema.
- **Ver productos**: Consulta el listado de productos disponibles.

### Gestión de Usuarios

- **Añadir Usuario**: Crea un nuevo registro de usuario con la información personal y de contacto.
- **Actualizar cliente**: Modifica la información de usuario existentes.
- **Eliminar cliente**: Elimina un usuario del sistema.
- **Ver clientes**: Revisa la lista de todos los usuarios registrados.

### Gestión de Ventas

- **Realizar venta**: Registra una nueva venta seleccionando productos y cliente.
- **Consultar ventas**: Visualiza las ventas realizadas, incluyendo detalles como fecha, cliente y productos vendidos.
- **Filtrar ventas**: Filtra las ventas por fechas, compara informacion de reportes y el hsitorial de ventas

### Reportes

- **Generar reportes**: Crea informes de ventas basados en filtros, se puede conocer el producto mas vendido y la categoría más vendida.
- **Descargar reportes**: Exporta los reportes generados en formato Excel.

## Contribución

1. Haz un fork del proyecto.
2. Crea una nueva rama para tus cambios:

   ```bash
   git checkout -b feature/nueva-funcionalidad
   ```

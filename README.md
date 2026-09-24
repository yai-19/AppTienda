# 🛍️ AppTienda - Sistema de Gestión de Tienda y Finanzas

**AppTienda** es una aplicación web intuitiva, moderna y adaptada a dispositivos móviles diseñada para gestionar el inventario de productos, registrar ventas en tiempo real con cálculo automático de ganancias y llevar el control contable de gastos operativos.

El proyecto está desarrollado en **ASP.NET Core MVC (.NET 9)** siguiendo los principios de **Arquitectura Limpia (*Clean / Onion Architecture*)**.

---

## 🎨 Características Principales

- **📦 Gestión de Productos**:
  - Registro de productos con nombre, descripción, código SKU, categoría y foto.
  - Registro del **Precio de Compra (Costo)** y **Precio de Venta Sugerido**.
  - Control de inventario en tiempo real (Stock disponible, estados: *Disponible*, *Vendido*, *Agotado*).
  - Cálculo instantáneo de ganancia estimada y margen de rentabilidad por unidad.

- **🛒 Registro Rápido de Ventas**:
  - Registro de ventas indicando fecha/hora y el precio real al que se vendió.
  - Cálculo automático de la ganancia neta por venta:
    $$\text{Ganancia} = (\text{Precio de Venta} - \text{Precio de Costo}) \times \text{Cantidad}$$
  - Descuento automático de stock en bodega.
  - Opción de anulación de venta que reintegra el stock automáticamente.

- **💸 Control de Gastos Operativos**:
  - Registro de salidas de dinero clasificadas por categoría (Alquiler, Servicios, Transporte/Fletes, Publicidad, Nómina, Suministros, etc.).

- **📊 Balance Financiero y Ganancia Neta Real**:
  - **Ingresos por Ventas**
  - **(-) Costo de Mercancía Vendida (COGS)**
  - **(=) Ganancia Bruta**
  - **(-) Gastos Operativos**
  - **(=) Ganancia Neta Real y % de Rentabilidad**
  - Filtros directos por período: **Hoy**, **Esta Semana**, **Este Mes**, **Este Año**, **Histórico Total** o **Rango Personalizado**.

- **📱 Diseño Optimizado para Celulares (*Mobile-First*)**:
  - Barra de navegación inferior (*Bottom Bar*) para navegación cómoda con una mano.
  - Botón de acceso rápido central para registrar ventas en un toque.
  - Paleta de colores cálida y elegante en tonos **Vino/Borgoña**, **Café/Mocha** y **Crema**, sin gráficos innecesarios para una carga ultrarrápida.

---

## 🏗️ Arquitectura del Proyecto

El proyecto está dividido en 5 capas desacopladas:

```plaintext
AppTienda/
│── AppTienda.sln                          # Solución principal para Visual Studio
│── .vscode/                               # Configuración para depuración en VS Code
│
├── Core/
│   ├── AppTienda.Core.Domain/             # Entidades del negocio, Enums y Contratos de Repositorios
│   │   ├── Common/                        # BaseEntity
│   │   ├── Entities/                      # Product, Category, Sale, Expense
│   │   ├── Enums/                         # ProductStatus, PaymentMethod, ExpenseCategory
│   │   └── Interfaces/                    # IGenericRepository, IProductRepository, etc.
│   │
│   └── AppTienda.Core.Application/        # Lógica de aplicación, Servicios, DTOs y Mapeos
│       ├── Dtos/                          # ProductDto, SaleDto, ExpenseDto, FinanceSummaryDto...
│       ├── Interfaces/                    # IProductService, ISaleService, IFinanceService...
│       ├── Services/                      # Implementación de lógica de negocio y cálculos
│       ├── Mappings/                      # Perfiles de AutoMapper
│       └── ServicesRegistration.cs
│
├── Infrastructure/
│   ├── AppTienda.Infrastructure.Persistence/ # Acceso a datos con Entity Framework Core
│   │   ├── Contexts/                      # AppTiendaDbContext
│   │   ├── EntitiesConfigurations/        # Fluent API (relaciones, precisión decimal)
│   │   ├── Repositories/                  # GenericRepository y repositorios especializados
│   │   └── ServicesRegistration.cs        # Inyección de dependencias y Seed de datos iniciales
│   │
│   └── AppTienda.Infrastructure.Shared/   # Servicios compartidos
│       ├── Interfaces/                    # IUploadFileService
│       ├── Services/                      # Manejo de subida de imágenes a wwwroot
│       └── ServicesRegistration.cs
│
└── Presentation/
    └── AppTienda.App/                     # Aplicación Web ASP.NET Core MVC
        ├── Controllers/                   # HomeController, ProductController, SaleController...
        ├── Views/                         # Vistas Razor responsivas con tema Vino/Mocha
        ├── wwwroot/                       # Archivos estáticos y subida de imágenes
        ├── appsettings.json               # Configuración de base de datos y logging
        └── Program.cs                     # Configuración del pipeline HTTP y servicios
```

---

## 🚀 Requisitos Previos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) o superior.
- **Visual Studio 2022** (versión 17.12+) o **Visual Studio Code** con la extensión de C#.

---

## 💻 Instrucciones de Instalación y Ejecución

### Opción A: Desde Visual Studio
1. Abre **Visual Studio**.
2. Haz clic en **Abrir un proyecto o una solución** y selecciona el archivo:
   ```
   AppTienda.sln
   ```
3. Verifica que el proyecto de inicio seleccionado sea `Presentation\AppTienda.App`.
4. Presiona **F5** o haz clic en **Iniciar Depuración**.

---

### Opción B: Desde VS Code o Terminal
1. Abre una terminal en la carpeta raíz del proyecto (`C:\Users\yaili\Desktop\AppTienda`).
2. Restaura los paquetes y compila la solución:
   ```bash
   dotnet restore
   dotnet build
   ```
3. Ejecuta la aplicación:
   ```bash
   dotnet run --project Presentation/AppTienda.App
   ```
4. Abre tu navegador web en:
   ```
   http://localhost:5282
   ```

---

## 📲 Cómo Usar la App desde tu Celular

Puedes abrir y usar la aplicación directamente desde el navegador de tu teléfono móvil conectado a la misma red Wi-Fi:

1. Conecta tu celular a la misma red **Wi-Fi** que tu computadora.
2. Inicia la aplicación en tu computadora con este comando:
   ```bash
   dotnet run --project Presentation/AppTienda.App --urls "http://0.0.0.0:5282"
   ```
3. Conoce la dirección IP local de tu computadora ejecutando `ipconfig` en la terminal (ejemplo: `192.168.0.104`).
4. En el navegador de tu celular (Chrome, Safari, etc.), entra a:
   ```
   http://TU_IP_LOCAL:5282
   ```
   *(Ejemplo: `http://192.168.0.104:5282`)*

---

## 🗄️ Base de Datos

La aplicación viene configurada por defecto con **SQLite** (`AppTienda.db`), lo que permite que funcione de inmediato sin necesidad de instalar ni configurar servidores de bases de datos.

Si deseas utilizar **SQL Server**, puedes cambiar la configuración en `Presentation/AppTienda.App/appsettings.json`:

```json
{
  "DatabaseProvider": "SqlServer",
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=AppTiendaDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

---

## 🛠️ Tecnologías Utilizadas

- **C# / .NET 9**
- **ASP.NET Core MVC**
- **Entity Framework Core 9** (SQLite / SQL Server / InMemory)
- **AutoMapper 14**
- **Bootstrap 5.3 & Bootstrap Icons**
- **HTML5 & CSS3** (Mobile-first responsive design)

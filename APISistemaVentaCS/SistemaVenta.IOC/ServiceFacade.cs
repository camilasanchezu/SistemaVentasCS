using SistemaVenta.BLL.Servicios.Contrato;

namespace SistemaVenta.IOC
{
    public  class ServiceFacade
    {


        public IRolService RolService { get; }
        public IUsuarioService UsuarioService { get; }
        public ICategoriaService CategoriaService { get; }
        public IProductoService ProductoService { get; }
        public IVentaService VentaService { get; }
        public IMenuService MenuService { get; }
        public IDashBoardService DashBoardService { get; }

        public ServiceFacade(
            IRolService rolService,
            IUsuarioService usuarioService,
            ICategoriaService categoriaService,
            IProductoService productoService,
            IVentaService ventaService,
            IMenuService menuService,
            IDashBoardService dashBoardService)
        {
            RolService = rolService;
            UsuarioService = usuarioService;
            CategoriaService = categoriaService;
            ProductoService = productoService;
            VentaService = ventaService;
            MenuService = menuService;
            DashBoardService = dashBoardService;
        }

    }
}
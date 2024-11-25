﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SistemaVenta.DAL.DBContext;

#nullable disable

namespace SistemaVenta.DAL.Migrations
{
    [DbContext(typeof(DbventaContext))]
    partial class DbventaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SistemaVenta.Model.Categoria", b =>
                {
                    b.Property<int>("IdCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idCategoria");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCategoria"));

                    b.Property<bool?>("EsActivo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true)
                        .HasColumnName("esActivo");

                    b.Property<DateTime?>("FechaRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fechaRegistro")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Nombre")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre");

                    b.HasKey("IdCategoria")
                        .HasName("PK__Categori__8A3D240CABEF0591");

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("SistemaVenta.Model.DetalleVenta", b =>
                {
                    b.Property<int>("IdDetalleVenta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idDetalleVenta");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDetalleVenta"));

                    b.Property<int?>("Cantidad")
                        .HasColumnType("int")
                        .HasColumnName("cantidad");

                    b.Property<int?>("IdProducto")
                        .HasColumnType("int")
                        .HasColumnName("idProducto");

                    b.Property<int?>("IdVenta")
                        .HasColumnType("int")
                        .HasColumnName("idVenta");

                    b.Property<decimal?>("Precio")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("precio");

                    b.Property<decimal?>("Total")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("total");

                    b.HasKey("IdDetalleVenta")
                        .HasName("PK__DetalleV__BFE2843FC1BBF9E8");

                    b.HasIndex("IdProducto");

                    b.HasIndex("IdVenta");

                    b.ToTable("DetalleVenta");
                });

            modelBuilder.Entity("SistemaVenta.Model.Menu", b =>
                {
                    b.Property<int>("IdMenu")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idMenu");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdMenu"));

                    b.Property<string>("Icono")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("icono");

                    b.Property<string>("Nombre")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre");

                    b.Property<string>("Url")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("url");

                    b.HasKey("IdMenu")
                        .HasName("PK__Menu__C26AF4836ED744EB");

                    b.ToTable("Menu", (string)null);
                });

            modelBuilder.Entity("SistemaVenta.Model.MenuRol", b =>
                {
                    b.Property<int>("IdMenuRol")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idMenuRol");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdMenuRol"));

                    b.Property<int?>("IdMenu")
                        .HasColumnType("int")
                        .HasColumnName("idMenu");

                    b.Property<int?>("IdRol")
                        .HasColumnType("int")
                        .HasColumnName("idRol");

                    b.HasKey("IdMenuRol")
                        .HasName("PK__MenuRol__9D6D61A4D7B1296A");

                    b.HasIndex("IdMenu");

                    b.HasIndex("IdRol");

                    b.ToTable("MenuRol", (string)null);
                });

            modelBuilder.Entity("SistemaVenta.Model.NumeroDocumento", b =>
                {
                    b.Property<int>("IdNumeroDocumento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idNumeroDocumento");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdNumeroDocumento"));

                    b.Property<DateTime?>("FechaRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fechaRegistro")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("UltimoNumero")
                        .HasColumnType("int")
                        .HasColumnName("ultimo_Numero");

                    b.HasKey("IdNumeroDocumento")
                        .HasName("PK__NumeroDo__471E421AD9FF5EB3");

                    b.ToTable("NumeroDocumento", (string)null);
                });

            modelBuilder.Entity("SistemaVenta.Model.Producto", b =>
                {
                    b.Property<int>("IdProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idProducto");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProducto"));

                    b.Property<bool?>("EsActivo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true)
                        .HasColumnName("esActivo");

                    b.Property<DateTime?>("FechaRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fechaRegistro")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int?>("IdCategoria")
                        .HasColumnType("int")
                        .HasColumnName("idCategoria");

                    b.Property<string>("Nombre")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("nombre");

                    b.Property<decimal?>("Precio")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("precio");

                    b.Property<int?>("Stock")
                        .HasColumnType("int")
                        .HasColumnName("stock");

                    b.HasKey("IdProducto")
                        .HasName("PK__Producto__07F4A1327575D7F2");

                    b.HasIndex("IdCategoria");

                    b.ToTable("Producto", (string)null);
                });

            modelBuilder.Entity("SistemaVenta.Model.Rol", b =>
                {
                    b.Property<int>("IdRol")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idRol");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRol"));

                    b.Property<DateTime?>("FechaRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fechaRegistro")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Nombre")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre");

                    b.HasKey("IdRol")
                        .HasName("PK__Rol__3C872F7626E53020");

                    b.ToTable("Rol", (string)null);
                });

            modelBuilder.Entity("SistemaVenta.Model.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idUsuario");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUsuario"));

                    b.Property<string>("Clave")
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)")
                        .HasColumnName("clave");

                    b.Property<string>("Correo")
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)")
                        .HasColumnName("correo");

                    b.Property<bool?>("EsActivo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true)
                        .HasColumnName("esActivo");

                    b.Property<DateTime?>("FechaRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fechaRegistro")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int?>("IdRol")
                        .HasColumnType("int")
                        .HasColumnName("idRol");

                    b.Property<string>("NombreCompleto")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("nombreCompleto");

                    b.HasKey("IdUsuario")
                        .HasName("PK__Usuario__645723A6143254DE");

                    b.HasIndex("IdRol");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("SistemaVenta.Model.Venta", b =>
                {
                    b.Property<int>("IdVenta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idVenta");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdVenta"));

                    b.Property<DateTime?>("FechaRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fechaRegistro")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("NumeroDocumento")
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)")
                        .HasColumnName("numeroDocumento");

                    b.Property<string>("TipoPago")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("tipoPago");

                    b.Property<decimal?>("Total")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("total");

                    b.HasKey("IdVenta")
                        .HasName("PK__Venta__077D56149D5270C1");

                    b.ToTable("Venta");
                });

            modelBuilder.Entity("SistemaVenta.Model.DetalleVenta", b =>
                {
                    b.HasOne("SistemaVenta.Model.Producto", "IdProductoNavigation")
                        .WithMany("DetalleVenta")
                        .HasForeignKey("IdProducto")
                        .HasConstraintName("FK__DetalleVe__idPro__66603565");

                    b.HasOne("SistemaVenta.Model.Venta", "IdVentaNavigation")
                        .WithMany("DetalleVenta")
                        .HasForeignKey("IdVenta")
                        .HasConstraintName("FK__DetalleVe__idVen__656C112C");

                    b.Navigation("IdProductoNavigation");

                    b.Navigation("IdVentaNavigation");
                });

            modelBuilder.Entity("SistemaVenta.Model.MenuRol", b =>
                {
                    b.HasOne("SistemaVenta.Model.Menu", "IdMenuNavigation")
                        .WithMany("MenuRols")
                        .HasForeignKey("IdMenu")
                        .HasConstraintName("FK__MenuRol__idMenu__4E88ABD4");

                    b.HasOne("SistemaVenta.Model.Rol", "IdRolNavigation")
                        .WithMany("MenuRols")
                        .HasForeignKey("IdRol")
                        .HasConstraintName("FK__MenuRol__idRol__4F7CD00D");

                    b.Navigation("IdMenuNavigation");

                    b.Navigation("IdRolNavigation");
                });

            modelBuilder.Entity("SistemaVenta.Model.Producto", b =>
                {
                    b.HasOne("SistemaVenta.Model.Categoria", "IdCategoriaNavigation")
                        .WithMany("Productos")
                        .HasForeignKey("IdCategoria")
                        .HasConstraintName("FK__Producto__idCate__5AEE82B9");

                    b.Navigation("IdCategoriaNavigation");
                });

            modelBuilder.Entity("SistemaVenta.Model.Usuario", b =>
                {
                    b.HasOne("SistemaVenta.Model.Rol", "IdRolNavigation")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdRol")
                        .HasConstraintName("FK__Usuario__idRol__52593CB8");

                    b.Navigation("IdRolNavigation");
                });

            modelBuilder.Entity("SistemaVenta.Model.Categoria", b =>
                {
                    b.Navigation("Productos");
                });

            modelBuilder.Entity("SistemaVenta.Model.Menu", b =>
                {
                    b.Navigation("MenuRols");
                });

            modelBuilder.Entity("SistemaVenta.Model.Producto", b =>
                {
                    b.Navigation("DetalleVenta");
                });

            modelBuilder.Entity("SistemaVenta.Model.Rol", b =>
                {
                    b.Navigation("MenuRols");

                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("SistemaVenta.Model.Venta", b =>
                {
                    b.Navigation("DetalleVenta");
                });
#pragma warning restore 612, 618
        }
    }
}

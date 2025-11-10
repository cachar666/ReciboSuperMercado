using ReciboSuperMercado.Dominio;
using FluentAssertions;
using Xunit;

namespace ReciboSuperMercado.Test;

public class Test_CatalogoProductos
{
    [Fact]
    public void AgregarProducto_DeberiaIncrementarCantidadDeProductos()
    {
        var catalogo = new Catalogo();
        var producto = new Producto("Manzana");

        catalogo.AgregarProducto(producto, 3.5m);

        catalogo.CantidadProductos().Should().Be(1);
    }

    [Fact]
    public void NoPermitirProductosDuplicados_PorDescripcion()
    {
        var catalogo = new Catalogo();
        var p1 = new Producto("Manzana");
        var p2 = new Producto("Manzana");

        catalogo.AgregarProducto(p1, 3.5m);

        Action act = () => catalogo.AgregarProducto(p2, 3.8m);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("El producto 'Manzana' ya existe en el catálogo.");
        catalogo.CantidadProductos().Should().Be(1);
    }

    [Fact]
    public void ObtenerPrecio_DeberiaRetornarPrecioRegistrado()
    {
        var catalogo = new Catalogo();
        var producto = new Producto("Banano");

        catalogo.AgregarProducto(producto, 4.2m);

        var precio = catalogo.ObtenerPrecio(producto);

        precio.Should().Be(4.2m);
    }

    [Fact]
    public void ObtenerPrecio_DeberiaLanzarExcepcion_SiProductoNoExiste()
    {
        // Arrange
        var catalogo = new Catalogo();
        var producto = new Producto("Pera");

        // Act
        Action act = () => catalogo.ObtenerPrecio(producto);

        // Assert
        act.Should().Throw<KeyNotFoundException>()
            .WithMessage("El producto 'Pera' no existe en el catálogo.");
    }
}
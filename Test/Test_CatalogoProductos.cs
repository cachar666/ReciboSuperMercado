using ReciboSuperMercado.Dominio;
using FluentAssertions;
using Xunit;

namespace ReciboSuperMercado;

public class Test_CatalogoProductos
{
    [Fact]
    public void AgregarProducto_DeberiaIncrementarCantidadDeProductos()
    {
        // Arrange
        var catalogo = new Catalogo();
        var producto = new Producto("Manzana");

        // Act
        catalogo.AgregarProducto(producto, 3.5m);

        // Assert
        catalogo.CantidadProductos().Should().Be(1);
    }
    [Fact]
    public void NoPermitirProductosDuplicados_PorDescripcion()
    {
        // Arrange
        var catalogo = new Catalogo();
        var p1 = new Producto("Manzana");
        var p2 = new Producto("Manzana"); // mismo nombre, diferente instancia

        catalogo.AgregarProducto(p1, 3.5m);

        // Act
        Action act = () => catalogo.AgregarProducto(p2, 3.8m);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("El producto 'Manzana' ya existe en el catálogo.");
        catalogo.CantidadProductos().Should().Be(1);
    }
    [Fact]
    public void ObtenerPrecio_DeberiaRetornarPrecioRegistrado()
    {
        // Arrange
        var catalogo = new Catalogo();
        var producto = new Producto("Banano");

        catalogo.AgregarProducto(producto, 4.2m);

        // Act
        var precio = catalogo.ObtenerPrecio(producto);

        // Assert
        precio.Should().Be(4.2m);
    }
}
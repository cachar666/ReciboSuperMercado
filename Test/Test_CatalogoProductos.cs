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
}
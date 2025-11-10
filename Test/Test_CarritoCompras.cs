using ReciboSuperMercado.Dominio;
using FluentAssertions;
using Xunit;

namespace ReciboSuperMercado.Test;

public class Test_CarritoCompras
{
    [Fact]
    public void SiCarritoVacio_Debe_Total_0()
    {
        // Arrange
        var carrito = new CarritoDeCompras();

        // Act
        var total = carrito.Total();

        // Assert
        total.Should().Be(0m);
    }
    [Fact]
    public void AgregarUnProducto_DeberiaCalcularTotalCorrectamente()
    {
        // Arrange
        var catalogo = new Catalogo();
        var producto = new Producto("Manzana");
        catalogo.AgregarProducto(producto, 3.5m);

        var carrito = new CarritoDeCompras(catalogo);

        // Act
        carrito.AgregarProducto(producto, 2); // 2 unidades * $3.5 = $7.0
        var total = carrito.Total();

        // Assert
        total.Should().Be(7.0m);
    }
    
    [Fact]
    public void AgregarMultiplesProductos_DeberiaSumarTotalCorrectamente()
    {
        // Arrange
        var catalogo = new Catalogo();
        var manzana = new Producto("Manzana");
        var pera = new Producto("Pera");

        catalogo.AgregarProducto(manzana, 3.5m);
        catalogo.AgregarProducto(pera, 4.0m);

        var carrito = new CarritoDeCompras(catalogo);

        // Act
        carrito.AgregarProducto(manzana, 2); // 7.0
        carrito.AgregarProducto(pera, 3);    // 12.0
        var total = carrito.Total();         // 19.0

        // Assert
        total.Should().Be(19.0m);
    }
}
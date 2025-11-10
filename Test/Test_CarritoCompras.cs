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
}
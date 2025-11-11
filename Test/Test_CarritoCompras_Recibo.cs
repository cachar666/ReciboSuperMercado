using ReciboSuperMercado.Dominio;
using FluentAssertions;
using Xunit;

namespace ReciboSuperMercado.Test;

public class Test_CarritoCompras_Recibo
{
    [Fact]
    public void ImprimirRecibo_DeberiaListarProductosCompradosConSubtotal()
    {
        // Arrange
        var catalogo = new Catalogo();
        var manzana = new Producto("Manzana");
        var pera = new Producto("Pera");
        catalogo.AgregarProducto(manzana, 2.0m);
        catalogo.AgregarProducto(pera, 3.0m);

        var carrito = new CarritoDeCompras(catalogo);

        carrito.AgregarProducto(manzana, 2m); // subtotal 4.0
        carrito.AgregarProducto(pera, 1m);    // subtotal 3.0

        // Act
        var recibo = carrito.ImprimirRecibo();

        // Assert
        recibo.Should().Contain("Manzana x2 -> $4.00");
        recibo.Should().Contain("Pera x1 -> $3.00");
    }
}
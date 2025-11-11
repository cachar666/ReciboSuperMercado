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
    
    [Fact]
    public void ImprimirRecibo_DeberiaListarDescuentosAplicados()
    {
        // Arrange
        var catalogo = new Catalogo();
        var cepillo = new Producto("Cepillo de dientes");
        catalogo.AgregarProducto(cepillo, 0.99m);

        var ofertas = new CatalogoOfertas();
        ofertas.RegistrarOferta(cepillo, compra: 3m, lleve: 2m, descripcion: "3x2");

        var carrito = new CarritoDeCompras(catalogo, ofertas);
        carrito.AgregarProducto(cepillo, 3m);

        // Act
        var recibo = carrito.ImprimirRecibo();

        // Assert
        recibo.Should().Contain("Cepillo de dientes x3 -> $2.97");
        recibo.Should().Contain("Descuento 3x2: -$0.99");
    }
    [Fact]
    public void ImprimirReciboCompleto_DeberiaIncluirTresTiposDeDescuentosYTotales()
    {
        // Arrange
        var catalogo = new Catalogo();
        var cepillo = new Producto("Cepillo de dientes");
        var shampoo = new Producto("Shampoo");
        var arroz = new Producto("Arroz");

        catalogo.AgregarProducto(cepillo, 0.99m);
        catalogo.AgregarProducto(shampoo, 10.00m);
        catalogo.AgregarProducto(arroz, 5.00m);

        var ofertas = new CatalogoOfertas();
        ofertas.RegistrarOferta(cepillo, 3m, 2m, "3x2");
        ofertas.RegistrarDescuentoPorcentual(shampoo, 10m, "10% off");
        ofertas.RegistrarDescuentoPorMayor(arroz, 10m, 45.00m, "10x45 Mayorista");

        var carrito = new CarritoDeCompras(catalogo, ofertas);

        carrito.AgregarProducto(cepillo, 3m);  // subtotal 2.97 ; desc 0.99
        carrito.AgregarProducto(shampoo, 2m);  // subtotal 20.00 ; desc 2.00
        carrito.AgregarProducto(arroz, 10m);   // subtotal 50.00 ; desc 5.00

        // Act
        var recibo = carrito.ImprimirRecibo();

        // Assert
        recibo.Should().Contain("Cepillo de dientes x3 -> $2.97");
        recibo.Should().Contain("Descuento 3x2: -$0.99");
        recibo.Should().Contain("Shampoo x2 -> $20.00");
        recibo.Should().Contain("Descuento 10% off: -$2.00");
        recibo.Should().Contain("Arroz x10 -> $50.00");
        recibo.Should().Contain("Descuento 10x45 Mayorista: -$5.00");
        recibo.Should().Contain("Subtotal: $72.97");
        recibo.Should().Contain("Total descuentos: -$7.99");
        recibo.Should().Contain("Total a pagar: $64.98");
    }
}
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
}
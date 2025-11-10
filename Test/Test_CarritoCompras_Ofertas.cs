using ReciboSuperMercado.Dominio;
using FluentAssertions;
using Xunit;

namespace ReciboSuperMercado.Test;

public class Test_CarritoCompras_Ofertas
{
    [Fact]
    public void CarritoConOfertas_DeberiaAplicarDescuentosYCalcularTotal()
    {
        // Arrange
        var catalogo = new Catalogo();
        var cepillo = new Producto("Cepillo de dientes");
        var crema   = new Producto("Crema dental");

        catalogo.AgregarProducto(cepillo, 0.99m);
        catalogo.AgregarProducto(crema, 1.49m);

        var ofertas = new CatalogoOfertas();
        ofertas.RegistrarOferta(cepillo, compra: 3m, lleve: 2m, descripcion: "3x2");
        ofertas.RegistrarOferta(crema,   compra: 2m, lleve: 1m, descripcion: "2x1");

        var carrito = new CarritoDeCompras(catalogo, ofertas);

        // Act
        carrito.AgregarProducto(cepillo, 3m); // subtotal = 3 * 0.99 = 2.97 ; desc = 0.99
        carrito.AgregarProducto(crema,   2m); // subtotal = 2 * 1.49 = 2.98 ; desc = 1.49
        var total = carrito.Total();          // total = 2.97 + 2.98 - (0.99 + 1.49) = 3.47

        // Assert
        total.Should().Be(3.47m);
    }
    
    [Fact]
    public void CarritoConOfertasCombinadas_DeberiaAplicarDescuentosCorrectos()
    {
        // Arrange
        var catalogo = new Catalogo();
        var cepillo = new Producto("Cepillo de dientes");
        var shampoo = new Producto("Shampoo");

        catalogo.AgregarProducto(cepillo, 0.99m);
        catalogo.AgregarProducto(shampoo, 10.0m);

        var ofertas = new CatalogoOfertas();
        ofertas.RegistrarOferta(cepillo, compra: 3m, lleve: 2m, descripcion: "3x2");
        ofertas.RegistrarDescuentoPorcentual(shampoo, porcentaje: 10m, descripcion: "10% de descuento");

        var carrito = new CarritoDeCompras(catalogo, ofertas);

        // Act
        carrito.AgregarProducto(cepillo, 3m); // subtotal = 2.97 ; descuento = 0.99
        carrito.AgregarProducto(shampoo, 2m); // subtotal = 20.00 ; descuento = 2.00
        var total = carrito.Total();          // total esperado = 2.97 + 20 - (0.99 + 2.00) = 19.98

        // Assert
        total.Should().Be(19.98m);
    }
}
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
        carrito.AgregarProducto(producto, 2m); // 2 unidades * $3.5 = $7.0
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
        carrito.AgregarProducto(manzana, 2m); // 7.0
        carrito.AgregarProducto(pera, 3m);    // 12.0
        var total = carrito.Total();         // 19.0

        // Assert
        total.Should().Be(19.0m);
    }
    
    [Fact]
    public void AgregarProductoPorPeso_DeberiaCalcularTotalConCantidadDecimal()
    {
        // Arrange
        var catalogo = new Catalogo();
        var banano = new Producto("Banano");
        catalogo.AgregarProducto(banano, 4.0m); // $4 por "unidad de medida" (ej. kg)

        var carrito = new CarritoDeCompras(catalogo);

        // Act
        carrito.AgregarProducto(banano, 1.25m); // 1.25 * 4.0 = 5.0
        var total = carrito.Total();

        // Assert
        total.Should().Be(5.0m);
    }
    
    // OFERTAS PAGUE LLEVE
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
}
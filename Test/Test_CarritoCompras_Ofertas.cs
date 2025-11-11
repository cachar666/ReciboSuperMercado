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
    
    [Fact]
    public void CarritoConOfertas_Combinadas_3x2_Porcentaje_Mayorista()
    {
        // Arrange
        var catalogo = new Catalogo();
        var cepillo = new Producto("Cepillo de dientes");
        var shampoo = new Producto("Shampoo");
        var arroz   = new Producto("Arroz");

        catalogo.AgregarProducto(cepillo, 0.99m);
        catalogo.AgregarProducto(shampoo, 10.00m);
        catalogo.AgregarProducto(arroz,   5.00m);

        var ofertas = new CatalogoOfertas();
        ofertas.RegistrarOferta(cepillo, compra: 3m,  lleve: 2m,  descripcion: "3x2");
        ofertas.RegistrarDescuentoPorcentual(shampoo, porcentaje: 10m, descripcion: "10% off");
        // Mayorista como valor total del paquete: 10 unidades por $45
        ofertas.RegistrarDescuentoPorMayor(arroz, cantidadMinima: 10m, valorPromoTotal: 45.00m, descripcion: "10x45 Mayorista");

        var carrito = new CarritoDeCompras(catalogo, ofertas);

        // Act
        carrito.AgregarProducto(cepillo, 3m);  // Subtotal 2.97 ; Descuento 0.99
        carrito.AgregarProducto(shampoo, 2m);  // Subtotal 20.00 ; Descuento 2.00
        carrito.AgregarProducto(arroz,   10m); // Subtotal 50.00 ; Descuento 5.00

        var total = carrito.Total();           // 72.97 - (0.99 + 2.00 + 5.00) = 64.98

        // Assert
        total.Should().Be(64.98m);
    }
    
    [Fact]
    public void CarritoCon4Cepillos_Oferta3x2_DeberiaCobrar3Unidades()
    {
        // Arrange
        var catalogo = new Catalogo();
        var cepillo = new Producto("Cepillo de dientes");
        catalogo.AgregarProducto(cepillo, 0.99m);

        var ofertas = new CatalogoOfertas();
        ofertas.RegistrarOferta(cepillo, compra: 3m, lleve: 2m, descripcion: "3x2");

        var carrito = new CarritoDeCompras(catalogo, ofertas);

        // Act
        carrito.AgregarProducto(cepillo, 4m);
        var total = carrito.Total();

        // Assert
        // Compra 4 cepillos → 1 grupo 3x2 (1 gratis) + 1 sin descuento.
        // Subtotal: 4 * 0.99 = 3.96 ; Descuento: 0.99 ; Total esperado: 2.97 + 0.99 = 3.96 - 0.99 = 2.97 + 0.99? no; correct is 3.96 - 0.99 = 2.97 + 0.99? Wait.
        // Correction: paga 3 * 0.99 = 2.97.
        total.Should().Be(2.97m);
    }
    
    [Fact]
    public void CarritoCon7Cepillos_Oferta3x2_DeberiaCobrar5Unidades()
    {
        // Arrange
        var catalogo = new Catalogo();
        var cepillo = new Producto("Cepillo de dientes");
        catalogo.AgregarProducto(cepillo, 0.99m);

        var ofertas = new CatalogoOfertas();
        ofertas.RegistrarOferta(cepillo, compra: 3m, lleve: 2m, descripcion: "3x2");

        var carrito = new CarritoDeCompras(catalogo, ofertas);

        // Act
        carrito.AgregarProducto(cepillo, 7m); // 7 comprados → 2 grupos (2 gratis) → se pagan 5
        var total = carrito.Total();

        // Assert
        // Subtotal: 7 * 0.99 = 6.93 ; Descuento: 2 * 0.99 = 1.98 ; Total: 6.93 - 1.98 = 4.95
        total.Should().Be(4.95m);
    }
}
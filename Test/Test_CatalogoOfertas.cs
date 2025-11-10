using ReciboSuperMercado.Dominio;
using FluentAssertions;
using Xunit;

namespace ReciboSuperMercado.Test;

public class Test_CatalogoOfertas
{
    [Fact]
    public void RegistrarOferta_3x2_DeberiaAplicarDescuentoCorrecto()
    {
        var catalogo = new Catalogo();
        var cepillo = new Producto("Cepillo de dientes");
        catalogo.AgregarProducto(cepillo, 0.99m);

        var ofertas = new CatalogoOfertas();
        ofertas.RegistrarOferta(cepillo, compra: 3m, lleve: 2m, descripcion: "3x2");

        var descuento = ofertas.CalcularDescuento(cepillo, cantidadComprada: 3m, catalogo);

        descuento.ValorDescontado.Should().Be(0.99m);
        descuento.Descripcion.Should().Be("3x2");
        descuento.Producto.Should().Be(cepillo);
    }

    [Fact]
    public void MultiplesOfertas_DeberianAplicarDescuentoCorrectoPorCadaProducto()
    {
        // Arrange
        var catalogo = new Catalogo();

        var cepillo = new Producto("Cepillo de dientes");
        var crema = new Producto("Crema dental");

        catalogo.AgregarProducto(cepillo, 0.99m);
        catalogo.AgregarProducto(crema, 1.49m);

        var ofertas = new CatalogoOfertas();
        ofertas.RegistrarOferta(cepillo, compra: 3m, lleve: 2m, descripcion: "3x2");
        ofertas.RegistrarOferta(crema, compra: 2m, lleve: 1m, descripcion: "2x1");

        // Act
        var descuentoCepillo = ofertas.CalcularDescuento(cepillo, cantidadComprada: 3m, catalogo);
        var descuentoCrema = ofertas.CalcularDescuento(crema, cantidadComprada: 2m, catalogo);

        // Assert
        descuentoCepillo.ValorDescontado.Should().Be(0.99m);
        descuentoCepillo.Descripcion.Should().Be("3x2");
        descuentoCepillo.Producto.Should().Be(cepillo);

        descuentoCrema.ValorDescontado.Should().Be(1.49m);
        descuentoCrema.Descripcion.Should().Be("2x1");
        descuentoCrema.Producto.Should().Be(crema);
    }
}
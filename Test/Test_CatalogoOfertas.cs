using ReciboSuperMercado.Dominio;
using FluentAssertions;
using Xunit;

namespace ReciboSuperMercado.Test;

public class Test_CatalogoOfertas
{
    [Fact]
    public void RegistrarOferta_3x2_DeberiaAplicarDescuentoCorrecto()
    {
        // Arrange
        var catalogo = new Catalogo();
        var cepillo = new Producto("Cepillo de dientes");
        catalogo.AgregarProducto(cepillo, 0.99m);

        var ofertas = new CatalogoOfertas();
        ofertas.RegistrarOferta(cepillo, compra: 3, lleve: 2, descripcion: "3x2");

        // Act
        var descuento = ofertas.CalcularDescuento(cepillo, cantidadComprada: 3, catalogo);

        // Assert
        descuento.ValorDescontado.Should().Be(0.99m);
        descuento.Descripcion.Should().Be("3x2");
        descuento.Producto.Should().Be(cepillo);
    }
}
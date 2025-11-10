using FluentAssertions;

namespace ReciboSuperMercado;

public class Test_CatalogoProductos
{
    [Fact]
    public void Test01_Si_AgregoProductosAlCatalogo_Debe_GuardarPreciosCorrectamente()
    {
        // Arrange
        var catalogo = new Catalogo();
        var manzanas = new Producto("Manzanas");
        var pan = new Producto("Pan");
        var leche = new Producto("Leche");
        
        // Act - A) Agregar productos
        catalogo.AgregarProducto(manzanas, 2.50m);
        catalogo.AgregarProducto(pan, 1.80m);
        catalogo.AgregarProducto(leche, 3.20m);
        
        // Assert - B) Verificar que se pueden consultar precios
        var precioManzanas = catalogo.ObtenerPrecio(manzanas);
        var precioPan = catalogo.ObtenerPrecio(pan);
        var precioLeche = catalogo.ObtenerPrecio(leche);
        
        precioManzanas.Should().Be(2.50m);
        precioPan.Should().Be(1.80m);
        precioLeche.Should().Be(3.20m);
        
        // C) Verificar que tenemos 3 productos
        catalogo.CantidadProductos().Should().Be(3);
    }
}
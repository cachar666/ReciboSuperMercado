using System.Collections.Generic;

namespace ReciboSuperMercado.Dominio;

public class CarritoDeCompras
{
    private readonly Catalogo _catalogo;
    private readonly CatalogoOfertas _ofertas;
    private readonly Dictionary<Producto, decimal> _items = new();

    public CarritoDeCompras(Catalogo catalogo, CatalogoOfertas ofertas)
    {
        _catalogo = catalogo;
        _ofertas  = ofertas;
    }

    public CarritoDeCompras(Catalogo catalogo) : this(catalogo, new CatalogoOfertas()) { }

    public CarritoDeCompras() : this(new Catalogo(), new CatalogoOfertas()) { }

    public void AgregarProducto(Producto producto, decimal cantidad)
    {
        if (_items.ContainsKey(producto))
            _items[producto] += cantidad;
        else
            _items[producto] = cantidad;
    }

    public decimal Total()
    {
        decimal subtotal = 0m;
        decimal descuentoTotal = 0m;

        foreach (var itemCarrito in _items)
        {
            var producto = itemCarrito.Key;
            var cantidad = itemCarrito.Value;

            var precioUnit = _catalogo.ObtenerPrecio(producto);
            subtotal += precioUnit * cantidad;

            var descuento = _ofertas.CalcularDescuento(producto, cantidad, _catalogo);
            descuentoTotal += descuento.ValorDescontado;
        }

        return subtotal - descuentoTotal;
    }
    
}
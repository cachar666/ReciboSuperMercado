using System.Collections.Generic;

namespace ReciboSuperMercado.Dominio;

public class CarritoDeCompras
{
    private readonly Catalogo _catalogo;
    private readonly Dictionary<Producto, decimal> _items = new();

    public CarritoDeCompras(Catalogo catalogo)
    {
        _catalogo = catalogo;
    }

    public CarritoDeCompras() : this(new Catalogo()) { }

    public void AgregarProducto(Producto producto, decimal cantidad)
    {
        if (_items.ContainsKey(producto))
            _items[producto] += cantidad;
        else
            _items[producto] = cantidad;
    }

    public decimal Total()
    {
        decimal total = 0m;

        foreach (var item in _items)
        {
            var precioUnitario = _catalogo.ObtenerPrecio(item.Key);
            total += precioUnitario * item.Value;
        }

        return total;
    }
}
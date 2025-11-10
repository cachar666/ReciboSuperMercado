using System.Collections.Generic;

namespace ReciboSuperMercado.Dominio;

public class CarritoDeCompras
{
    private readonly Catalogo _catalogo;
    private readonly Dictionary<Producto, int> _items = new();

    public CarritoDeCompras(Catalogo catalogo)
    {
        _catalogo = catalogo;
    }

    public CarritoDeCompras() : this(new Catalogo()) { }

    public void AgregarProducto(Producto producto, int cantidad)
    {
        // Si ya existe, sumamos cantidades
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
            var precio = _catalogo.ObtenerPrecio(item.Key);
            total += precio * item.Value;
        }

        return total;
    }
}
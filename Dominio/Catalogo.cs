using System.Collections.Generic;

namespace ReciboSuperMercado.Dominio;

public class Catalogo
{
    private readonly Dictionary<string, decimal> _precios = new();

    public void AgregarProducto(Producto producto, decimal precio)
    {
        if (_precios.ContainsKey(producto.Nombre))
            throw new InvalidOperationException($"El producto '{producto.Nombre}' ya existe en el catálogo.");

        _precios[producto.Nombre] = precio;
    }

    public decimal ObtenerPrecio(Producto producto)
    {
        return _precios[producto.Nombre];
    }

    public int CantidadProductos()
    {
        return _precios.Count;
    }
}
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
    public string ImprimirRecibo()
    {
        var lineas = new List<string>();
        decimal subtotal = 0m;
        decimal totalDescuentos = 0m;

        foreach (var itemCarrito in _items)
        {
            var producto = itemCarrito.Key;
            var cantidad = itemCarrito.Value;
            var precio = _catalogo.ObtenerPrecio(producto);
            var valor = precio * cantidad;
            subtotal += valor;

            lineas.Add($"{producto.Nombre} x{cantidad} -> ${valor:0.00}");

            var descuento = _ofertas.CalcularDescuento(producto, cantidad, _catalogo);
            if (descuento.ValorDescontado > 0)
            {
                totalDescuentos += descuento.ValorDescontado;
                lineas.Add($"Descuento {descuento.Descripcion}: -${descuento.ValorDescontado:0.00}");
            }
        }

        lineas.Add($"Subtotal: ${subtotal:0.00}");
        lineas.Add($"Total descuentos: -${totalDescuentos:0.00}");
        lineas.Add($"Total a pagar: ${(subtotal - totalDescuentos):0.00}");

        return string.Join(Environment.NewLine, lineas);
    }
}
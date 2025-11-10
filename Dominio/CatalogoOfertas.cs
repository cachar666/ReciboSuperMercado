using System.Collections.Generic;

namespace ReciboSuperMercado.Dominio;

public class CatalogoOfertas
{
    private readonly List<Oferta> _ofertas = new();
    private readonly List<DescuentoPorcentual> _descuentos = new();

    public void RegistrarOferta(Producto producto, decimal compra, decimal lleve, string descripcion)
    {
        _ofertas.Add(new Oferta(producto, compra, lleve, descripcion));
    }

    public void RegistrarDescuentoPorcentual(Producto producto, decimal porcentaje, string descripcion)
    {
        _descuentos.Add(new DescuentoPorcentual(producto, porcentaje, descripcion));
    }

    public Descuento CalcularDescuento(Producto producto, decimal cantidadComprada, Catalogo catalogo)
    {
        // Prioridad 1: buscar oferta 3x2, 2x1, etc.
        var oferta = _ofertas.Find(o => o.Producto.Equals(producto));
        if (oferta is not null)
        {
            var grupos = (int)(cantidadComprada / oferta.Compra);
            var unidadesGratis = grupos * (oferta.Compra - oferta.Lleve);
            var precioUnitario = catalogo.ObtenerPrecio(producto);
            var valorDescontado = unidadesGratis * precioUnitario;

            return new Descuento(producto, oferta.Descripcion, valorDescontado);
        }

        // Prioridad 2: buscar descuento porcentual
        var descuentoPct = _descuentos.Find(d => d.Producto.Equals(producto));
        if (descuentoPct is not null)
        {
            var precioUnitario = catalogo.ObtenerPrecio(producto);
            var subtotal = cantidadComprada * precioUnitario;
            var valorDescontado = subtotal * (descuentoPct.Porcentaje / 100m);

            return new Descuento(producto, descuentoPct.Descripcion, valorDescontado);
        }

        // Sin descuento
        return new Descuento(producto, "", 0m);
    }
}

public class Oferta
{
    public Producto Producto { get; }
    public decimal Compra { get; }
    public decimal Lleve { get; }
    public string Descripcion { get; }

    public Oferta(Producto producto, decimal compra, decimal lleve, string descripcion)
    {
        Producto = producto;
        Compra = compra;
        Lleve = lleve;
        Descripcion = descripcion;
    }
}

public class DescuentoPorcentual
{
    public Producto Producto { get; }
    public decimal Porcentaje { get; }
    public string Descripcion { get; }

    public DescuentoPorcentual(Producto producto, decimal porcentaje, string descripcion)
    {
        Producto = producto;
        Porcentaje = porcentaje;
        Descripcion = descripcion;
    }
}

public class Descuento
{
    public Producto Producto { get; }
    public string Descripcion { get; }
    public decimal ValorDescontado { get; }

    public Descuento(Producto producto, string descripcion, decimal valorDescontado)
    {
        Producto = producto;
        Descripcion = descripcion;
        ValorDescontado = valorDescontado;
    }
}

using System.Collections.Generic;

namespace ReciboSuperMercado.Dominio;

public class CatalogoOfertas
{
    private readonly List<Oferta> _ofertas = new();

    public void RegistrarOferta(Producto producto, decimal compra, decimal lleve, string descripcion)
    {
        _ofertas.Add(new Oferta(producto, compra, lleve, descripcion));
    }

    public Descuento CalcularDescuento(Producto producto, decimal cantidadComprada, Catalogo catalogo)
    {
        var oferta = _ofertas.Find(o => o.Producto.Equals(producto));
        if (oferta is null)
            return new Descuento(producto, "", 0m);

        var grupos = (int)(cantidadComprada / oferta.Compra);
        var unidadesGratis = grupos * (oferta.Compra - oferta.Lleve);
        var precioUnitario = catalogo.ObtenerPrecio(producto);
        var valorDescontado = unidadesGratis * precioUnitario;

        return new Descuento(producto, oferta.Descripcion, valorDescontado);
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
using System.Collections.Generic;

namespace ReciboSuperMercado.Dominio;

public class CatalogoOfertas
{
    private readonly List<Oferta> _ofertas = new();                     // 3x2, 2x1
    private readonly List<DescuentoPorcentual> _descuentos = new();     // %
    private readonly List<DescuentoPorMayor> _mayoristas = new();       // mayorista

    public void RegistrarOferta(Producto producto, decimal compra, decimal lleve, string descripcion)
        => _ofertas.Add(new Oferta(producto, compra, lleve, descripcion));

    public void RegistrarDescuentoPorcentual(Producto producto, decimal porcentaje, string descripcion)
        => _descuentos.Add(new DescuentoPorcentual(producto, porcentaje, descripcion));

    public void RegistrarDescuentoPorMayor(Producto producto, decimal cantidadMinima, decimal valorPromoTotal, string descripcion)
        => _mayoristas.Add(new DescuentoPorMayor(producto, cantidadMinima, valorPromoTotal, descripcion));

    public Descuento CalcularDescuento(Producto producto, decimal cantidadComprada, Catalogo catalogo)
    {
        // 1) Oferta tipo 3x2
        var oferta = _ofertas.Find(o => o.Producto.Equals(producto));
        if (oferta is not null)
        {
            var grupos = (int)(cantidadComprada / oferta.Compra);
            var unidadesGratis = grupos * (oferta.Compra - oferta.Lleve);
            var precioUnitario = catalogo.ObtenerPrecio(producto);
            var valorDescontado = unidadesGratis * precioUnitario;
            return new Descuento(producto, oferta.Descripcion, valorDescontado);
        }

        // 2) Descuento al por mayor (valor total de la promo)
        var may = _mayoristas.Find(m => m.Producto.Equals(producto));
        if (may is not null && cantidadComprada >= may.CantidadMinima)
        {
            var precioNormal = catalogo.ObtenerPrecio(producto) * cantidadComprada;
            var valorDescontado = precioNormal - may.ValorPromoTotal;
            if (valorDescontado < 0) valorDescontado = 0m;
            return new Descuento(producto, may.Descripcion, valorDescontado);
        }

        // 3) Descuento porcentual
        var pct = _descuentos.Find(d => d.Producto.Equals(producto));
        if (pct is not null)
        {
            var precioUnitario = catalogo.ObtenerPrecio(producto);
            var subtotal = cantidadComprada * precioUnitario;
            var valorDescontado = subtotal * (pct.Porcentaje / 100m);
            return new Descuento(producto, pct.Descripcion, valorDescontado);
        }

        // Sin descuento
        return new Descuento(producto, "", 0m);
    }
}

// ────────────────────────────────────────────────────────────────────────────────
// Modelos
// ────────────────────────────────────────────────────────────────────────────────

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

public class DescuentoPorMayor
{
    public Producto Producto { get; }
    public decimal CantidadMinima { get; }
    public decimal ValorPromoTotal { get; }
    public string Descripcion { get; }

    public DescuentoPorMayor(Producto producto, decimal cantidadMinima, decimal valorPromoTotal, string descripcion)
    {
        Producto = producto;
        CantidadMinima = cantidadMinima;
        ValorPromoTotal = valorPromoTotal;
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

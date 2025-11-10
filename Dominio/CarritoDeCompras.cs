namespace ReciboSuperMercado.Dominio;

public class CarritoDeCompras
{
    private readonly Catalogo _catalogo;
    private Producto? _producto;
    private int _cantidad;

    public CarritoDeCompras(Catalogo catalogo)
    {
        _catalogo = catalogo;
    }

    public CarritoDeCompras() : this(new Catalogo()) { }

    public void AgregarProducto(Producto producto, int cantidad)
    {
        _producto = producto;
        _cantidad = cantidad;
    }

    public decimal Total()
    {
        if (_producto == null)
            return 0m;

        var precio = _catalogo.ObtenerPrecio(_producto);
        return precio * _cantidad;
    }
}
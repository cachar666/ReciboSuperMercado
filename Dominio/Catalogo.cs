namespace ReciboSuperMercado.Dominio;

public class Catalogo
{
    private readonly Dictionary<Producto, decimal> _precios = new();

    public void AgregarProducto(Producto producto, decimal precio)
    {
        _precios[producto] = precio;
    }

    public int CantidadProductos()
    {
        return _precios.Count;
    }
}
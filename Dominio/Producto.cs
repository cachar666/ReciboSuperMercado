namespace ReciboSuperMercado.Dominio;

public class Producto
{
    public string Nombre { get; }

    public Producto(string nombre)
    {
        Nombre = nombre;
    }
}
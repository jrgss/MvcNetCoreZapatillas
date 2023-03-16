using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreZapatillas.Data;
using MvcNetCoreZapatillas.Models;
using System.Data;

#region SQL SERVER
//VUESTRO PROCEDIMIENTO DE PAGINACION DE IMAGENES DE ZAPATILLAS

//CREATE PROCEDURE SP_PROCEDURE_IMAGENES(@IDPRODUCTO INT, @POSICION INT, @NUMREGISTROS INT OUT)
//AS
//SELECT @NUMREGISTROS = COUNT(IDIMAGEN) FROM IMAGENESZAPASPRACTICA WHERE IDPRODUCTO=@IDPRODUCTO
//SELECT IDIMAGEN, IDPRODUCTO, IMAGEN FROM(
//SELECT CAST(ROW_NUMBER()OVER(ORDER BY IDPRODUCTO)AS INT) AS POSICION,
//IDIMAGEN, IDPRODUCTO, IMAGEN 
//FROM IMAGENESZAPASPRACTICA WHERE IDPRODUCTO=@IDPRODUCTO) AS QUERY 
//WHERE QUERY.POSICION >=@POSICION AND QUERY.POSICION <(@POSICION+1)
//GO

#endregion

namespace MvcNetCoreZapatillas.Repositories
{
    public class RepositoryZapatillas
    {
        private ZapatillasContext context;

        public RepositoryZapatillas(ZapatillasContext context)
        {
            this.context = context;
        }

        public async Task<List<Zapatilla>> GetZapatillasAsync()
        {
            var consulta = from datos in this.context.Zapatillas
                           select datos;

            return await consulta.ToListAsync();
        }

        public async Task<Zapatilla> FindZapatillas(int id)
        {
            var consulta = from datos in this.context.Zapatillas
                           where datos.IdProducto == id
                           select datos;
            return consulta.FirstOrDefault();
        }

        public async Task<ModelZapatillaCantidad> GetZapatillasProductoAsync(int id,int posicion)
        {

            string sql = " SP_PROCEDURE_IMAGENES @IDPRODUCTO, @POSICION, @NUMREGISTROS OUT";
            SqlParameter pamposicion = new SqlParameter("@POSICION", posicion);
            SqlParameter pamid = new SqlParameter("@IDPRODUCTo", id);
            SqlParameter pamregistros = new SqlParameter("@NUMREGISTROS", -1);
            pamregistros.Direction = ParameterDirection.Output;
            var consulta = this.context.ImagenesZapatillas.FromSqlRaw(sql, pamid, pamposicion, pamregistros);
            List<ImagenZapatilla> imagenes = await consulta.ToListAsync();
            int registros = (int)pamregistros.Value;
            ModelZapatillaCantidad model = new ModelZapatillaCantidad();
            model.ImagenZapatilla = imagenes;
            model.CantidadRegistros = registros;
            return model;

        }


    }
}

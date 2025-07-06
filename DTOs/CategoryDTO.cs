namespace Steady_Management_App.DTOs
{
    public class CategoryDTO
    {
        /// <summary>
        /// Clave primaria de la categoría
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Descripción (nombre) de la categoría
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}

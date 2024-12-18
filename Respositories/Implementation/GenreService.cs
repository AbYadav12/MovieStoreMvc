using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Respositories.Abstract;

namespace MovieStoreMvc.Respositories.Implementation
{
    public class GenreService : IGenreService
    {
        private readonly DatabaseContext ctx;
        public GenreService(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }
        public bool Add(Genre model)
        {
            try
            {
                ctx.Genre.Add(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                Console.WriteLine($"[INFO] Attempting to delete Genre with ID: {id} at {DateTime.Now}");

                var data = this.GetById(id);
                if (data == null)
                {
                    Console.WriteLine($"[WARNING] Genre with ID: {id} not found. Deletion aborted at {DateTime.Now}");
                    return false;
                }

                ctx.Genre.Remove(data);
                ctx.SaveChanges();

                Console.WriteLine($"[INFO] Successfully deleted Genre with ID: {id} at {DateTime.Now}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to delete Genre with ID: {id}. Exception: {ex.Message} at {DateTime.Now}");
                return false;
            }
        }


        public Genre? GetById(int id)
        {
            Genre? genre = ctx.Genre.FirstOrDefault(g => g.ID == id);
            if (genre == null)
            {
                // Log or handle the null case
                Console.WriteLine($"Genre with id {id} not found.");
            }
            return genre;
        }

        public IQueryable<Genre> List()
        {
           var data = ctx.Genre.AsQueryable();
            return data;
        }

        public bool Update(Genre model)
        {
            try
            {
                ctx.Genre.Update(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

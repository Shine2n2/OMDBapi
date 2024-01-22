namespace OMDBassess.Model
{
    public class MovieSearchResponse
    {
        public List<Movie>? Search { get; set; }
        public string? totalResults { get; set; }
        public string? Response { get; set; }
    }
}

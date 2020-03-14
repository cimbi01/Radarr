using System.Collections.Generic;
using System.Linq;
using NLog;
using NzbDrone.Core.Languages;
using NzbDrone.Core.Parser.Model;

namespace NzbDrone.Core.MediaFiles.MovieImport.Aggregation.Aggregators
{
    public class AggregateLanguage : IAggregateLocalMovie
    {
        private readonly Logger _logger;

        public AggregateLanguage(Logger logger)
        {
            _logger = logger;
        }

        public LocalMovie Aggregate(LocalMovie localMovie, bool otherFiles)
        {
            // Get languages in preferred order, download client item, folder and finally file.
            // Non-English languages will be preferred later, in the event there is a conflict
            // between parsed languages the more preferred item will be used.
            var languages = new List<Language>();

            languages.AddRange(GetLanguage(localMovie.DownloadClientMovieInfo));
            languages.AddRange(GetLanguage(localMovie.FolderMovieInfo));
            languages.AddRange(GetLanguage(localMovie.FileMovieInfo));

            // TODO: Fix this so we return more than one language in case of multi.
            var language = new List<Language> { languages.FirstOrDefault(l => l != Language.Unknown) ?? Language.Unknown };

            var movieLanguage = localMovie.Movie?.OriginalLanguage;

            // If unknown language from parse, fallback to the matched movie original language
            if (movieLanguage != null && language.First() == Language.Unknown)
            {
                language = new List<Language> { movieLanguage };
            }

            _logger.Debug("Using language: {0}", language.First());

            localMovie.Languages = language;

            return localMovie;
        }

        private List<Language> GetLanguage(ParsedMovieInfo parsedMovieInfo)
        {
            if (parsedMovieInfo == null)
            {
                // English is the default language when otherwise unknown
                return new List<Language> { Language.Unknown };
            }

            return parsedMovieInfo.Languages;
        }
    }
}

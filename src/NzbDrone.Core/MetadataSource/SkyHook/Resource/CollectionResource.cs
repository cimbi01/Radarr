using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NzbDrone.Core.MetadataSource.SkyHook.Resource
{
    public class CollectionResource
    {
        public string Name { get; set; }
        public int TmdbId { get; set; }
        public List<ImageResource> Images { get; set; }
    }
}

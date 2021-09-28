using Onion.Core.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using AM = AutoMapper;

namespace Onion.Infrastructure.Mapper
{
    public class Mapper : IMapper
    {
        private readonly AM.Mapper _mapper;

        public Mapper()
        {
            AM.MapperConfiguration configuration = new AM.MapperConfiguration(MappperProfile.Configure);
            _mapper = new AM.Mapper(configuration);
        }

        public TDest Map<TSource, TDest>(TSource source, Action<TDest> additionalProperties = null)
        {
            return _mapper.Map<TSource, TDest>(source, opts => 
            {
                opts.AfterMap((s, d) => 
                {
                    additionalProperties?.Invoke(d);
                });
            });
        }
    }
}

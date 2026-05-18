using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Mapping
{
    public class AppMapper : IAppMapper
    {
        //readonly private IAppMapper _mapper;
        global::AutoMapper.IMapper mapper2;
        public AppMapper(AutoMapper.IMapper mapper)
        {
            mapper2 = mapper;
        }
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper2.Map<TSource, TDestination>(source);    
        }
    }
}

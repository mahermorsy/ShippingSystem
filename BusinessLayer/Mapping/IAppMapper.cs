using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Mapping
{
    public interface IAppMapper
    {
        public TDestination Map<TSource, TDestination>(TSource source);     
    }
}

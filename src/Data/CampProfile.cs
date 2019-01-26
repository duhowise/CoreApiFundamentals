using AutoMapper;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile:Profile
    {
        public CampProfile()
        {
            this.CreateMap<Camp, CampModel>()
                .ForMember(c=>c.VenueName,o=>o.MapFrom(m=>m.Location.VenueName))  
                .ForMember(c=>c.Address1,o=>o.MapFrom(m=>m.Location.Address1))  
                .ForMember(c=>c.Address2,o=>o.MapFrom(m=>m.Location.Address2))  
                .ForMember(c=>c.Address3,o=>o.MapFrom(m=>m.Location.Address3))  
                .ForMember(c=>c.CityTown,o=>o.MapFrom(m=>m.Location.CityTown))  
                .ForMember(c=>c.Country,o=>o.MapFrom(m=>m.Location.Country))  
                .ForMember(c=>c.PostalCode,o=>o.MapFrom(m=>m.Location.PostalCode))  
                .ForMember(c=>c.StateProvince,o=>o.MapFrom(m=>m.Location.StateProvince))  
                .ForMember(c=>c.Talks, o=>o.MapFrom(m=>m.Talks))  
                .ReverseMap();
        }
    }
}
